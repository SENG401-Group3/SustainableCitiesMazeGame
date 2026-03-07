using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public UIDocument document;
    private VisualElement root;
    private Label questionText, feedbackText;
    private Button continueButton;
    private Button[] answerButtons = new Button[3];

    private Questions currentQuestion;
    private int attemptCount = 0;
    private bool questionCompleted = false;

    void Start()
    {
        root = document.rootVisualElement;

        // Link UI - ENSURE THESE NAMES MATCH UI BUILDER EXACTLY
        questionText = root.Q<Label>("QuestionText");
        feedbackText = root.Q<Label>("FeedbackText");
        continueButton = root.Q<Button>("ContinueButton");

        // Initialization
        if (feedbackText != null) feedbackText.text = "";
        if (continueButton != null) continueButton.style.display = DisplayStyle.None;

        for (int i = 0; i < 3; i++)
        {
            // Looking for AnswerButton0, AnswerButton1, AnswerButton2
            answerButtons[i] = root.Q<Button>($"AnswerButton{i}");
            if (answerButtons[i] != null)
            {
                int index = i;
                answerButtons[i].clicked += () => OnAnswerSelected(index);
            }
        }

        if (continueButton != null)
            continueButton.clicked += FinishAndRedirect;

        LoadQuestionForCity(PlayerPrefs.GetInt("CurrentCity", 1));
    }

    void LoadQuestionForCity(int city)
    {
        string diff = city <= 2 ? "easy" : (city <= 4 ? "medium" : "hard");

        // Ensure the bank exists and contains the difficulty
        if (QuestionsBank.Questionss != null && QuestionsBank.Questionss.ContainsKey(diff))
        {
            var available = QuestionsBank.Questionss[diff];
            currentQuestion = available[Random.Range(0, available.Count)];

            // Apply text to UI
            if (questionText != null) questionText.text = currentQuestion.Prompt;
            for (int i = 0; i < 3; i++)
            {
                if (answerButtons[i] != null) answerButtons[i].text = currentQuestion.Choices[i];
            }
        }
        else { Debug.LogError("QuestionsBank is missing or difficulty not found!"); }
    }

    void OnAnswerSelected(int index)
    {
        if (questionCompleted || currentQuestion == null) return;
        attemptCount++;

        string choice = (index == 0) ? "A" : (index == 1 ? "B" : "C");
        if (choice == currentQuestion.CorrectAnswer)
        {
            questionCompleted = true;
            // The 10, 5, 3 Point System
            int points = (attemptCount == 1) ? 10 : (attemptCount == 2 ? 5 : 3);

            if (feedbackText != null)
            {
                feedbackText.text = $"CORRECT! +{points} Points";
                feedbackText.style.color = Color.green;
                feedbackText.style.display = DisplayStyle.Flex;
            }

            CityGameManager gm = FindFirstObjectByType<CityGameManager>();
            if (gm != null) gm.AddScoreAndArtifacts(points);

            ShowEndUI();
        }
        else if (attemptCount >= 3)
        {
            if (feedbackText != null)
            {
                feedbackText.text = $"Out of tries! Answer was {currentQuestion.CorrectAnswer}";
                feedbackText.style.color = Color.yellow;
                feedbackText.style.display = DisplayStyle.Flex;
            }
            ShowEndUI();
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Incorrect, try again!";
                feedbackText.style.color = Color.red;
                feedbackText.style.display = DisplayStyle.Flex;
            }
        }
    }

    void ShowEndUI()
    {
        foreach (var b in answerButtons) if (b != null) b.style.display = DisplayStyle.None;
        if (continueButton != null) continueButton.style.display = DisplayStyle.Flex;
    }

    void FinishAndRedirect()
    {
        // Save and trigger CityUpdater logic
        PlayerPrefs.Save();
        CityUpdater updater = FindFirstObjectByType<CityUpdater>();
        if (updater != null) updater.CompleteCity();
        else SceneManager.LoadScene("CitySelection");
    }
}