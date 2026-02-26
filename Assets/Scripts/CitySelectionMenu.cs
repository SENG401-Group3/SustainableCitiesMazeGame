using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class CitySelectionMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject tutorialPanel;
    public GameObject scoresPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject messagePanel;

    [Header("Background")]
    public Image backgroundImage; // The background image of the main menu
    public Sprite[] cityBackgrounds; // 5 sprites, one per city
    private int currentCityIndex = 1; // 1 = least sustainable

    [Header("Message")]
    public TMPro.TextMeshProUGUI messageText;
    public string[] sustainabilityMessages = new string[]
    {
        "The city has become a little more sustainable!",
        "Recycling programs have started! The city is cleaner.",
        "Solar panels are appearing! The city is greener.",
        "The city is much more sustainable now!",
        "Congratulations! You've reached the most sustainable city!"
    };

    [Header("Scores Panel")]
    public Text[] cityScoreTexts;
    public Text[] cityStatusTexts;
    public Text totalScoreText;
    public Text totalArtifactsText;
    public Text highestCityText;
    public Button resetProgressButton;

    [Header("Tutorial Panel")]
    public TMPro.TextMeshProUGUI tutorialContentText; // The main text area for the tutorial
    public Button backButton; // Button to return to main menu

    [Header("Settings Panel")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;

    [Header("Audio")]
    public AudioClip buttonHoverSound;
    public AudioClip buttonClickSound;
    public AudioClip gameStartSound;
    private AudioSource audioSource;

    private int currentTutorialPage = 0; // Kept for compatibility but not used

    void Start()
    {
        InitializeAudio();
        LoadProgress();
        UpdateBackground();
        ShowMainMenu();
        CheckForMessage();
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

    void UpdateBackground()
    {
        if (backgroundImage != null && cityBackgrounds.Length >= currentCityIndex)
        {
            backgroundImage.sprite = cityBackgrounds[currentCityIndex - 1];
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
            messagePanel.SetActive(true);
            Invoke("HideMessage", 3f);
        }
    }

    void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(true);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (scoresPanel != null) scoresPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (messagePanel != null) messagePanel.SetActive(false);
    }

    // ============== SIMPLE TUTORIAL (Option 1) ==============
    public void ShowTutorial()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(true);

        // Set the tutorial text
        if (tutorialContentText != null)
        {
            tutorialContentText.text = "WELCOME TO SUSTAINABILITY CHALLENGE!\n\n" +
                "• Find the treasure box in the maze\n" +
                "• Answer sustainability questions correctly\n" +
                "• Collect artifacts to unlock more sustainable cities\n" +
                "• Progress through all 5 cities to win!\n\n" +
                "GOAL: Learn about UN Sustainability Goals while having fun!";
        }
    }

    public void CloseTutorial()
    {
        PlayButtonSound();
        tutorialPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    // =========================================================

    public void ShowScores()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(false);
        scoresPanel.SetActive(true);
        // Load and display scores here
    }

    public void ShowSettings()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    // ============== PLAY BUTTON ==============
    public void StartGame()
    {
        PlayButtonSound();
        SceneManager.LoadScene("0");
    }

    public void QuitGame()
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

    public void OnButtonHover()
    {
        if (buttonHoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonHoverSound);
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        LoadProgress();
        UpdateBackground();
    }
}