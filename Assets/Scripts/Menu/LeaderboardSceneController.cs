using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

/*
 * Controls the leaderboard scene including score submission,
 * animations, and UI interactions.
 */

public class LeaderboardSceneController : MonoBehaviour
{
    [Header("UI Document")]
    [SerializeField] private UIDocument document; // Reference to the UIDocument containing the leaderboard UI

    [Header("UI Elements")]
    private VisualElement root;                // Root VisualElement of the UIDocument
    private VisualElement leaderboardContainer; // Main container for leaderboard content
    private VisualElement scoreList;            // Container for score entries
    private Button submitButton;                // Button to submit score
    private Button retryButton;                  // Button to retry submission after failure
    private Button backToMenuButton;             // Button to return to main menu
    private VisualElement loadingSpinner;        // Loading spinner animation element
    private Label statusMessage;                  // Status message for submission feedback

    [Header("Score Data")]
    private int playerScore;      // Player's final score
    private string playerName = "MAKUO"; // Player name for leaderboard

    [Header("Submission State")]
    private bool isSubmitting = false;   // Whether a submission is in progress
    private bool scoreSubmitted = false;  // Whether score has been successfully submitted

    /*
     * Unity's Start method - initializes UI elements and sets up the leaderboard
     */
    void Start()
    {
        // Try to get UIDocument component if not assigned
        if (document == null)
        {
            document = GetComponent<UIDocument>();

            if (document == null)
            {
                Debug.LogError("❌ LeaderboardSceneController: UIDocument not assigned and not found on GameObject!");
                return;
            }
        }

        // Get the root VisualElement
        root = document.rootVisualElement;

        // Find all UI elements by their names (must match UXML exactly)
        leaderboardContainer = root.Q<VisualElement>("LeaderboardContainer");
        scoreList = root.Q<VisualElement>("ScoreList");
        submitButton = root.Q<Button>("SubmitButton");
        retryButton = root.Q<Button>("RetryButton");
        backToMenuButton = root.Q<Button>("BackToMenuButton");
        loadingSpinner = root.Q<VisualElement>("LoadingSpinner");
        statusMessage = root.Q<Label>("StatusMessage");

        // Get the final score from PlayerPrefs
        playerScore = PlayerPrefs.GetInt("TotalScore", 0);
        Debug.Log($"🏆 LeaderboardScene - Final TotalScore from PlayerPrefs: {playerScore}");

        // Debug: Check all score-related PlayerPrefs
        Debug.Log("📊 Checking ALL score PlayerPrefs:");

        // Check individual city scores
        int totalFromCities = 0;
        for (int i = 1; i <= 5; i++)
        {
            int cityScore = PlayerPrefs.GetInt($"City{i}Score", 0);
            Debug.Log($"   City{i}Score: {cityScore}");
            totalFromCities += cityScore;
        }
        Debug.Log($"📊 Sum of all city scores: {totalFromCities}");

        // Check temporary scores for each city
        for (int i = 1; i <= 5; i++)
        {
            int tempScore = PlayerPrefs.GetInt($"City{i}TempScore", -1);
            if (tempScore != -1)
                Debug.Log($"   City{i}TempScore: {tempScore}");
        }

        // Set up button click events
        if (submitButton != null)
            submitButton.clicked += OnSubmitClicked;

        if (retryButton != null)
            retryButton.clicked += OnRetryClicked;

        if (backToMenuButton != null)
            backToMenuButton.clicked += () => {
                Debug.Log("🔴 Back to Menu button clicked - resetting game...");
                // Reset all game data for a new playthrough
                ResetGameForNewPlaythrough();
                Debug.Log("🏁 Returning to main menu - reset to City 1");
                SceneManager.LoadScene("CitySelection");
            };

        // Hide certain UI elements initially
        if (retryButton != null) retryButton.style.display = DisplayStyle.None;
        if (loadingSpinner != null) loadingSpinner.style.display = DisplayStyle.None;
        if (statusMessage != null) statusMessage.style.display = DisplayStyle.None;

        // Hide score list initially - only show after successful submission
        if (scoreList != null)
        {
            scoreList.style.display = DisplayStyle.None;
        }

        // Play entrance animation
        StartCoroutine(AnimateLeaderboardEntrance());
    }

    /*
     * Resets all game data for a new playthrough
     * Called when returning to main menu
     */
    void ResetGameForNewPlaythrough()
    {
        Debug.Log("🔄 ===== RESETTING GAME FOR NEW PLAYTHROUGH =====");

        // Store score before reset for debug
        int beforeReset = PlayerPrefs.GetInt("TotalScore", 0);
        Debug.Log($"📊 TotalScore BEFORE reset: {beforeReset}");

        // Reset game state
        PlayerPrefs.SetInt("CurrentCity", 1);      // Start from City 1
        PlayerPrefs.SetInt("GameComplete", 0);      // Clear game complete flag
        PlayerPrefs.SetInt("TotalScore", 0);        // Reset total score to zero

        // Reset individual city scores and progress
        for (int i = 1; i <= 5; i++)
        {
            int cityScoreBefore = PlayerPrefs.GetInt($"City{i}Score", 0);
            if (cityScoreBefore > 0)
            {
                Debug.Log($"   City{i}Score was: {cityScoreBefore} - resetting to 0");
            }

            PlayerPrefs.SetInt($"City{i}Score", 0);
            PlayerPrefs.SetInt($"City{i}Completed", 0);
            PlayerPrefs.SetInt($"City{i}Artifacts", 0);
            PlayerPrefs.DeleteKey($"City{i}TempScore");
            PlayerPrefs.DeleteKey($"City{i}TempArtifacts");
        }

        // Save all changes
        PlayerPrefs.Save();

        // Verify reset
        int afterReset = PlayerPrefs.GetInt("TotalScore", 0);
        Debug.Log($"📊 TotalScore AFTER reset: {afterReset}");
        Debug.Log("✅ ===== GAME RESET COMPLETE =====");
    }

    /*
     * Animates the leaderboard panel entrance with scale and fade effects
     */
    IEnumerator AnimateLeaderboardEntrance()
    {
        if (leaderboardContainer == null) yield break;

        // Start small and transparent
        leaderboardContainer.style.opacity = 0;
        leaderboardContainer.style.scale = new StyleScale(new Scale(new Vector2(0.8f, 0.8f)));

        // Animate to full size over duration
        float elapsed = 0;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Use easing function for bounce effect
            float scaleT = Mathf.Sin(t * Mathf.PI * 0.5f);

            leaderboardContainer.style.opacity = t;
            leaderboardContainer.style.scale = new StyleScale(new Scale(new Vector2(
                0.8f + 0.2f * scaleT,
                0.8f + 0.2f * scaleT
            )));

            yield return null;
        }

        // Ensure final state is correct
        leaderboardContainer.style.opacity = 1;
        leaderboardContainer.style.scale = new StyleScale(new Scale(Vector2.one));
    }

    /*
     * Handler for submit button click
     */
    void OnSubmitClicked()
    {
        if (isSubmitting || scoreSubmitted) return;
        Debug.Log("📤 Submit button clicked - starting submission");
        StartCoroutine(SubmitScoreRoutine());
    }

    /*
     * Handler for retry button click after failed submission
     */
    void OnRetryClicked()
    {
        if (isSubmitting) return;
        Debug.Log("🔄 Retry button clicked");

        // Hide retry button with animation
        StartCoroutine(AnimateHideRetryButton());
        StartCoroutine(SubmitScoreRoutine());
    }

    /*
     * Animates hiding the retry button with fade out
     */
    IEnumerator AnimateHideRetryButton()
    {
        if (retryButton == null) yield break;

        float elapsed = 0;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            retryButton.style.opacity = 1 - (elapsed / duration);
            yield return null;
        }

        retryButton.style.display = DisplayStyle.None;
        retryButton.style.opacity = 1; // Reset opacity for next time
    }

    /*
     * Coroutine that handles the score submission process with animations
     * Simulates network delay and success/failure states
     */
    IEnumerator SubmitScoreRoutine()
    {
        isSubmitting = true;
        Debug.Log("⏳ Starting submission routine...");

        // Disable submit button to prevent double submission
        if (submitButton != null) submitButton.SetEnabled(false);

        // Show loading spinner with fade in animation
        if (loadingSpinner != null)
        {
            loadingSpinner.style.display = DisplayStyle.Flex;
            loadingSpinner.style.opacity = 0;

            float elapsed = 0;
            while (elapsed < 0.2f)
            {
                elapsed += Time.deltaTime;
                loadingSpinner.style.opacity = elapsed / 0.2f;
                yield return null;
            }
            loadingSpinner.style.opacity = 1;

            // Start spinner rotation
            StartCoroutine(AnimateSpinner());
        }

        // Show status message with fade in
        if (statusMessage != null)
        {
            statusMessage.text = "Submitting score...";
            statusMessage.style.color = Color.white;
            statusMessage.style.display = DisplayStyle.Flex;
            statusMessage.style.opacity = 0;

            float elapsed = 0;
            while (elapsed < 0.2f)
            {
                elapsed += Time.deltaTime;
                statusMessage.style.opacity = elapsed / 0.2f;
                yield return null;
            }
            statusMessage.style.opacity = 1;

            // Start pulse animation on text
            StartCoroutine(AnimatePulseText());
        }

        // Simulate network delay (2 seconds)
        float submitElapsed = 0;
        while (submitElapsed < 2f)
        {
            submitElapsed += Time.deltaTime;

            // Animate dots while waiting
            if (statusMessage != null)
            {
                int dots = ((int)(submitElapsed * 2)) % 4;
                statusMessage.text = "Submitting score" + new string('.', dots);
            }

            yield return null;
        }

        // Simulate success/failure (80% success rate)
        bool success = Random.Range(0, 10) > 2;
        Debug.Log($"📊 Submission result: {(success ? "SUCCESS" : "FAILURE")}");

        // Hide spinner
        if (loadingSpinner != null)
        {
            float elapsed = 0;
            while (elapsed < 0.2f)
            {
                elapsed += Time.deltaTime;
                loadingSpinner.style.opacity = 1 - (elapsed / 0.2f);
                yield return null;
            }
            loadingSpinner.style.display = DisplayStyle.None;
        }

        if (success)
        {
            // Handle successful submission
            scoreSubmitted = true;
            Debug.Log($"✅ Submission successful! Score: {playerScore}");

            if (statusMessage != null)
            {
                statusMessage.text = "✓ SCORE SUBMITTED!";
                statusMessage.style.color = Color.green;

                // Pop animation on success message
                float elapsed = 0;
                while (elapsed < 0.3f)
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / 0.3f;
                    float scale = 1 + Mathf.Sin(t * Mathf.PI) * 0.2f;
                    statusMessage.style.scale = new StyleScale(new Scale(new Vector2(scale, scale)));
                    yield return null;
                }
                statusMessage.style.scale = new StyleScale(new Scale(Vector2.one));
            }

            // Show the leaderboard with player's score
            ShowLeaderboardWithScore();

            // Wait before hiding success message
            yield return new WaitForSeconds(1.5f);

            // Hide status message
            if (statusMessage != null)
            {
                float elapsed = 0;
                while (elapsed < 0.2f)
                {
                    elapsed += Time.deltaTime;
                    statusMessage.style.opacity = 1 - (elapsed / 0.2f);
                    yield return null;
                }
                statusMessage.style.display = DisplayStyle.None;
            }

            // Hide submit button after successful submission
            if (submitButton != null)
            {
                submitButton.style.display = DisplayStyle.None;
            }
        }
        else
        {
            // Handle failed submission
            Debug.Log("❌ Submission failed - showing retry option");

            if (statusMessage != null)
            {
                statusMessage.text = "✗ SUBMISSION FAILED";
                statusMessage.style.color = Color.red;

                // Shake animation on failure message
                float shakeElapsed = 0;
                while (shakeElapsed < 0.5f)
                {
                    shakeElapsed += Time.deltaTime;
                    float offsetX = Random.Range(-5f, 5f);
                    float offsetY = Random.Range(-2f, 2f);
                    statusMessage.style.translate = new StyleTranslate(new Translate(offsetX, offsetY));
                    yield return null;
                }
                statusMessage.style.translate = new StyleTranslate(new Translate(0, 0));
            }

            // Show retry button with pop animation
            if (retryButton != null)
            {
                retryButton.style.display = DisplayStyle.Flex;
                retryButton.style.opacity = 0;
                retryButton.style.scale = new StyleScale(new Scale(new Vector2(0.8f, 0.8f)));

                float elapsed = 0;
                while (elapsed < 0.3f)
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / 0.3f;
                    retryButton.style.opacity = t;
                    retryButton.style.scale = new StyleScale(new Scale(new Vector2(
                        0.8f + 0.2f * t,
                        0.8f + 0.2f * t
                    )));
                    yield return null;
                }

                retryButton.style.opacity = 1;
                retryButton.style.scale = new StyleScale(new Scale(Vector2.one));
            }

            // Re-enable submit button for another attempt
            if (submitButton != null) submitButton.SetEnabled(true);
        }

        isSubmitting = false;
    }

    /*
     * Displays the leaderboard with player score after successful submission
     */
    void ShowLeaderboardWithScore()
    {
        if (scoreList == null) return;

        Debug.Log($"📊 Showing leaderboard with score: {playerScore}");

        // Make score list visible
        scoreList.style.display = DisplayStyle.Flex;
        scoreList.Clear();

        // Add player as #1 with highlight
        AddScoreEntry(playerName, playerScore, 1, true);

        // Add dummy competitor scores for context
        AddScoreEntry("EcoWarrior", 1500, 2, false);
        AddScoreEntry("GreenMachine", 1200, 3, false);
        AddScoreEntry("SolarSam", 900, 4, false);
        AddScoreEntry("RecycleRex", 750, 5, false);

        // Animate entries appearing
        StartCoroutine(AnimateScoreEntries());

        // Animate player's highlighted entry
        StartCoroutine(AnimateHighlight());
    }

    /*
     * Animates score entries appearing with staggered fade-in
     */
    IEnumerator AnimateScoreEntries()
    {
        if (scoreList == null) yield break;

        var entries = scoreList.Children();
        int index = 0;

        foreach (var entry in entries)
        {
            entry.style.opacity = 0;
            entry.style.translate = new StyleTranslate(new Translate(20, 0));

            // Stagger animations
            yield return new WaitForSeconds(0.1f * index);

            float elapsed = 0;
            float duration = 0.3f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                entry.style.opacity = t;
                entry.style.translate = new StyleTranslate(new Translate(
                    Mathf.Lerp(20, 0, t),
                    0
                ));

                yield return null;
            }

            entry.style.opacity = 1;
            entry.style.translate = new StyleTranslate(new Translate(0, 0));
            index++;
        }
    }

    /*
     * Animates the loading spinner rotation
     */
    IEnumerator AnimateSpinner()
    {
        float rotation = 0;
        while (loadingSpinner != null && loadingSpinner.style.display == DisplayStyle.Flex)
        {
            rotation += 360 * Time.deltaTime;
            loadingSpinner.style.rotate = new StyleRotate(new Rotate(Angle.Degrees(rotation % 360)));
            yield return null;
        }
    }

    /*
     * Animates status text with gentle pulse effect
     */
    IEnumerator AnimatePulseText()
    {
        float elapsed = 0;
        while (statusMessage != null && statusMessage.style.display == DisplayStyle.Flex)
        {
            elapsed += Time.deltaTime;
            float pulse = 1 + Mathf.Sin(elapsed * 8) * 0.05f;
            statusMessage.style.scale = new StyleScale(new Scale(new Vector2(pulse, pulse)));
            yield return null;
        }
    }

    /*
     * Animates the highlighted player entry with pulse effect
     */
    IEnumerator AnimateHighlight()
    {
        yield return new WaitForSeconds(0.1f);

        var firstEntry = scoreList.ElementAt(0);
        if (firstEntry != null)
        {
            float elapsed = 0;
            while (elapsed < 1f)
            {
                elapsed += Time.deltaTime;
                float pulse = 1 + Mathf.Sin(elapsed * 10) * 0.05f;
                firstEntry.style.scale = new StyleScale(new Scale(new Vector2(pulse, pulse)));
                yield return null;
            }
            firstEntry.style.scale = new StyleScale(new Scale(Vector2.one));
        }
    }

    /*
     * Creates and adds a score entry to the leaderboard
     * 
     * @param name - Player name to display
     * @param score - Player score
     * @param rank - Rank position
     * @param highlight - Whether this entry should be highlighted
     */
    void AddScoreEntry(string name, int score, int rank, bool highlight)
    {
        if (scoreList == null) return;

        // Create container for this score entry
        var entry = new VisualElement();
        entry.style.flexDirection = FlexDirection.Row;
        entry.style.justifyContent = Justify.SpaceBetween;

        // Add padding and spacing
        entry.style.paddingTop = 10;
        entry.style.paddingBottom = 10;
        entry.style.paddingLeft = 10;
        entry.style.paddingRight = 10;
        entry.style.marginBottom = 5;
        entry.style.opacity = 0; // Start invisible for animation

        // Apply different styling for highlighted entries
        if (highlight)
        {
            entry.style.backgroundColor = new Color(0.3f, 0.8f, 0.3f, 0.3f);
            entry.style.borderTopWidth = 2;
            entry.style.borderBottomWidth = 2;
            entry.style.borderLeftWidth = 2;
            entry.style.borderRightWidth = 2;
            entry.style.borderTopColor = Color.green;
            entry.style.borderBottomColor = Color.green;
            entry.style.borderLeftColor = Color.green;
            entry.style.borderRightColor = Color.green;
        }
        else
        {
            entry.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        }

        // Create rank label
        var rankLabel = new Label(rank.ToString());
        rankLabel.style.width = 40;
        rankLabel.style.unityFontStyleAndWeight = FontStyle.Bold;

        // Create name label
        var nameLabel = new Label(name);
        nameLabel.style.width = 150;

        // Create score label
        var scoreLabel = new Label(score.ToString());
        scoreLabel.style.width = 80;
        scoreLabel.style.unityTextAlign = TextAnchor.MiddleRight;

        // Add all labels to entry
        entry.Add(rankLabel);
        entry.Add(nameLabel);
        entry.Add(scoreLabel);

        // Add entry to score list
        scoreList.Add(entry);
    }
}