using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardSceneController : MonoBehaviour
{
    public UIDocument document;

    private VisualElement root;
    private VisualElement leaderboardContainer;
    private VisualElement scoreList;
    private Button submitButton;
    private Button retryButton;
    private Button backToMenuButton;
    private VisualElement loadingSpinner;
    private Label statusMessage;

    private int playerScore;
    private string playerName = "MAKUO";
    private bool isSubmitting = false;
    private bool scoreSubmitted = false;

    void Start()
    {
        if (document == null)
        {
            document = GetComponent<UIDocument>();

            if (document == null)
            {
                Debug.LogError("❌ LeaderboardSceneController: UIDocument not assigned and not found on GameObject!");
                return;
            }
        }

        root = document.rootVisualElement;

        // Get UI elements
        leaderboardContainer = root.Q<VisualElement>("LeaderboardContainer");
        scoreList = root.Q<VisualElement>("ScoreList");
        submitButton = root.Q<Button>("SubmitButton");
        retryButton = root.Q<Button>("RetryButton");
        backToMenuButton = root.Q<Button>("BackToMenuButton");
        loadingSpinner = root.Q<VisualElement>("LoadingSpinner");
        statusMessage = root.Q<Label>("StatusMessage");

        // Get the final score with detailed debugging
        playerScore = PlayerPrefs.GetInt("TotalScore", 0);
        Debug.Log($"🏆 LeaderboardScene - Final TotalScore from PlayerPrefs: {playerScore}");

        // Check ALL score-related PlayerPrefs
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

        // Check temp scores
        for (int i = 1; i <= 5; i++)
        {
            int tempScore = PlayerPrefs.GetInt($"City{i}TempScore", -1);
            if (tempScore != -1)
                Debug.Log($"   City{i}TempScore: {tempScore}");
        }

        // Setup buttons
        if (submitButton != null)
            submitButton.clicked += OnSubmitClicked;

        if (retryButton != null)
            retryButton.clicked += OnRetryClicked;

        if (backToMenuButton != null)
            backToMenuButton.clicked += () => {
                Debug.Log("🔴 Back to Menu button clicked - resetting game...");
                // Reset everything for a new game
                ResetGameForNewPlaythrough();
                Debug.Log("🏁 Returning to main menu - reset to City 1");
                SceneManager.LoadScene("CitySelection");
            };

        // Hide elements initially
        if (retryButton != null) retryButton.style.display = DisplayStyle.None;
        if (loadingSpinner != null) loadingSpinner.style.display = DisplayStyle.None;
        if (statusMessage != null) statusMessage.style.display = DisplayStyle.None;

        // Hide the score list initially - show only after submission
        if (scoreList != null)
        {
            scoreList.style.display = DisplayStyle.None;
        }

        // Animate the leaderboard entrance
        StartCoroutine(AnimateLeaderboardEntrance());
    }

    // Add this new method to reset all game data
    void ResetGameForNewPlaythrough()
    {
        Debug.Log("🔄 ===== RESETTING GAME FOR NEW PLAYTHROUGH =====");

        // Check score before reset
        int beforeReset = PlayerPrefs.GetInt("TotalScore", 0);
        Debug.Log($"📊 TotalScore BEFORE reset: {beforeReset}");

        // Reset to City 1
        PlayerPrefs.SetInt("CurrentCity", 1);

        // Clear game complete flag
        PlayerPrefs.SetInt("GameComplete", 0);

        // RESET TOTAL SCORE TO ZERO for new game
        PlayerPrefs.SetInt("TotalScore", 0);

        // Reset individual city scores
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

        PlayerPrefs.Save();

        // Verify reset
        int afterReset = PlayerPrefs.GetInt("TotalScore", 0);
        Debug.Log($"📊 TotalScore AFTER reset: {afterReset}");
        Debug.Log("✅ ===== GAME RESET COMPLETE =====");
    }

    IEnumerator AnimateLeaderboardEntrance()
    {
        if (leaderboardContainer == null) yield break;

        // Start small and transparent
        leaderboardContainer.style.opacity = 0;
        leaderboardContainer.style.scale = new StyleScale(new Scale(new Vector2(0.8f, 0.8f)));

        // Animate to full size
        float elapsed = 0;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Ease out back for bounce effect
            float scaleT = Mathf.Sin(t * Mathf.PI * 0.5f);

            leaderboardContainer.style.opacity = t;
            leaderboardContainer.style.scale = new StyleScale(new Scale(new Vector2(
                0.8f + 0.2f * scaleT,
                0.8f + 0.2f * scaleT
            )));

            yield return null;
        }

        leaderboardContainer.style.opacity = 1;
        leaderboardContainer.style.scale = new StyleScale(new Scale(Vector2.one));
    }

    void OnSubmitClicked()
    {
        if (isSubmitting || scoreSubmitted) return;
        Debug.Log("📤 Submit button clicked - starting submission");
        StartCoroutine(SubmitScoreRoutine());
    }

    void OnRetryClicked()
    {
        if (isSubmitting) return;
        Debug.Log("🔄 Retry button clicked");

        // Hide retry button with animation
        StartCoroutine(AnimateHideRetryButton());
        StartCoroutine(SubmitScoreRoutine());
    }

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
        retryButton.style.opacity = 1;
    }

    IEnumerator SubmitScoreRoutine()
    {
        isSubmitting = true;
        Debug.Log("⏳ Starting submission routine...");

        // Disable submit button
        if (submitButton != null) submitButton.SetEnabled(false);

        // Show loading spinner with animation
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

        // Show status message
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

            // Pulse animation while submitting
            StartCoroutine(AnimatePulseText());
        }

        // Simulate network delay (2 seconds)
        float submitElapsed = 0;
        while (submitElapsed < 2f)
        {
            submitElapsed += Time.deltaTime;

            // Update dots animation
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
            // SUCCESS ANIMATION
            scoreSubmitted = true;
            Debug.Log($"✅ Submission successful! Score: {playerScore}");

            if (statusMessage != null)
            {
                statusMessage.text = "✓ SCORE SUBMITTED!";
                statusMessage.style.color = Color.green;

                // Pop animation
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

            // NOW show the leaderboard with the score
            ShowLeaderboardWithScore();

            // Wait before hiding status
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
            // FAILURE ANIMATION
            Debug.Log("❌ Submission failed - showing retry option");

            if (statusMessage != null)
            {
                statusMessage.text = "✗ SUBMISSION FAILED";
                statusMessage.style.color = Color.red;

                // Shake animation
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

            // Re-enable submit button for retry
            if (submitButton != null) submitButton.SetEnabled(true);
        }

        isSubmitting = false;
    }

    void ShowLeaderboardWithScore()
    {
        if (scoreList == null) return;

        Debug.Log($"📊 Showing leaderboard with score: {playerScore}");

        // Show the score list
        scoreList.style.display = DisplayStyle.Flex;
        scoreList.Clear();

        // Add player as #1 with HIGHLIGHT using the ACTUAL score
        AddScoreEntry(playerName, playerScore, 1, true);
        AddScoreEntry("EcoWarrior", 1500, 2, false);
        AddScoreEntry("GreenMachine", 1200, 3, false);
        AddScoreEntry("SolarSam", 900, 4, false);
        AddScoreEntry("RecycleRex", 750, 5, false);

        // Animate the entries
        StartCoroutine(AnimateScoreEntries());

        // Animate the highlight
        StartCoroutine(AnimateHighlight());
    }

    IEnumerator AnimateScoreEntries()
    {
        if (scoreList == null) yield break;

        var entries = scoreList.Children();
        int index = 0;

        foreach (var entry in entries)
        {
            entry.style.opacity = 0;
            entry.style.translate = new StyleTranslate(new Translate(20, 0));

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

    IEnumerator AnimateHighlight()
    {
        yield return new WaitForSeconds(0.1f);

        var firstEntry = scoreList.ElementAt(0);
        if (firstEntry != null)
        {
            // Pulse animation
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

    void AddScoreEntry(string name, int score, int rank, bool highlight)
    {
        if (scoreList == null) return;

        var entry = new VisualElement();
        entry.style.flexDirection = FlexDirection.Row;
        entry.style.justifyContent = Justify.SpaceBetween;

        entry.style.paddingTop = 10;
        entry.style.paddingBottom = 10;
        entry.style.paddingLeft = 10;
        entry.style.paddingRight = 10;
        entry.style.marginBottom = 5;
        entry.style.opacity = 0;

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

        var rankLabel = new Label(rank.ToString());
        rankLabel.style.width = 40;
        rankLabel.style.unityFontStyleAndWeight = FontStyle.Bold;

        var nameLabel = new Label(name);
        nameLabel.style.width = 150;

        var scoreLabel = new Label(score.ToString());
        scoreLabel.style.width = 80;
        scoreLabel.style.unityTextAlign = TextAnchor.MiddleRight;

        entry.Add(rankLabel);
        entry.Add(nameLabel);
        entry.Add(scoreLabel);

        scoreList.Add(entry);
    }
}