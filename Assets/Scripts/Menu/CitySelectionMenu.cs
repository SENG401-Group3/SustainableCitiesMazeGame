using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;

public class CitySelectionMenu : MonoBehaviour
{
    [Header("UI Toolkit")]
    public UIDocument document; // Drag your UIDocument here

    [Header("Background")]
    public Sprite[] cityBackgrounds; // 5 city images
    private int currentCityIndex = 1;

    [Header("Message")]
    public string[] sustainabilityMessages = new string[]
    {
        "The city has become a little more sustainable!",
        "Recycling programs have started! The city is cleaner.",
        "Solar panels are appearing! The city is greener.",
        "The city is much more sustainable now!",
        "Congratulations! You've reached the most sustainable city!"
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

    // Tutorial text stored directly in script
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
        LoadProgress();
        StartCoroutine(DelayedSetup());
        CheckForMessage();
    }

    IEnumerator DelayedSetup()
    {
        yield return new WaitForSeconds(0.1f);
        SetupUI();
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
        if (document == null)
        {
            Debug.LogError("UIDocument not assigned!");
            return;
        }

        root = document.rootVisualElement;

        if (root == null)
        {
            Debug.LogError("Root visual element is null!");
            return;
        }

        // Find existing UI elements
        backgroundContainer = root.Q<VisualElement>("Background");
        var playButton = root.Q<Button>("PlayButton");
        var tutorialButton = root.Q<Button>("TutorialButton");
        var scoresButton = root.Q<Button>("ScoresButton");
        var settingsButton = root.Q<Button>("SettingsButton");
        var creditsButton = root.Q<Button>("CreditsButton");
        var quitButton = root.Q<Button>("QuitButton");

        messagePanel = root.Q<VisualElement>("MessagePanel");
        messageText = root.Q<Label>("MessageText");

        // Create tutorial panel programmatically
        CreateTutorialPanel();

        // Debug which buttons were found
        Debug.Log($"PlayButton found: {playButton != null}");
        Debug.Log($"TutorialButton found: {tutorialButton != null}");
        Debug.Log($"ScoresButton found: {scoresButton != null}");
        Debug.Log($"SettingsButton found: {settingsButton != null}");
        Debug.Log($"CreditsButton found: {creditsButton != null}");
        Debug.Log($"QuitButton found: {quitButton != null}");

        // Update background
        UpdateBackground();

        // Connect button events
        if (playButton != null) playButton.clicked += StartGame;
        if (tutorialButton != null) tutorialButton.clicked += ShowTutorial;
        if (scoresButton != null) scoresButton.clicked += ShowScores;
        if (settingsButton != null) settingsButton.clicked += ShowSettings;
        if (creditsButton != null) creditsButton.clicked += ShowCredits;
        if (quitButton != null) quitButton.clicked += QuitGame;

        // Hide message panel at start
        if (messagePanel != null)
            messagePanel.style.display = DisplayStyle.None;
    }

    void CreateTutorialPanel()
    {
        // Create main tutorial container
        tutorialPanel = new VisualElement();
        tutorialPanel.name = "TutorialPanel";
        tutorialPanel.style.position = Position.Absolute;
        tutorialPanel.style.width = Length.Percent(100);
        tutorialPanel.style.height = Length.Percent(100);
        tutorialPanel.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.95f);
        tutorialPanel.style.display = DisplayStyle.None; // Hidden by default
        tutorialPanel.style.alignItems = Align.Center;
        tutorialPanel.style.justifyContent = Justify.Center;

        // Create tutorial text label
        var tutorialLabel = new Label();
        tutorialLabel.name = "TutorialLabel";
        tutorialLabel.text = tutorialText;
        tutorialLabel.style.color = Color.white;
        tutorialLabel.style.fontSize = 24;
        tutorialLabel.style.whiteSpace = WhiteSpace.Normal;
        tutorialLabel.style.width = Length.Percent(80);
        tutorialLabel.style.height = StyleKeyword.Auto; // Fixed: was Length.Auto
        tutorialLabel.style.marginBottom = 30;
        tutorialLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

        // Create back button
        tutorialBackButton = new Button();
        tutorialBackButton.name = "TutorialBackButton";
        tutorialBackButton.text = "BACK";
        tutorialBackButton.style.width = 200;
        tutorialBackButton.style.height = 50;
        tutorialBackButton.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
        tutorialBackButton.style.color = Color.white;
        tutorialBackButton.style.fontSize = 20;
        tutorialBackButton.style.marginTop = 20;

        // Add click handler
        tutorialBackButton.clicked += HideTutorial;

        // Add elements to panel
        tutorialPanel.Add(tutorialLabel);
        tutorialPanel.Add(tutorialBackButton);

        // Add panel to root
        root.Add(tutorialPanel);
    }

    void UpdateBackground()
    {
        if (backgroundContainer == null)
        {
            Debug.LogError("Background element not found! Check UXML name.");
            return;
        }

        if (cityBackgrounds == null || cityBackgrounds.Length == 0)
        {
            Debug.LogError("City backgrounds not assigned!");
            return;
        }

        Sprite currentCitySprite = cityBackgrounds[currentCityIndex - 1];
        if (currentCitySprite != null)
        {
            backgroundContainer.style.backgroundImage = new StyleBackground(currentCitySprite);
        }
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

        // Hide main menu background
        if (backgroundContainer != null)
            backgroundContainer.style.display = DisplayStyle.None;

        // Show tutorial panel
        if (tutorialPanel != null)
            tutorialPanel.style.display = DisplayStyle.Flex;
    }

    void HideTutorial()
    {
        PlayButtonSound();

        // Show main menu background
        if (backgroundContainer != null)
            backgroundContainer.style.display = DisplayStyle.Flex;

        // Hide tutorial panel
        if (tutorialPanel != null)
            tutorialPanel.style.display = DisplayStyle.None;
    }

    void StartGame()
    {
        PlayButtonSound();
        Debug.Log("Loading scene: 0");
        SceneManager.LoadScene("0");
    }

    void ShowScores()
    {
        PlayButtonSound();
        Debug.Log("Scores clicked - to be implemented");
    }

    void ShowSettings()
    {
        PlayButtonSound();
        Debug.Log("Settings clicked - to be implemented");
    }

    void ShowCredits()
    {
        PlayButtonSound();
        Debug.Log("Credits clicked - to be implemented");
    }

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
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        LoadProgress();
        UpdateBackground();
    }
}