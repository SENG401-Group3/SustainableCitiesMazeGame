using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/* Manages the question UI flow, including displaying questions,
 handling answer selection, scoring, and progression. */

public class QuestionManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private UIDocument document; // Reference to the UIDocument containing the question UI
    [Tooltip("The root VisualElement of the UI document")]
    private VisualElement root;

    [Header("UI Elements")]
    private Label promptText;        // The static prompt text (e.g., "The city is changing...")
    private Label questionText;       // The specific question text
    private Label feedbackText;       // Text showing correct/incorrect feedback
    private Label hintText;           // Text showing hint for the question
    private Button continueButton;    // Button to continue after answering
    private Button[] answerButtons = new Button[3]; // Array of 3 answer buttons

    [Header("Question Data")]
    private Questions currentQuestion;           // The current question object
    private int correctIndex;                     // Index of correct answer after shuffling
    private List<string> shuffledChoices;         // Shuffled answer choices
    private int attemptCount = 0;                  // Current attempt number (1-3)
    private bool questionCompleted = false;        // Whether question has been answered correctly
    private bool hasHint = false;                  // Whether player has collected a hint scroll

    /* Unity's Start method - initializes UI elements and loads the first question.*/
    public void Start()
    {
        // Validate UIDocument reference
        if (document == null)
        {
            Debug.LogError("❌ QuestionManager: UIDocument is not assigned!");
            return;
        }

        // Get the root VisualElement
        root = document.rootVisualElement;

        // Find all UI elements by their names
        promptText = root.Q<Label>("PromptText");
        questionText = root.Q<Label>("QuestionText");
        feedbackText = root.Q<Label>("FeedbackText");
        hintText = root.Q<Label>("HintText");
        continueButton = root.Q<Button>("ContinueButton");

        Debug.Log($"🔍 Continue button found: {continueButton != null}");
        Debug.Log($"🔍 Hint text found: {hintText != null}");

        // Initialize feedback text
        if (feedbackText != null)
            feedbackText.text = "";

        // Hide hint initially
        if (hintText != null)
            hintText.style.display = DisplayStyle.None;

        // Setup continue button
        if (continueButton != null)
        {
            continueButton.style.display = DisplayStyle.None; // Hidden initially
            continueButton.clicked += OnContinueClicked;
            Debug.Log("✅ Continue button event assigned directly");
        }
        else
        {
            Debug.LogError("❌ Continue button is NULL in Start!");
        }

        // Find and setup answer buttons (AnswerButton0, AnswerButton1, AnswerButton2)
        for (int i = 0; i < 3; i++)
        {
            answerButtons[i] = root.Q<Button>($"AnswerButton{i}");
            if (answerButtons[i] != null)
            {
                int index = i; // Capture index for closure
                answerButtons[i].clicked += () => OnAnswerSelected(index);
            }
        }

        // Check if player has hint scroll from PlayerPrefs
        CheckForHintScroll();

        // Load the first question
        LoadQuestionForCity(PlayerPrefs.GetInt("CurrentCity", 1));
    }

    /* Checks if player has collected a hint scroll using PlayerPrefs */
    private void CheckForHintScroll()
    {
        // Check PlayerPrefs for hint scroll collected flag
        // The HintScroll script sets this to 1 when collected
        hasHint = PlayerPrefs.GetInt("HintScrollCollected", 0) == 1;

        Debug.Log($"📜 Player has hint scroll (from PlayerPrefs): {hasHint}");
    }

    /* Shows the hint for the current question */
    private void ShowHint()
    {
        if (hintText != null && currentQuestion != null && !string.IsNullOrEmpty(currentQuestion.Hint))
        {
            hintText.text = $"💡 Hint: {currentQuestion.Hint}";
            hintText.style.display = DisplayStyle.Flex;
            hintText.style.color = Color.cyan;
            Debug.Log($"💡 Hint displayed: {currentQuestion.Hint}");

            // Clear the hint scroll flag since it's been used
            PlayerPrefs.SetInt("HintScrollCollected", 0);
            PlayerPrefs.Save();
            hasHint = false;
            Debug.Log("📜 Hint scroll used and cleared from PlayerPrefs");
        }
        else if (hintText != null)
        {
            Debug.Log("⚠️ No hint available for this question");
        }
    }

    /* Unity's Update method - handles debug input for testing.*/
    public void Update()
    {
        // Debug: Press F8 to check button status
        if (Keyboard.current != null && Keyboard.current.f8Key.wasPressedThisFrame)
        {
            if (continueButton != null)
            {
                Debug.Log($"🔍 Button display: {continueButton.style.display.value}");
                Debug.Log($"🔍 Button enabled: {continueButton.enabledSelf}");
            }
        }

        // Debug: Press F9 to simulate button click
        if (Keyboard.current != null && Keyboard.current.f9Key.wasPressedThisFrame)
        {
            if (continueButton != null && continueButton.style.display.value == DisplayStyle.Flex)
            {
                Debug.Log("🟢 SIMULATING BUTTON CLICK");
                OnContinueClicked();
            }
        }

        // Debug: Press H to manually show hint (for testing)
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame && currentQuestion != null)
        {
            Debug.Log("🔧 Debug: Manually showing hint");
            ShowHint();
        }
    }


    /* Event handler for continue button click.
    Saves progress and redirects to next scene.*/

    private void OnContinueClicked()
    {
        Debug.Log("🎯🎯🎯 ON CONTINUE CLICKED EXECUTED! 🎯🎯🎯");
        Debug.Log("🔴 FINISH AND REDIRECT WAS CALLED!");

        // Save any pending PlayerPrefs changes
        PlayerPrefs.Save();

        int currentCity = PlayerPrefs.GetInt("CurrentCity", 1);
        Debug.Log($"🔄 Current city: {currentCity}");

        // Use CityUpdater singleton to handle city completion
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


    /* Loads a random question for the specified city.
     Shuffles answer choices and updates UI.*/

    private void LoadQuestionForCity(int city)
    {
        // Validate question bank
        if (QuestionsBank.AllQuestions == null || QuestionsBank.AllQuestions.Count == 0)
        {
            Debug.LogError("QuestionsBank is empty!");
            return;
        }

        // Select random question
        currentQuestion = QuestionsBank.AllQuestions[Random.Range(0, QuestionsBank.AllQuestions.Count)];

        // Shuffle choices and get correct answer index
        shuffledChoices = QuestionsBank.GetShuffledChoices(currentQuestion, out correctIndex);

        // Update UI with question text
        if (questionText != null)
            questionText.text = $"{QuestionsBank.PromptPrefix}\n{currentQuestion.Prompt}";

        // Update answer buttons with shuffled choices
        for (int i = 0; i < 3; i++)
        {
            if (answerButtons[i] != null)
                answerButtons[i].text = shuffledChoices[i];
        }

        // Show hint if player has one
        if (hasHint)
        {
            ShowHint();
        }
    }


    /* Handles answer button selection.
    Checks correctness, awards points, and manages attempts.*/
    private void OnAnswerSelected(int index)
    {
        // Prevent multiple selections after question is completed
        if (questionCompleted || currentQuestion == null)
            return;

        attemptCount++;

        // Check if selected index matches the shuffled correct answer position
        bool isCorrect = (index == correctIndex);

        Debug.Log($"📝 Answer selected at index: {index}, Correct index: {correctIndex}, Is correct: {isCorrect}");

        // Ensure feedback text is visible
        if (feedbackText != null)
        {
            feedbackText.style.display = DisplayStyle.Flex;
        }

        // Hide hint after first attempt (optional - you can remove this if you want hint to stay)
        if (hintText != null && attemptCount == 1)
        {
            hintText.style.display = DisplayStyle.None;
        }

        // Handle correct answer
        if (isCorrect)
        {
            questionCompleted = true;

            // Award points based on attempt count (10, 5, or 3 points)
            int points = (attemptCount == 1) ? 10 : (attemptCount == 2 ? 5 : 3);

            Debug.Log($"✅ CORRECT! Awarding {points} points on attempt {attemptCount}");

            // Show success feedback
            if (feedbackText != null)
            {
                feedbackText.text = $"✅ CORRECT! +{points} Points";
                feedbackText.style.color = Color.green;
            }

            // Add points to game manager
            CityGameManager gm = FindFirstObjectByType<CityGameManager>();
            if (gm != null)
            {
                gm.AddScore(points);
                Debug.Log($"✅ Added {points} points to CityGameManager. Total now: {gm.GetPlayerScore()}");
            }
            else
            {
                Debug.LogError("❌ CityGameManager not found!");
            }

            // Show continue button
            ShowEndUI();
        }
        // Handle out of tries (3 incorrect attempts)
        else if (attemptCount >= 3)
        {
            // Get original correct answer for feedback
            string correctLetter = currentQuestion.CorrectAnswer;
            string correctAnswerText = currentQuestion.Choices.Find(c => c.StartsWith(correctLetter + ")"))?.Substring(3) ?? "Unknown";

            Debug.Log($"❌ Out of tries! Correct answer was {correctLetter}: {correctAnswerText}");

            // Show failure feedback
            if (feedbackText != null)
            {
                feedbackText.text = $"❌ No tries left! The correct answer was {correctLetter}: {correctAnswerText}";
                feedbackText.style.color = Color.yellow;
            }

            ShowEndUI();
        }
        // Handle incorrect answer with tries remaining
        else
        {
            int triesLeft = 3 - attemptCount;
            string triesText = triesLeft == 1 ? "try" : "tries";
            Debug.Log($"❌ Incorrect! {triesLeft} {triesText} left.");

            // Show try again feedback
            if (feedbackText != null)
            {
                feedbackText.text = $"❌ Incorrect! {triesLeft} {triesText} left.";
                feedbackText.style.color = Color.red;
            }
        }
    }


    /*Hides answer buttons and shows continue button.
     Called when question is complete (correct or out of tries).*/

    private void ShowEndUI()
    {
        Debug.Log("📢 ShowEndUI() called");

        // Hide all answer buttons
        foreach (var b in answerButtons)
        {
            if (b != null)
                b.style.display = DisplayStyle.None;
        }

        // Hide hint if it's still visible
        if (hintText != null)
        {
            hintText.style.display = DisplayStyle.None;
        }

        // Show and enable continue button
        if (continueButton != null)
        {
            continueButton.style.display = DisplayStyle.Flex;
            continueButton.SetEnabled(true);
            continueButton.Focus();
            Debug.Log("✅ Continue button visible and enabled");
        }
    }
}