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
    private Label promptText;
    private Label questionText, feedbackText;
    private Button continueButton;
    private Button[] answerButtons = new Button[3];

    private Questions currentQuestion;
    private int correctIndex;
    private List<string> shuffledChoices;
    private int attemptCount = 0;
    private bool questionCompleted = false;

    void Start()
    {
        root = document.rootVisualElement;

        // Link UI
        promptText = root.Q<Label>("PromptText");
        questionText = root.Q<Label>("QuestionText");
        feedbackText = root.Q<Label>("FeedbackText");
        continueButton = root.Q<Button>("ContinueButton");

        Debug.Log($"🔍 Continue button found: {continueButton != null}");

        if (feedbackText != null) feedbackText.text = "";

        if (continueButton != null)
        {
            continueButton.style.display = DisplayStyle.None;
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
        if (QuestionsBank.AllQuestions == null || QuestionsBank.AllQuestions.Count == 0)
        {
            Debug.LogError("QuestionsBank is empty!");
            return;
        }

        currentQuestion = QuestionsBank.AllQuestions[Random.Range(0, QuestionsBank.AllQuestions.Count)];

        shuffledChoices = QuestionsBank.GetShuffledChoices(currentQuestion, out correctIndex);

        if (questionText != null)
            questionText.text = $"{QuestionsBank.PromptPrefix}\n{currentQuestion.Prompt}";

        for (int i = 0; i < 3; i++)
        {
            if (answerButtons[i] != null)
                answerButtons[i].text = shuffledChoices[i];
        }
    }

    void OnAnswerSelected(int index)
    {
        if (questionCompleted || currentQuestion == null) return;
        attemptCount++;

        // Check if the selected index matches the index where the correct answer was placed after shuffling
        bool isCorrect = (index == correctIndex);

        Debug.Log($"📝 Answer selected at index: {index}, Correct index: {correctIndex}, Is correct: {isCorrect}");

        // Make sure feedback text is visible for all attempts
        if (feedbackText != null)
        {
            feedbackText.style.display = DisplayStyle.Flex;
        }

        if (isCorrect)
        {
            questionCompleted = true;
            int points = (attemptCount == 1) ? 10 : (attemptCount == 2 ? 5 : 3);

            Debug.Log($"✅ CORRECT! Awarding {points} points on attempt {attemptCount}");

            if (feedbackText != null)
            {
                feedbackText.text = $"✅ CORRECT! +{points} Points";
                feedbackText.style.color = Color.green;
            }

            CityGameManager gm = FindFirstObjectByType<CityGameManager>();
            if (gm != null)
            {
                gm.AddScoreAndArtifacts(points);
                Debug.Log($"✅ Added {points} points to CityGameManager. Total now: {gm.GetPlayerScore()}");
            }
            else
            {
                Debug.LogError("❌ CityGameManager not found!");
            }

            ShowEndUI();
        }
        else if (attemptCount >= 3)
        {
            // Get the original correct answer letter
            string correctLetter = currentQuestion.CorrectAnswer;
            // Find what that answer text is
            string correctAnswerText = currentQuestion.Choices.Find(c => c.StartsWith(correctLetter + ")"))?.Substring(3) ?? "Unknown";

            Debug.Log($"❌ Out of tries! Correct answer was {correctLetter}: {correctAnswerText}");

            if (feedbackText != null)
            {
                feedbackText.text = $"❌ No tries left! The correct answer was {correctLetter}: {correctAnswerText}";
                feedbackText.style.color = Color.yellow;
            }
            ShowEndUI();
        }
        else
        {
            int triesLeft = 3 - attemptCount;
            string triesText = triesLeft == 1 ? "try" : "tries";
            Debug.Log($"❌ Incorrect! {triesLeft} {triesText} left.");

            if (feedbackText != null)
            {
                feedbackText.text = $"❌ Incorrect! {triesLeft} {triesText} left.";
                feedbackText.style.color = Color.red;
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
            continueButton.Focus();
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