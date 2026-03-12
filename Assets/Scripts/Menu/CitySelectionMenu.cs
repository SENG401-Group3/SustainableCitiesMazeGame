using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class CitySelectionMenu : MonoBehaviour
{
    [SerializeField] private GameUIManager gameUIManager;
    private VisualElement root;
    private VisualElement backgroundContainer;
    private Button playButton;
    private Button tutorialButton;
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
    }

    private void OnEnable()
    {
        if (root == null) return;

        backgroundContainer = root.Q<VisualElement>("Background");

        playButton = root.Q<Button>("PlayButton");
        if (playButton != null)
            playButton.clicked += StartGame;

        tutorialButton = root.Q<Button>("TutorialButton");
        if (tutorialButton != null)
            tutorialButton.clicked += ShowTutorial;

        // Scores button removed

        settingsButton = root.Q<Button>("SettingsButton");
        if (settingsButton != null)
            settingsButton.clicked += ShowSettings;

        profileButton = root.Q<Button>("ProfileButton");
        if (profileButton != null)
            profileButton.clicked += ShowProfile;

        quitButton = root.Q<Button>("QuitButton");
        if (quitButton != null)
            quitButton.clicked += QuitGame;
    }

    private void OnDisable()
    {
        if (playButton != null) playButton.clicked -= StartGame;
        if (tutorialButton != null) tutorialButton.clicked -= ShowTutorial;
        if (settingsButton != null) settingsButton.clicked -= ShowSettings;
        if (profileButton != null) profileButton.clicked -= ShowProfile;
        if (quitButton != null) quitButton.clicked -= QuitGame;
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
        SceneManager.LoadScene("MazeScene");
    }

    private void ShowTutorial()
    {
        Debug.Log("🔵 STEP 1: CitySelectionMenu.ShowTutorial() CALLED");

        if (gameUIManager == null)
        {
            gameUIManager = FindFirstObjectByType<GameUIManager>();
            Debug.Log($"🔵 STEP 2: GameUIManager found: {gameUIManager != null}");
        }

        if (gameUIManager != null)
        {
            Debug.Log("🔵 STEP 3: Calling gameUIManager.ShowTutorial()");
            gameUIManager.ShowTutorial();
        }
        else
        {
            Debug.LogError("❌ GameUIManager is NULL!");
        }
    }

    private void ShowSettings()
    {
        Debug.Log("Settings button clicked");
    }

    private void ShowProfile()
    {
        if (gameUIManager != null)
            gameUIManager.ShowProfile();
        else
            Debug.LogError("❌ Cannot show profile: gameUIManager is null!");
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
        // Find GameUIManager if not assigned
        if (gameUIManager == null)
        {
            gameUIManager = FindFirstObjectByType<GameUIManager>();
            if (gameUIManager == null)
                Debug.LogError("❌ GameUIManager not found in scene! Make sure it exists.");
        }

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