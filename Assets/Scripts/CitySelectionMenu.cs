using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class CitySelectionMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject playMenuPanel;
    public GameObject tutorialPanel;
    public GameObject scoresPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Scores Panel")]
    public Text[] cityScoreTexts;
    public Text[] cityStatusTexts;
    public Text totalScoreText;
    public Text totalArtifactsText;
    public Text highestCityText;
    public Button resetProgressButton;

    [Header("Tutorial Panel")]
    public Text tutorialTitleText;
    public Text tutorialContentText;
    public Image tutorialImage;
    public Button[] tutorialPageButtons;
    public Text pageNumberText;

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

    private string[] tutorialPages = new string[]
    {
        "WELCOME TO SUSTAINABILITY CHALLENGE!\n\nLearn about UN Sustainability Goals while exploring cities and solving challenges.",
        "GAMEPLAY:\n• Navigate through mazes\n• Find treasure boxes\n• Answer sustainability questions\n• Collect artifacts\n• Unlock more sustainable cities",
        "CITIES:\nStart from the least sustainable city and work your way up to the most sustainable one. Each city teaches different sustainability concepts.",
        "SCORING:\n• +100 points per correct answer\n• +1 artifact per correct answer\n• Complete cities to unlock the next level\n• Earn achievements for milestones",
        "SUSTAINABILITY GOALS:\nLearn about clean energy, recycling, public transport, green buildings, and carbon neutrality!"
    };

    private int currentTutorialPage = 0;

    void Start()
    {
        InitializeAudio();
        ShowMainMenu();
    }

    void InitializeAudio()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ShowMainMenu()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(true);
        if (playMenuPanel != null) playMenuPanel.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (scoresPanel != null) scoresPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void ShowTutorial()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        currentTutorialPage = 0;
        UpdateTutorialPage();
    }

    public void ShowScores()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(false);
        scoresPanel.SetActive(true);
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

    public void NextTutorialPage()
    {
        if (currentTutorialPage < tutorialPages.Length - 1)
        {
            currentTutorialPage++;
            UpdateTutorialPage();
        }
    }

    public void PreviousTutorialPage()
    {
        if (currentTutorialPage > 0)
        {
            currentTutorialPage--;
            UpdateTutorialPage();
        }
    }

    void UpdateTutorialPage()
    {
        if (tutorialContentText != null)
            tutorialContentText.text = tutorialPages[currentTutorialPage];

        if (pageNumberText != null)
            pageNumberText.text = $"{currentTutorialPage + 1}/{tutorialPages.Length}";

        if (tutorialPageButtons != null && tutorialPageButtons.Length >= 2)
        {
            tutorialPageButtons[0].interactable = currentTutorialPage > 0;
            tutorialPageButtons[1].interactable = currentTutorialPage < tutorialPages.Length - 1;
        }
    }

    // ============== PLAY BUTTON ==============
    public void StartGame()
    {
        PlayButtonSound();
        SceneManager.LoadScene("City1_LeastSustainable");
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
}