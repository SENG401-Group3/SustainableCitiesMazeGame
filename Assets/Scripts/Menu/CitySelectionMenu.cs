using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
//using System.Collections.Generic;
using System.Collections;

public class CitySelectionMenu : MonoBehaviour
{
    [SerializeField] private GameUIManager gameUIManager;
    private VisualElement root;
    private VisualElement backgroundContainer;
    private Button playButton;
    private Button tutorialButton;
    private Button scoresButton;
    private Button settingsButton;
    private Button profileButton;
    private Button quitButton;
    public static int pendingCityIndex = -1;
    public static string pendingMessage = null;
    private int currentCityIndex = 1;
    public Sprite[] cityBackgrounds;
    private void Awake()
    {
        // fetch the panel as soon as it is initialized
        root = GetComponent<UIDocument>().rootVisualElement;
        //welcomePanel = welcomeDoc.rootVisualElement;
    }

    private void OnEnable()
    {
        backgroundContainer = root.Q<VisualElement>("Background");

        playButton = root.Q<Button>("PlayButton");
        playButton.clicked += StartGame;

        tutorialButton = root.Q<Button>("TutorialButton");
        tutorialButton.clicked += ShowTutorial;
        
        scoresButton = root.Q<Button>("ScoresButton");
        scoresButton.clicked += ShowScores;

        settingsButton = root.Q<Button>("SettingsButton");
        settingsButton.clicked += ShowSettings;

        profileButton = root.Q<Button>("ProfileButton");
        profileButton.clicked += ShowProfile;

        quitButton = root.Q<Button>("QuitButton");
        quitButton.clicked += QuitGame;

        //UpdateBackground();

        /*messagePanel = root.Q<VisualElement>("MessagePanel");
        messageText = root.Q<Label>("MessageText");

        leaderboardPanel = root.Q<VisualElement>("LeaderboardPanel");
        closeLeaderboardButton = root.Q<Button>("CloseLeaderboardButton");

        
        if (closeLeaderboardButton != null) closeLeaderboardButton.clicked += HideLeaderboard;

        if (messagePanel != null) messagePanel.style.display = DisplayStyle.None;
        if (leaderboardPanel != null) leaderboardPanel.style.display = DisplayStyle.None;*/
    }

    private void OnDisable()
    {
        playButton.clicked -= StartGame;
        tutorialButton.clicked -= ShowTutorial;
        scoresButton.clicked -= ShowScores;
        settingsButton.clicked -= ShowSettings;
        profileButton.clicked -= ShowProfile;
        quitButton.clicked -= QuitGame;
    }

    void UpdateBackground()
    {
        if (backgroundContainer == null || cityBackgrounds == null || cityBackgrounds.Length == 0)
            return;

        int index = currentCityIndex - 1;

        if (index < 0 || index >= cityBackgrounds.Length)
            return;

        Sprite sprite = cityBackgrounds[index];

        if (sprite != null)
            backgroundContainer.style.backgroundImage = new StyleBackground(sprite);
    }

    private void StartGame()
    {
        //PlayButtonSound();
        SceneManager.LoadScene("MazeScene");
    }

    private void ShowTutorial()
    {
        gameUIManager.ShowTutorial();
    }

    private void ShowScores()
    {
        gameUIManager.ShowScores();
    }

    private void ShowSettings()
    {
        
    }

    private void ShowProfile()
    {
        gameUIManager.ShowProfile();
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void Start()
    {
        if (pendingCityIndex != -1)
        {
            currentCityIndex = pendingCityIndex;
            pendingCityIndex = -1;
        }
        else
        {
            currentCityIndex = PlayerPrefs.GetInt("CurrentCity", 1);
        }

        UpdateBackground();
    }
}