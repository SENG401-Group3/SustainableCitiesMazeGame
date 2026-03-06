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

    void Start()
    {
        if (document == null)
        {
            Debug.LogError("❌ UIDocument not assigned!");
            return;
        }

        root = document.rootVisualElement;

        leaderboardPanel = root.Q<VisualElement>("LeaderboardPanel");
        scoreList = root.Q<VisualElement>("ScoreList");
        submitButton = root.Q<Button>("SubmitButton");
        retryButton = root.Q<Button>("RetryButton");
        closeButton = root.Q<Button>("CloseLeaderboardButton");
        loadingSpinner = root.Q<VisualElement>("LoadingSpinner");
        statusMessage = root.Q<Label>("StatusMessage");

        if (retryButton != null) retryButton.style.display = DisplayStyle.None;
        if (loadingSpinner != null) loadingSpinner.style.display = DisplayStyle.None;
        if (statusMessage != null) statusMessage.style.display = DisplayStyle.None;

        if (submitButton != null) submitButton.clicked += OnSubmitClicked;
        if (retryButton != null) retryButton.clicked += OnRetryClicked;
        if (closeButton != null) closeButton.clicked += HideLeaderboard;

        LoadDummyScores();
    }

    public void ShowLeaderboard()
    {
        if (leaderboardPanel != null)
            leaderboardPanel.style.display = DisplayStyle.Flex;
    }

    void HideLeaderboard()
    {
        if (leaderboardPanel != null)
            leaderboardPanel.style.display = DisplayStyle.None;
    }

    void OnSubmitClicked()
    {
        StartCoroutine(SubmitScore());
    }

    void OnRetryClicked()
    {
        if (retryButton != null)
            retryButton.style.display = DisplayStyle.None;

        StartCoroutine(SubmitScore());
    }

    IEnumerator SubmitScore()
    {
        if (submitButton != null)
            submitButton.SetEnabled(false);

        if (loadingSpinner != null)
            loadingSpinner.style.display = DisplayStyle.Flex;

        if (statusMessage != null)
        {
            statusMessage.text = "Submitting score...";
            statusMessage.style.color = Color.white;
            statusMessage.style.opacity = 1f;
            statusMessage.style.display = DisplayStyle.Flex;
        }

        yield return new WaitForSeconds(2f);

        bool success = Random.Range(0, 2) == 0;

        if (loadingSpinner != null)
            loadingSpinner.style.display = DisplayStyle.None;

        if (success)
        {
            if (statusMessage != null)
            {
                statusMessage.text = "✓ Score submitted!";
                statusMessage.style.color = Color.green;

                // FIXED: Removed parentheses from .animation
                statusMessage.experimental.animation.Scale(1.1f, 200);
            }

            AddPlayerScore("YOU", Random.Range(500, 2000));

            yield return new WaitForSeconds(2f);

            if (statusMessage != null)
                statusMessage.style.opacity = 0f;
        }
        else
        {
            if (statusMessage != null)
            {
                statusMessage.text = "✗ Submission failed";
                statusMessage.style.color = Color.red;
            }

            if (retryButton != null)
                retryButton.style.display = DisplayStyle.Flex;
        }

        if (submitButton != null)
            submitButton.SetEnabled(true);
    }

    void LoadDummyScores()
    {
        if (scoreList == null) return;
        scoreList.Clear();
        AddScoreEntry("Player1", 1500, 1);
        AddScoreEntry("Player2", 1200, 2);
        AddScoreEntry("Player3", 900, 3);
        AddScoreEntry("Player4", 600, 4);
        AddScoreEntry("Player5", 300, 5);
    }

    void AddScoreEntry(string name, int score, int rank)
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
    }

    void AddPlayerScore(string name, int score)
    {
        int newRank = scoreList.childCount + 1;
        AddScoreEntry(name, score, newRank);

        if (scoreList.childCount > 0)
        {
            var lastEntry = scoreList[scoreList.childCount - 1];
            lastEntry.schedule.Execute(() => {
                lastEntry.AddToClassList("score-entry--highlight");
            }).StartingIn(50);
        }
    }
}