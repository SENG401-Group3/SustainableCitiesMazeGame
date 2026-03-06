using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;

public class CitySelectionMenu : MonoBehaviour
{
    [Header("UI Toolkit")]
    public UIDocument document;

    [Header("Background")]
    public Sprite[] cityBackgrounds;
    private int currentCityIndex = 1;

    [Header("Message")]
    public string[] sustainabilityMessages = new string[]
    {
        "The city has become a little more sustainable!",
        "Recycling programs have started! The city is cleaner.",
        "Solar panels are appearing! The city is greener.",
        "The city is much more sustainable now!",
        "You've reached the end of the game! Congratulations!"
    };

    [Header("Audio")]
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    private VisualElement root;
    private VisualElement messagePanel;
    private Label messageText;
    private VisualElement tutorialPanel;
    private Button tutorialBackButton;
    private VisualElement backgroundContainer;
    private bool isMessageVisible = false;

    private VisualElement leaderboardPanel;
    private Button closeLeaderboardButton;

    public static int pendingCityIndex = -1;
    public static string pendingMessage = null;

    private string tutorialText =
        "WELCOME TO SUSTAINABILITY CHALLENGE!\n\n" +
        "• Find the treasure box in the maze\n" +
        "• Answer sustainability questions correctly\n" +
        "• Collect artifacts to unlock more sustainable cities\n" +
        "• Progress through all 5 cities to win!\n\n" +
        "GOAL: Learn about UN Sustainability Goals while having fun!";

    void Start()
    {
        InitializeAudio();

        if (pendingCityIndex != -1)
        {
            currentCityIndex = pendingCityIndex;
            pendingCityIndex = -1;
        }
        else
        {
            LoadProgress();
        }

        StartCoroutine(DelayedSetup());

        if (!string.IsNullOrEmpty(pendingMessage))
        {
            StartCoroutine(ShowDelayedMessage(pendingMessage));
            pendingMessage = null;
        }

        CheckForMessage();
    }

    IEnumerator DelayedSetup()
    {
        yield return new WaitForSeconds(0.1f);
        SetupUI();
    }

    IEnumerator ShowDelayedMessage(string message)
    {
        yield return new WaitForSeconds(0.3f);
        if (messagePanel != null && messageText != null)
        {
            messageText.text = message;
            messagePanel.style.display = DisplayStyle.Flex;
            isMessageVisible = true;
            yield return new WaitForSeconds(3f);
            if (messagePanel != null)
                messagePanel.style.display = DisplayStyle.None;
            isMessageVisible = false;
        }
    }

    void InitializeAudio()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void LoadProgress()
    {
        currentCityIndex = PlayerPrefs.GetInt("CurrentCity", 1);
    }

    void SetupUI()
    {
        if (document == null) return;

        root = document.rootVisualElement;
        if (root == null) return;

        root.style.display = DisplayStyle.None;

        backgroundContainer = root.Q<VisualElement>("Background");
        var playButton = root.Q<Button>("PlayButton");
        var tutorialButton = root.Q<Button>("TutorialButton");
        var scoresButton = root.Q<Button>("ScoresButton");
        var settingsButton = root.Q<Button>("SettingsButton");
        var creditsButton = root.Q<Button>("CreditsButton");
        var quitButton = root.Q<Button>("QuitButton");

        messagePanel = root.Q<VisualElement>("MessagePanel");
        messageText = root.Q<Label>("MessageText");

        leaderboardPanel = root.Q<VisualElement>("LeaderboardPanel");
        closeLeaderboardButton = root.Q<Button>("CloseLeaderboardButton");

        CreateTutorialPanel();
        UpdateBackground();

        if (playButton != null) playButton.clicked += StartGame;
        if (tutorialButton != null) tutorialButton.clicked += ShowTutorial;
        if (scoresButton != null) scoresButton.clicked += ShowScores;
        if (settingsButton != null) settingsButton.clicked += ShowSettings;
        if (creditsButton != null) creditsButton.clicked += ShowCredits;
        if (quitButton != null) quitButton.clicked += QuitGame;
        if (closeLeaderboardButton != null) closeLeaderboardButton.clicked += HideLeaderboard;

        if (messagePanel != null) messagePanel.style.display = DisplayStyle.None;
        if (leaderboardPanel != null) leaderboardPanel.style.display = DisplayStyle.None;

        root.style.display = DisplayStyle.Flex;
    }

    void CreateTutorialPanel()
    {
        tutorialPanel = new VisualElement();
        tutorialPanel.name = "TutorialPanel";
        tutorialPanel.style.position = Position.Absolute;
        tutorialPanel.style.width = Length.Percent(100);
        tutorialPanel.style.height = Length.Percent(100);

        // FIXED: Changed alpha to 1.0f to make it fully opaque
        tutorialPanel.style.backgroundColor = new Color(0.05f, 0.05f, 0.05f, 1.0f);

        tutorialPanel.style.display = DisplayStyle.None;
        tutorialPanel.style.alignItems = Align.Center;
        tutorialPanel.style.justifyContent = Justify.Center;

        var tutorialLabel = new Label();
        tutorialLabel.text = tutorialText;
        tutorialLabel.style.color = Color.white;
        tutorialLabel.style.fontSize = 24;
        tutorialLabel.style.whiteSpace = WhiteSpace.Normal;
        tutorialLabel.style.width = Length.Percent(80);
        tutorialLabel.style.marginBottom = 30;
        tutorialLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

        tutorialBackButton = new Button();
        tutorialBackButton.text = "BACK";
        tutorialBackButton.style.width = 200;
        tutorialBackButton.style.height = 50;
        tutorialBackButton.style.backgroundColor = new Color(0.95f, 0.6f, 0f); // Match the orange "BACK" button in your screenshot
        tutorialBackButton.style.color = Color.white;
        tutorialBackButton.clicked += HideTutorial;

        tutorialPanel.Add(tutorialLabel);
        tutorialPanel.Add(tutorialBackButton);
        root.Add(tutorialPanel);
    }

    void UpdateBackground()
    {
        if (backgroundContainer == null || cityBackgrounds == null || cityBackgrounds.Length == 0) return;
        Sprite currentCitySprite = cityBackgrounds[currentCityIndex - 1];
        if (currentCitySprite != null)
            backgroundContainer.style.backgroundImage = new StyleBackground(currentCitySprite);
    }

    void CheckForMessage()
    {
        int cityJustUnlocked = PlayerPrefs.GetInt("ShowSustainabilityMessage", 0);
        if (cityJustUnlocked > 0 && cityJustUnlocked <= sustainabilityMessages.Length)
        {
            ShowMessage(sustainabilityMessages[cityJustUnlocked - 1]);
            PlayerPrefs.SetInt("ShowSustainabilityMessage", 0);
            PlayerPrefs.Save();
        }
    }

    void ShowMessage(string msg)
    {
        if (messagePanel != null && messageText != null)
        {
            messageText.text = msg;
            messagePanel.style.display = DisplayStyle.Flex;
            isMessageVisible = true;
            Invoke(nameof(HideMessage), 3f);
        }
    }

    void HideMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.style.display = DisplayStyle.None;
            isMessageVisible = false;
        }
    }

    void ShowTutorial()
    {
        PlayButtonSound();
        if (tutorialPanel != null)
        {
            tutorialPanel.style.display = DisplayStyle.Flex;
            tutorialPanel.BringToFront(); // Ensure it sits above the background
        }
    }

    void HideTutorial()
    {
        PlayButtonSound();
        if (tutorialPanel != null) tutorialPanel.style.display = DisplayStyle.None;
    }

    void StartGame()
    {
        PlayButtonSound();
        SceneManager.LoadScene("MazeScene");
    }

    void ShowScores()
    {
        PlayButtonSound();
        if (leaderboardPanel != null)
        {
            leaderboardPanel.style.display = DisplayStyle.Flex;
            leaderboardPanel.BringToFront();
        }
    }

    void HideLeaderboard()
    {
        PlayButtonSound();
        if (leaderboardPanel != null)
        {
            leaderboardPanel.style.display = DisplayStyle.None;
        }
    }

    void ShowSettings() { PlayButtonSound(); }
    void ShowCredits() { PlayButtonSound(); }

    void QuitGame()
    {
        PlayButtonSound();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void PlayButtonSound()
    {
        if (buttonClickSound != null && audioSource != null)
            audioSource.PlayOneShot(buttonClickSound);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        LoadProgress();
        UpdateBackground();
    }
}