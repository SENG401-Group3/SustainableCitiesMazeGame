using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    [Header("UI Toolkit")]
    public UIDocument document;

    [Header("Question Settings")]
    public int firstTryPoints = 10;
    public int secondTryPoints = 5;
    public int thirdTryPoints = 3;
    public int artifactReward = 1;

    [Header("Animation Settings")]
    public float feedbackDuration = 0.3f;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;
    public float shakeIntensity = 5f;

    private VisualElement root;
    private VisualElement questionPanel;
    private Label questionText;
    private Label hintText; // NEW: Hint text element
    private Button[] answerButtons = new Button[3];
    private Label feedbackText;
    private Label progressMessage;
    private Button continueButton;

    private Questions currentQuestion;
    private string currentDifficulty;
    private int cityLevel;
    private bool answered = false;
    private int attemptCount = 0;
    private bool questionCompleted = false;
    private bool isAnimating = false;

    // Track used questions across this playthrough
    private static List<string> usedQuestionIds = new List<string>();

    void Start()
    {
        // Reset used questions when starting a new game (city 1)
        if (PlayerPrefs.GetInt("CurrentCity", 1) == 1)
        {
            usedQuestionIds.Clear();
            Debug.Log("🔄 New game started - resetting used questions list");
        }

        if (document == null)
        {
            Debug.LogError("❌ UIDocument not assigned to QuestionManager!");
            return;
        }

        root = document.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("❌ Root visual element is null!");
            return;
        }

        // Find all UI elements
        questionPanel = root.Q<VisualElement>("QuestionPanel");
        questionText = root.Q<Label>("QuestionText");
        hintText = root.Q<Label>("HintText"); // NEW: Find hint text
        feedbackText = root.Q<Label>("FeedbackText");
        progressMessage = root.Q<Label>("ProgressMessage");
        continueButton = root.Q<Button>("ContinueButton");

        answerButtons[0] = root.Q<Button>("AnswerButton0");
        answerButtons[1] = root.Q<Button>("AnswerButton1");
        answerButtons[2] = root.Q<Button>("AnswerButton2");

        // Add transition styles for smooth animations
        foreach (var btn in answerButtons)
        {
            if (btn != null)
            {
                btn.style.transitionProperty = new List<StylePropertyName> { "background-color", "scale", "translate" };
                btn.style.transitionDuration = new List<TimeValue> { feedbackDuration };
                btn.style.transitionTimingFunction = new List<EasingFunction> { EasingMode.EaseOut };
            }
        }

        VerifyElements();

        cityLevel = PlayerPrefs.GetInt("CurrentCity", 1);
        Debug.Log($"🏙️ Current city level: {cityLevel}");

        SetupButtons();
        LoadQuestionForCity(cityLevel);

        // Hide UI elements initially
        if (continueButton != null)
            continueButton.style.display = DisplayStyle.None;

        if (feedbackText != null)
            feedbackText.style.display = DisplayStyle.None;

        if (progressMessage != null)
            progressMessage.style.display = DisplayStyle.None;

        if (hintText != null)
            hintText.style.display = DisplayStyle.None; // NEW: Hide hint by default
    }

    void VerifyElements()
    {
        Debug.Log("🔍 Verifying UI elements:");
        Debug.Log($"  QuestionPanel: {(questionPanel != null ? "✅" : "❌")}");
        Debug.Log($"  QuestionText: {(questionText != null ? "✅" : "❌")}");
        Debug.Log($"  HintText: {(hintText != null ? "✅" : "❌")}"); // NEW
        Debug.Log($"  AnswerButton0: {(answerButtons[0] != null ? "✅" : "❌")}");
        Debug.Log($"  AnswerButton1: {(answerButtons[1] != null ? "✅" : "❌")}");
        Debug.Log($"  AnswerButton2: {(answerButtons[2] != null ? "✅" : "❌")}");
        Debug.Log($"  FeedbackText: {(feedbackText != null ? "✅" : "❌")}");
        Debug.Log($"  ProgressMessage: {(progressMessage != null ? "✅" : "❌")}");
        Debug.Log($"  ContinueButton: {(continueButton != null ? "✅" : "❌")}");
    }

    void SetupButtons()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            if (answerButtons[i] != null)
                answerButtons[i].clicked += () => OnAnswerSelected(index);
        }

        if (continueButton != null)
            continueButton.clicked += () => OnContinueClicked();
    }

    string GetDifficultyForCity(int city)
    {
        if (city <= 2) return "easy";
        else if (city <= 4) return "medium";
        else return "hard";
    }

    void LoadQuestionForCity(int city)
    {
        currentDifficulty = GetDifficultyForCity(city);
        Debug.Log($"📚 Loading {currentDifficulty} question for city {city}");

        if (QuestionsBank.Questionss.ContainsKey(currentDifficulty))
        {
            var questions = QuestionsBank.Questionss[currentDifficulty];

            // Filter out already used questions
            var availableQuestions = questions.Where(q => !usedQuestionIds.Contains(q.Id)).ToList();

            // If all questions in this difficulty are used, allow repeats (fallback)
            if (availableQuestions.Count == 0)
            {
                Debug.LogWarning($"⚠️ All {currentDifficulty} questions have been used! Starting repeats.");
                availableQuestions = questions;
            }

            int randomIndex = Random.Range(0, availableQuestions.Count);
            currentQuestion = availableQuestions[randomIndex];

            // Mark this question as used
            usedQuestionIds.Add(currentQuestion.Id);
            Debug.Log($"📌 Using question {currentQuestion.Id}. Total used: {usedQuestionIds.Count}");

            DisplayQuestion(currentQuestion);
        }
        else
        {
            Debug.LogError($"❌ No questions found for difficulty: {currentDifficulty}");
        }
    }

    void DisplayQuestion(Questions q)
    {
        if (questionText != null)
            questionText.text = q.Prompt;

        Debug.Log($"📝 Question: {q.Prompt}");

        // NEW: Check if a hint is available (set by HintScroll)
        int hintAvailable = PlayerPrefs.GetInt("HintScrollCollected", 0);
        if (hintAvailable == 1 && hintText != null)
        {
            // Show the hint (the correct answer text)
            string hint = $"💡 Hint: The correct answer is {GetCorrectAnswerText()}";
            hintText.text = hint;
            hintText.style.color = Color.yellow;
            hintText.style.fontSize = 18;
            hintText.style.display = DisplayStyle.Flex;

            // Reset the hint flag (used up)
            PlayerPrefs.SetInt("HintScrollCollected", 0);
            PlayerPrefs.Save();
            Debug.Log("💡 Hint displayed");
        }
        else if (hintText != null)
        {
            hintText.style.display = DisplayStyle.None;
        }

        for (int i = 0; i < 3; i++)
        {
            if (i < q.Choices.Count && answerButtons[i] != null)
            {
                answerButtons[i].text = q.Choices[i];
                answerButtons[i].style.display = DisplayStyle.Flex;
                answerButtons[i].style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
                answerButtons[i].style.scale = new Scale(Vector3.one);
                answerButtons[i].style.translate = new Translate(0, 0, 0);
                Debug.Log($"  {GetLetterFromIndex(i)}: {q.Choices[i]}");
            }
            else if (answerButtons[i] != null)
            {
                answerButtons[i].style.display = DisplayStyle.None;
            }
        }

        // Hide all feedback elements initially
        if (feedbackText != null)
            feedbackText.style.display = DisplayStyle.None;

        if (progressMessage != null)
            progressMessage.style.display = DisplayStyle.None;

        if (continueButton != null)
            continueButton.style.display = DisplayStyle.None;

        attemptCount = 0;
        questionCompleted = false;
        isAnimating = false;
    }

    string GetLetterFromIndex(int index)
    {
        if (index == 0) return "A";
        if (index == 1) return "B";
        return "C";
    }

    int GetPointsForAttempt()
    {
        if (attemptCount == 1) return firstTryPoints;
        else if (attemptCount == 2) return secondTryPoints;
        else return thirdTryPoints;
    }

    string GetCorrectAnswerText()
    {
        if (currentQuestion.CorrectAnswer == "A") return currentQuestion.Choices[0];
        if (currentQuestion.CorrectAnswer == "B") return currentQuestion.Choices[1];
        return currentQuestion.Choices[2];
    }

    void OnAnswerSelected(int answerIndex)
    {
        if (questionCompleted || isAnimating) return;

        isAnimating = true;
        attemptCount++;
        string selectedLetter = GetLetterFromIndex(answerIndex);
        Debug.Log($"👆 Attempt #{attemptCount}: Player selected {selectedLetter}");

        Button selectedButton = answerButtons[answerIndex];

        if (selectedLetter == currentQuestion.CorrectAnswer)
        {
            StartCoroutine(PlayCorrectAnimation(selectedButton, answerIndex));
        }
        else
        {
            StartCoroutine(PlayIncorrectAnimation(selectedButton, answerIndex));
        }
    }

    IEnumerator PlayCorrectAnimation(Button button, int answerIndex)
    {
        button.style.translate = new Translate(0, 0, 0);
        button.style.backgroundColor = new StyleColor(correctColor);
        button.style.scale = new Scale(new Vector3(1.1f, 1.1f, 1f));

        yield return new WaitForSeconds(feedbackDuration);

        button.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
        button.style.scale = new Scale(Vector3.one);

        yield return new WaitForSeconds(0.1f);

        int pointsEarned = GetPointsForAttempt();
        Debug.Log($"✅ Correct on attempt #{attemptCount}! Earning {pointsEarned} points");

        questionCompleted = true;

        if (feedbackText != null)
        {
            feedbackText.text = $"Correct! +{pointsEarned} points";
            feedbackText.style.color = Color.green;
            feedbackText.style.display = DisplayStyle.Flex;
        }

        foreach (var btn in answerButtons)
        {
            if (btn != null)
                btn.style.display = DisplayStyle.None;
        }

        if (continueButton != null)
            continueButton.style.display = DisplayStyle.Flex;

        CityGameManager gameManager = FindFirstObjectByType<CityGameManager>();
        if (gameManager != null)
        {
            for (int i = 0; i < pointsEarned; i++)
            {
                gameManager.AddScoreAndArtifacts();
            }
            Debug.Log($"💰 Awarded {pointsEarned} points");
        }

        isAnimating = false;
    }

    IEnumerator PlayIncorrectAnimation(Button button, int answerIndex)
    {
        // Shake effect using translate
        for (int i = 0; i < 3; i++)
        {
            button.style.translate = new Translate(shakeIntensity, 0, 0);
            yield return new WaitForSeconds(0.05f);
            button.style.translate = new Translate(-shakeIntensity, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }

        button.style.translate = new Translate(0, 0, 0);
        button.style.backgroundColor = new StyleColor(incorrectColor);
        yield return new WaitForSeconds(feedbackDuration);
        button.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));

        yield return new WaitForSeconds(0.1f);

        Debug.Log($"❌ Wrong answer on attempt #{attemptCount}");

        if (attemptCount >= 3)
        {
            string correctAnswerText = GetCorrectAnswerText();
            Debug.Log($"⚠️ Out of attempts - showing correct answer: {correctAnswerText}");
            questionCompleted = true;

            if (feedbackText != null)
            {
                feedbackText.text = $"The correct answer is:\n{correctAnswerText}";
                feedbackText.style.color = Color.yellow;
                feedbackText.style.whiteSpace = WhiteSpace.Normal;
                feedbackText.style.display = DisplayStyle.Flex;
            }

            foreach (var btn in answerButtons)
            {
                if (btn != null)
                    btn.style.display = DisplayStyle.None;
            }

            if (continueButton != null)
                continueButton.style.display = DisplayStyle.Flex;
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.text = $"Try again! ({attemptCount}/3 attempts used)";
                feedbackText.style.color = Color.red;
                feedbackText.style.display = DisplayStyle.Flex;
            }

            // 🔒 BLOCK CLICKING FOR 2 SECONDS
            // Disable all answer buttons
            foreach (var btn in answerButtons)
            {
                if (btn != null)
                    btn.SetEnabled(false);
            }

            // Wait 2 seconds
            yield return new WaitForSeconds(2f);

            // Re-enable all answer buttons
            foreach (var btn in answerButtons)
            {
                if (btn != null)
                    btn.SetEnabled(true);
            }

            // Hide feedback after the delay
            if (feedbackText != null && !questionCompleted)
                feedbackText.style.display = DisplayStyle.None;
        }

        isAnimating = false;
    }

    IEnumerator HideFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (feedbackText != null && !questionCompleted)
            feedbackText.style.display = DisplayStyle.None;
    }

    void OnContinueClicked()
    {
        Debug.Log("➡️ Continue button clicked");
        int currentCity = PlayerPrefs.GetInt("CurrentCity", 1);
        StartCoroutine(ShowProgressMessageAndReturn(currentCity));
    }

    IEnumerator ShowProgressMessageAndReturn(int currentCity)
    {
        // Hide all other UI elements
        foreach (var btn in answerButtons)
        {
            if (btn != null)
                btn.style.display = DisplayStyle.None;
        }

        if (continueButton != null)
            continueButton.style.display = DisplayStyle.None;

        if (feedbackText != null)
            feedbackText.style.display = DisplayStyle.None;

        // Show progress message on question screen
        string message = "";
        if (currentCity < 5)
        {
            message = $"You are now moving to the next sustainable city!";
        }
        else
        {
            message = $"You've reached the end of the game! Returning to city 1.";
        }

        if (progressMessage != null)
        {
            progressMessage.text = message;
            progressMessage.style.color = Color.green;
            progressMessage.style.fontSize = 24;
            progressMessage.style.whiteSpace = WhiteSpace.Normal;
            progressMessage.style.display = DisplayStyle.Flex;
            Debug.Log($"📢 Progress message: {message}");
        }

        // Show message for 3 seconds
        yield return new WaitForSeconds(0.01f);

        // Calculate next city
        int nextCity;
        if (currentCity < 5)
        {
            nextCity = currentCity + 1;
            PlayerPrefs.SetInt("CurrentCity", nextCity);
            PlayerPrefs.SetInt("ShowSustainabilityMessage", nextCity);
            Debug.Log($"🏙️ Unlocked city {nextCity}");
        }
        else
        {
            Debug.Log("🏆 Game complete! Resetting to city 1.");
            usedQuestionIds.Clear();
            nextCity = 1;
            PlayerPrefs.SetInt("ShowSustainabilityMessage", 5);
            PlayerPrefs.SetInt("CurrentCity", 1);
        }

        PlayerPrefs.Save();

        // Set pending city (menu will load hidden and show correct background)
        CitySelectionMenu.pendingCityIndex = nextCity;

        // Set pending message to show on menu after it loads
        CitySelectionMenu.pendingMessage = currentCity < 5 ?
            "You are now moving to the next sustainable city!" :
            "You've reached the end of the game! Returning to city 1.";

        Debug.Log($"🎯 Set pending city to {nextCity}");

        // Load the menu scene
        SceneManager.LoadScene("CitySelection");
    }
}