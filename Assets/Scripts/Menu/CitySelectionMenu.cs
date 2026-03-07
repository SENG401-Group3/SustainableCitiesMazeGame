using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class CitySelectionMenu : MonoBehaviour
{
    public UIDocument document;
    public LeaderboardManager leaderboardManager; // The slot you are dragging GameManager into
    public Sprite[] cityBackgrounds;

    private VisualElement root;
    private VisualElement backgroundContainer;
    private VisualElement leaderboardPanel;
    private VisualElement tutorialPanel;

    void Start()
    {
        if (document == null)
        {
            Debug.LogError("❌ CitySelectionMenu: UIDocument not assigned!");
            return;
        }

        root = document.rootVisualElement;
        root.style.opacity = 0;

        // AUTO-FIND: If the slot is blank, find the LeaderboardManager in the scene
        if (leaderboardManager == null)
        {
            leaderboardManager = FindFirstObjectByType<LeaderboardManager>();
            Debug.Log($"LeaderboardManager auto-found: {leaderboardManager != null}");
        }

        StartCoroutine(SetupSequence());
    }

    IEnumerator SetupSequence()
    {
        yield return null;

        backgroundContainer = root.Q<VisualElement>("Background");
        leaderboardPanel = root.Q<VisualElement>("LeaderboardPanel");

        // Set Background
        int cityIdx = PlayerPrefs.GetInt("CurrentCity", 1) - 1;
        cityIdx = Mathf.Clamp(cityIdx, 0, cityBackgrounds.Length - 1);
        if (backgroundContainer != null && cityBackgrounds.Length > 0)
            backgroundContainer.style.backgroundImage = new StyleBackground(cityBackgrounds[cityIdx]);

        // Bind Buttons
        var playButton = root.Q<Button>("PlayButton");
        if (playButton != null)
            playButton.clicked += () => SceneManager.LoadScene("MazeScene");

        var scoresButton = root.Q<Button>("ScoresButton");
        if (scoresButton != null)
            scoresButton.clicked += ShowScores;

        // Setup Tutorial Button
        Button tutBtn = root.Q<Button>("TutorialButton");
        if (tutBtn != null) tutBtn.clicked += ShowTutorial;

        // Setup Close Button for Leaderboard
        Button closeBtn = root.Q<Button>("CloseLeaderboardButton");
        if (closeBtn != null) closeBtn.clicked += () =>
        {
            if (leaderboardPanel != null) leaderboardPanel.style.display = DisplayStyle.None;
        };

        CreateTutorialPanel();

        yield return new WaitForSeconds(0.2f);
        root.style.opacity = 1;

        // Check for City 5 Completion Win
        if (PlayerPrefs.GetInt("GameComplete", 0) == 1)
        {
            yield return new WaitForSeconds(0.5f);
            ShowScores();
            PlayerPrefs.SetInt("GameComplete", 0);
            PlayerPrefs.Save();
        }
    }

    void CreateTutorialPanel()
    {
        tutorialPanel = new VisualElement();
        tutorialPanel.style.position = Position.Absolute;
        tutorialPanel.style.width = Length.Percent(100);
        tutorialPanel.style.height = Length.Percent(100);
        tutorialPanel.style.backgroundColor = new Color(0.05f, 0.05f, 0.05f, 0.95f);
        tutorialPanel.style.display = DisplayStyle.None;
        tutorialPanel.style.alignItems = Align.Center;
        tutorialPanel.style.justifyContent = Justify.Center;

        Label title = new Label("HOW TO PLAY");
        title.style.fontSize = 40;
        title.style.color = Color.white;

        Label text = new Label("1. Find Artifacts\n2. Answer Questions\n3. Save the Cities!");
        text.style.color = Color.white;
        text.style.fontSize = 20;

        Button close = new Button { text = "CLOSE" };
        close.clicked += () => tutorialPanel.style.display = DisplayStyle.None;

        tutorialPanel.Add(title);
        tutorialPanel.Add(text);
        tutorialPanel.Add(close);
        root.Add(tutorialPanel);
    }

    void ShowTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.style.display = DisplayStyle.Flex;
            tutorialPanel.BringToFront();
        }
    }

    void ShowScores()
    {
        Debug.Log("ShowScores called");

        if (leaderboardPanel != null)
        {
            leaderboardPanel.style.display = DisplayStyle.Flex;
            leaderboardPanel.BringToFront();

            int total = PlayerPrefs.GetInt("TotalScore", 0);
            Debug.Log($"Total score from PlayerPrefs: {total}");

            if (leaderboardManager != null)
            {
                leaderboardManager.StartManualSubmission("Makuo", total);
            }
            else
            {
                Debug.LogError("❌ LeaderboardManager is null! Make sure it's attached to a GameObject.");
            }
        }
        else
        {
            Debug.LogError("LeaderboardPanel not found in UXML!");
        }
    }
}