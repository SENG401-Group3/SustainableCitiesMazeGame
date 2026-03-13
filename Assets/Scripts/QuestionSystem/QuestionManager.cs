using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

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

        // Link UI
        questionText = root.Q<Label>("QuestionText");
        feedbackText = root.Q<Label>("FeedbackText");
        continueButton = root.Q<Button>("ContinueButton");

        Debug.Log($"🔍 Continue button found: {continueButton != null}");

        if (feedbackText != null) feedbackText.text = "";

        if (continueButton != null)
        {
            continueButton.style.display = DisplayStyle.None;

            // SIMPLE DIRECT ASSIGNMENT - no lambda, no extra events
            continueButton.clicked += OnContinueClicked;

            Debug.Log("✅ Continue button event assigned directly");
        }
        else
        {
            Debug.LogError("❌ Continue button is NULL in Start!");
        }

        // Answer buttons
        for (int i = 0; i < 3; i++)
        {
            answerButtons[i] = root.Q<Button>($"AnswerButton{i}");
            if (answerButtons[i] != null)
            {
                int index = i;
                answerButtons[i].clicked += () => OnAnswerSelected(index);
            }
        }

        LoadQuestionForCity(PlayerPrefs.GetInt("CurrentCity", 1));
    }

    // DIRECT METHOD for continue button
    void OnContinueClicked()
    {
        Debug.Log("🎯🎯🎯 ON CONTINUE CLICKED EXECUTED! 🎯🎯🎯");
        Debug.Log("🔴 FINISH AND REDIRECT WAS CALLED!");

        PlayerPrefs.Save();
        int currentCity = PlayerPrefs.GetInt("CurrentCity", 1);
        Debug.Log($"🔄 Current city: {currentCity}");

        if (CityUpdater.Instance != null)
        {
            Debug.Log("✅ CityUpdater.Instance found");
            CityUpdater.Instance.CompleteCity();
        }
        else
        {
            Debug.LogError("❌ CityUpdater instance not found!");
            SceneManager.LoadScene("CitySelection");
        }
    }

    void LoadQuestionForCity(int city)
    {
        string diff = city <= 2 ? "easy" : (city <= 4 ? "medium" : "hard");

        if (QuestionsBank.Questionss != null && QuestionsBank.Questionss.ContainsKey(diff))
        {
            var available = QuestionsBank.Questionss[diff];
            currentQuestion = available[Random.Range(0, available.Count)];

            if (questionText != null) questionText.text = currentQuestion.Prompt;
            for (int i = 0; i < 3; i++)
            {
                if (answerButtons[i] != null) answerButtons[i].text = currentQuestion.Choices[i];
            }
        }
        else { Debug.LogError("QuestionsBank missing!"); }
    }

    void OnAnswerSelected(int index)
    {
        if (questionCompleted || currentQuestion == null) return;
        attemptCount++;

        string choice = (index == 0) ? "A" : (index == 1 ? "B" : "C");
        Debug.Log($"📝 Answer: {choice}, Attempt: {attemptCount}");

        if (choice == currentQuestion.CorrectAnswer)
        {
            questionCompleted = true;
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
        Debug.Log("📢 ShowEndUI() called");

        foreach (var b in answerButtons)
        {
            if (b != null) b.style.display = DisplayStyle.None;
        }

        if (continueButton != null)
        {
            continueButton.style.display = DisplayStyle.Flex;
            continueButton.SetEnabled(true);
            continueButton.Focus(); // Try to force focus
            Debug.Log("✅ Continue button visible and enabled");
        }
    }

    void Update()
    {
        // Press F8 to check button
        if (Keyboard.current != null && Keyboard.current.f8Key.wasPressedThisFrame)
        {
            if (continueButton != null)
            {
                Debug.Log($"🔍 Button display: {continueButton.style.display.value}");
                Debug.Log($"🔍 Button enabled: {continueButton.enabledSelf}");
            }
        }

        // Press F9 to simulate click
        if (Keyboard.current != null && Keyboard.current.f9Key.wasPressedThisFrame)
        {
            if (continueButton != null && continueButton.style.display.value == DisplayStyle.Flex)
            {
                Debug.Log("🟢 SIMULATING BUTTON CLICK");
                OnContinueClicked();
            }
        }
    }
}