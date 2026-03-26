using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.Networking;

public class LeaderboardSceneController : MonoBehaviour
{
    //public UIDocument document;

    private VisualElement root;
    private VisualElement leaderboardContainer;
    private VisualElement scoreList;
    private Button submitButton;
    private Button retryButton;
    private Button backToMenuButton;
    private VisualElement loadingSpinner;
    private Label statusMessage;
    private Label completeLabel;
    private Label expertLabel;

    private int playerScore;
    private string playerName;
    private bool isSubmitting = false;
    private bool scoreSubmitted = false;
    private string[] playerNames;
    private int[] playerScores;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        //gameComplete = false;

        // Get UI elements
        leaderboardContainer = root.Q<VisualElement>("LeaderboardContainer");
        scoreList = root.Q<VisualElement>("ScoreList");
        submitButton = root.Q<Button>("SubmitButton");
        retryButton = root.Q<Button>("RetryButton");
        backToMenuButton = root.Q<Button>("BackToMenuButton");
        loadingSpinner = root.Q<VisualElement>("LoadingSpinner");
        statusMessage = root.Q<Label>("StatusMessage");
        completeLabel = root.Q<Label>("CompletionLabel");
        expertLabel = root.Q<Label>("ExpertLabel");

        //Hide labels until game complete
        completeLabel.style.display = DisplayStyle.None;
        expertLabel.style.display = DisplayStyle.None;

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
                if(DBManager.username == "Guest")
                {
                    ResetGameForNewPlaythrough();
                }
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
        playerName = DBManager.LoggedIn ? DBManager.username : "";

        if(DBManager.gameComplete)
        {
            completeLabel.style.display = DisplayStyle.Flex;
            expertLabel.style.display = DisplayStyle.Flex;
        }
        UIAnimator.Instance.AnimateEntrance(leaderboardContainer);
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
        UIAnimator.Instance.FadeOutElement(retryButton, 0.2f);
        StartCoroutine(SubmitScoreRoutine());
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
            UIAnimator.Instance.FadeInElement(loadingSpinner, 0.2f);
            UIAnimator.Instance.RotateElement(loadingSpinner, 360f);
        }

        // Show status message
        if (statusMessage != null)
        {
            statusMessage.text = "Submitting score...";
            statusMessage.style.color = Color.white;
            UIAnimator.Instance.FadeInElement(statusMessage, 0.2f);
            UIAnimator.Instance.PulseElement(statusMessage);
        }

        // Simulate network delay (2 seconds)
        yield return UIAnimator.Instance.AnimateLoading(statusMessage, "Submitting score", 2f);

        // Simulate success/failure (80% success rate)
        bool success = Random.Range(0, 10) > 2;
        Debug.Log($"📊 Submission result: {(success ? "SUCCESS" : "FAILURE")}");

        // Hide spinner
        if (loadingSpinner != null)
        {
            UIAnimator.Instance.FadeOutElement(loadingSpinner, 0.2f);
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
                UIAnimator.Instance.PulseElement(statusMessage, 0.3f, 1.2f);
            }

            // NOW show the leaderboard with the score
            yield return ShowLeaderboardWithScore();

            // Wait before hiding status
            yield return new WaitForSeconds(1.5f);

            // Hide status message
            if (statusMessage != null)
            {
                UIAnimator.Instance.FadeOutElement(statusMessage, 0.2f);
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
                UIAnimator.Instance.ShakeElement(statusMessage, 0.5f, 5f, 2f);
            }

            // Show retry button with pop animation
            if (retryButton != null)
            {
                retryButton.style.display = DisplayStyle.Flex;
                UIAnimator.Instance.AnimateEntrance(retryButton, 0.3f);
            }

            // Re-enable submit button for retry
            if (submitButton != null) submitButton.SetEnabled(true);
        }

        isSubmitting = false;
    }

    IEnumerator ShowLeaderboardWithScore()
    {
        if (scoreList == null) yield break;

        Debug.Log($"📊 Showing leaderboard with score: {playerScore}");

        yield return FetchLeaderboardData();

        // Show the score list
        scoreList.style.display = DisplayStyle.Flex;
        scoreList.Clear();

        // Add player as #1 with HIGHLIGHT using the ACTUAL score
        int rank = 1;

        for (int i = 0; i < playerNames.Length && i < playerScores.Length; i++)
        {
            if (playerNames[i] == playerName)
            {
                AddScoreEntry(playerNames[i], playerScores[i], rank, true);
            }
            else
            {
                AddScoreEntry(playerNames[i], playerScores[i], rank, false);
            }
            rank++;
        }

        // Animate the entries
        StartCoroutine(AnimateScoreEntries());

        // Animate the highlight
        //StartCoroutine(AnimateHighlight());
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

            UIAnimator.Instance.SlideInElement(entry, new Vector2(20, 0), 0.3f);
            index++;
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
        rankLabel.style.color = highlight ? Color.green : Color.white;

        var nameLabel = new Label(name);
        nameLabel.style.width = 150;
        nameLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
        nameLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        nameLabel.style.color = highlight ? Color.green : Color.white;

        var scoreLabel = new Label(score.ToString());
        scoreLabel.style.width = 80;
        scoreLabel.style.unityTextAlign = TextAnchor.MiddleRight;
        scoreLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        scoreLabel.style.color = highlight ? Color.green : Color.white;

        entry.Add(rankLabel);
        entry.Add(nameLabel);
        entry.Add(scoreLabel);

        scoreList.Add(entry);
    }

    IEnumerator FetchLeaderboardData()
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest request = UnityWebRequest.Post(DBManager.hostname + "/leaderboard.php", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Leaderboard data fetched:\n" + request.downloadHandler.text);
                Leaderboard leaderboard = JsonUtility.FromJson<Leaderboard>(request.downloadHandler.text);
                Debug.Log($"Parsed leaderboard - Username: {leaderboard.username}, Highscore: {leaderboard.highscore}, Error: {leaderboard.error}");

                // if (leaderboard.error != null)
                // {
                //     Debug.LogError("Error in leaderboard response: " + leaderboard.error);
                //     yield break;
                // }
                playerNames = leaderboard.username;
                playerScores = leaderboard.highscore;
            }
            else
            {
                Debug.LogError("Error fetching leaderboard: " + request.error);
            }
        }
    }
}

// helper class for parsing the returned json object from the leaderboard request
    [System.Serializable]
    public class Leaderboard
    {
        public string[] username;
        public int[] highscore;
        public string error;
    }
