using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI")]
    public UIDocument document;

    private VisualElement root;
    private VisualElement leaderboardPanel;
    private VisualElement scoreList;
    private Button submitButton;
    private Button retryButton;
    private Button closeButton;
    private VisualElement loadingSpinner;
    private Label statusMessage;

    // Store pending data for retries
    private string pendingName;
    private int pendingScore;

    void Start()
    {
        if (document == null)
        {
            Debug.LogError("❌ LeaderboardManager: UIDocument not assigned!");
            return;
        }

        root = document.rootVisualElement;

        if (root == null)
        {
            Debug.LogError("❌ LeaderboardManager: Root visual element is null!");
            return;
        }

        // Find all UI elements
        leaderboardPanel = root.Q<VisualElement>("LeaderboardPanel");
        scoreList = root.Q<VisualElement>("ScoreList");
        submitButton = root.Q<Button>("SubmitButton");
        retryButton = root.Q<Button>("RetryButton");
        closeButton = root.Q<Button>("CloseLeaderboardButton");
        loadingSpinner = root.Q<VisualElement>("LoadingSpinner");
        statusMessage = root.Q<Label>("StatusMessage");

        // Debug: See what was found
        Debug.Log("=== LeaderboardManager Element Check ===");
        Debug.Log($"LeaderboardPanel found: {leaderboardPanel != null}");
        Debug.Log($"ScoreList found: {scoreList != null}");
        Debug.Log($"SubmitButton found: {submitButton != null}");
        Debug.Log($"RetryButton found: {retryButton != null}");
        Debug.Log($"CloseButton found: {closeButton != null}");
        Debug.Log($"LoadingSpinner found: {loadingSpinner != null}");
        Debug.Log($"StatusMessage found: {statusMessage != null}");
        Debug.Log("========================================");

        // Hide elements initially
        if (retryButton != null) retryButton.style.display = DisplayStyle.None;
        if (loadingSpinner != null) loadingSpinner.style.display = DisplayStyle.None;
        if (statusMessage != null) statusMessage.style.display = DisplayStyle.None;

        // Add button listeners
        if (submitButton != null) submitButton.clicked += OnSubmitClicked;
        if (retryButton != null) retryButton.clicked += OnRetryClicked;
        if (closeButton != null) closeButton.clicked += HideLeaderboard;

        LoadDummyScores();
    }

    public void ShowLeaderboard()
    {
        Debug.Log("ShowLeaderboard called");
        if (leaderboardPanel != null)
        {
            leaderboardPanel.style.display = DisplayStyle.Flex;
            leaderboardPanel.BringToFront();
            Debug.Log("Leaderboard panel shown");
        }
        else
        {
            Debug.LogError("LeaderboardPanel is null!");
        }
    }

    public void HideLeaderboard()
    {
        if (leaderboardPanel != null)
            leaderboardPanel.style.display = DisplayStyle.None;
    }

    // Call this from other scripts to use REAL player scores
    public void StartManualSubmission(string playerName, int score)
    {
        Debug.Log($"StartManualSubmission called with {playerName}: {score}");
        pendingName = playerName;
        pendingScore = score;
        ShowLeaderboard();
        StartCoroutine(SubmitScoreRoutine(playerName, score));
    }

    void OnSubmitClicked()
    {
        Debug.Log("Submit clicked");
        // For testing/manual submit
        StartCoroutine(SubmitScoreRoutine("YOU", Random.Range(500, 2000)));
    }

    void OnRetryClicked()
    {
        Debug.Log("Retry clicked");
        if (retryButton != null) retryButton.style.display = DisplayStyle.None;
        StartCoroutine(SubmitScoreRoutine(pendingName, pendingScore));
    }

    IEnumerator SubmitScoreRoutine(string name, int score)
    {
        Debug.Log("SubmitScoreRoutine started");

        // AC: Short "submitting" state
        if (submitButton != null) submitButton.SetEnabled(false);
        if (loadingSpinner != null) loadingSpinner.style.display = DisplayStyle.Flex;

        if (statusMessage != null)
        {
            statusMessage.text = "Submitting score...";
            statusMessage.style.color = Color.white;
            statusMessage.style.display = DisplayStyle.Flex;
            statusMessage.style.opacity = 1f;
        }

        yield return new WaitForSeconds(1.5f); // Short delay for "satisfying" feel

        // Simulate success/fail
        bool success = Random.Range(0, 10) > 2; // 80% success rate
        Debug.Log($"Submission result: {(success ? "SUCCESS" : "FAILURE")}");

        if (loadingSpinner != null) loadingSpinner.style.display = DisplayStyle.None;

        if (success)
        {
            // AC: Confirmation animation
            if (statusMessage != null)
            {
                statusMessage.text = "✓ Score submitted!";
                statusMessage.style.color = Color.green;
            }

            AddPlayerScore(name, score);
            yield return new WaitForSeconds(1.5f);
            if (statusMessage != null) statusMessage.style.display = DisplayStyle.None;
        }
        else
        {
            // AC: Failure animation & Retry option
            if (statusMessage != null)
            {
                statusMessage.text = "✗ Submission failed";
                statusMessage.style.color = Color.red;
            }

            if (retryButton != null) retryButton.style.display = DisplayStyle.Flex;
        }

        if (submitButton != null) submitButton.SetEnabled(true);
    }

    void LoadDummyScores()
    {
        if (scoreList == null)
        {
            Debug.LogError("ScoreList is null, cannot load scores");
            return;
        }

        scoreList.Clear();
        AddScoreEntry("EcoWarrior", 1500, 1);
        AddScoreEntry("GreenMachine", 1200, 2);
        AddScoreEntry("SolarSam", 900, 3);
        Debug.Log($"Loaded {scoreList.childCount} dummy scores");
    }

    void AddScoreEntry(string name, int score, int rank, bool highlight = false)
    {
        var entry = new VisualElement();
        entry.AddToClassList("score-entry");

        var rankLabel = new Label(rank.ToString());
        rankLabel.AddToClassList("rank-label");

        var nameLabel = new Label(name);
        nameLabel.AddToClassList("name-label");

        var scoreLabel = new Label(score.ToString());
        scoreLabel.AddToClassList("score-label");

        entry.Add(rankLabel);
        entry.Add(nameLabel);
        entry.Add(scoreLabel);

        scoreList.Add(entry);

        if (highlight)
        {
            // AC: Rank highlight animation
            entry.schedule.Execute(() => entry.AddToClassList("score-entry--highlight")).StartingIn(100);
        }
    }

    public void AddPlayerScore(string name, int score)
    {
        int newRank = scoreList.childCount + 1;
        AddScoreEntry(name, score, newRank, true);
    }
}