using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class CitySelectionMenu : MonoBehaviour
{
    public UIDocument document;
    public Sprite[] cityBackgrounds;

    private VisualElement root;
    private VisualElement backgroundContainer;
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

        StartCoroutine(SetupSequence());
    }

    IEnumerator SetupSequence()
    {
        yield return null;

        backgroundContainer = root.Q<VisualElement>("Background");

        // Set Background based on CurrentCity
        int cityIdx = PlayerPrefs.GetInt("CurrentCity", 1) - 1;
        cityIdx = Mathf.Clamp(cityIdx, 0, cityBackgrounds.Length - 1);
        if (backgroundContainer != null && cityBackgrounds.Length > 0)
            backgroundContainer.style.backgroundImage = new StyleBackground(cityBackgrounds[cityIdx]);

        Debug.Log($"CitySelectionMenu loaded - CurrentCity: {cityIdx + 1}");

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

        CreateTutorialPanel();

        yield return new WaitForSeconds(0.2f);
        root.style.opacity = 1;
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
        Debug.Log("ShowScores called - Scores button clicked");

        // You can add future functionality here
        // For example, load a persistent leaderboard scene or show high scores
        Debug.Log("🏆 Scores button clicked - feature coming soon!");

        // Optional: Show a message that leaderboard is coming soon
        // You could add a simple popup here
    }
}