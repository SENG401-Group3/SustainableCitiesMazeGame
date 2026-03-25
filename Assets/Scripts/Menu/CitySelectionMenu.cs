using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

/*
 * Manages the city selection menu UI, including background images,
 * button interactions, and navigation to other menus.
 */

public class CitySelectionMenu : MonoBehaviour
{
    [Header("UI Manager")]
    [SerializeField] private GameUIManager gameUIManager; // Reference to the GameUIManager for showing panels

    [Header("City Backgrounds")]
    [SerializeField] private Sprite[] cityBackgrounds; // Array of background images for cities 1-5 (assigned in Inspector)

    private int currentCityIndex = 1; // Current city being displayed (1-5)
    [Header("Static State")]
    private static int pendingCityIndex = -1; // Used to override the current city when returning from another scene
    private int currentCityIndex; // Current city being displayed (1-5)

    [Header("UI Elements")]
    private VisualElement root; // Root VisualElement of the UIDocument
    private VisualElement backgroundContainer; // Container for the background image
    private Button playButton; // Button to start the game
    private Button tutorialButton; // Button to show tutorial
    private Button scoresButton; // Button to show settings
    private Button profileButton; // Button to show profile
    private Button quitButton; // Button to quit the game

    // Public read-only property to access city backgrounds if needed
    public Sprite[] CityBackgrounds
    {
        get { return cityBackgrounds; }
    }

    /*
     * Unity's Awake method - initializes the root VisualElement
     */
    private void Awake()
    {
        // Get the root VisualElement from the UIDocument component
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    /*
     * Unity's OnEnable method - sets up button click events and finds UI elements
     */
    private void OnEnable()
    {
        if (root == null) return;

        // Find the background container element
        backgroundContainer = root.Q<VisualElement>("Background");

        // Find and configure the Play button
        playButton = root.Q<Button>("PlayButton");
        if (playButton != null)
            playButton.clicked += StartGame;

        // Find and configure the Tutorial button
        tutorialButton = root.Q<Button>("TutorialButton");
        if (tutorialButton != null)
            tutorialButton.clicked += ShowTutorial;

        // Find and configure the Settings button
        settingsButton = root.Q<Button>("SettingsButton");
        if (settingsButton != null)
            settingsButton.clicked += ShowSettings;
        scoresButton = root.Q<Button>("ScoresButton");
        scoresButton.clicked += ShowScores;

        // Find and configure the Profile button
        profileButton = root.Q<Button>("ProfileButton");
        if (profileButton != null)
            profileButton.clicked += ShowProfile;

        // Find and configure the Quit button
        quitButton = root.Q<Button>("QuitButton");
        if (quitButton != null)
            quitButton.clicked += QuitGame;
    }

    /*
     * Unity's OnDisable method - removes button click events to prevent memory leaks
     */
    private void OnDisable()
    {
        if (playButton != null) playButton.clicked -= StartGame;
        if (tutorialButton != null) tutorialButton.clicked -= ShowTutorial;
        if (settingsButton != null) settingsButton.clicked -= ShowSettings;
        if (profileButton != null) profileButton.clicked -= ShowProfile;
        if (quitButton != null) quitButton.clicked -= QuitGame;
        playButton.clicked -= StartGame;
        tutorialButton.clicked -= ShowTutorial;
        scoresButton.clicked -= ShowScores;
        profileButton.clicked -= ShowProfile;
        quitButton.clicked -= QuitGame;
    }

    /*
     * Unity's Start method - initializes the current city index and updates the background
     */
    private void Start()
    {
        // Find GameUIManager if not assigned in Inspector
        if (gameUIManager == null)
        {
            gameUIManager = FindFirstObjectByType<GameUIManager>();
            if (gameUIManager == null)
                Debug.LogError("❌ GameUIManager not found in scene! Make sure it exists.");
        }

        // Use the saved city index from PlayerPrefs
        currentCityIndex = PlayerPrefs.GetInt("CurrentCity", 1);

        // Update the background to match the current city
        // Check if there's a pending city index from another scene
        if(DBManager.username == "Guest")
        {
            profileButton.style.display = DisplayStyle.None;
            if (pendingCityIndex != -1)
            {
                currentCityIndex = pendingCityIndex;
                pendingCityIndex = -1; // Reset after use (one-time override)
            }
            else
            {
                // Use the saved city index from PlayerPrefs
                currentCityIndex = PlayerPrefs.GetInt("CurrentCity", 1);
            }
        } else{

            // Update the background to match the current city
            currentCityIndex = DBManager.cityNumber;
        }
        UpdateBackground();
    }

    /*
     * Updates the background image based on the current city index
     */
    private void UpdateBackground()
    {
        // Validate that all required components exist
        if (backgroundContainer == null || cityBackgrounds == null || cityBackgrounds.Length == 0)
            return;

        // Convert 1-based city index to 0-based array index
        int index = currentCityIndex - 1;

        // Validate array bounds
        if (index < 0 || index >= cityBackgrounds.Length)
            return;

        // Get the appropriate sprite for this city
        Sprite sprite = cityBackgrounds[index];

        // Apply the sprite as background if valid
        if (sprite != null)
            backgroundContainer.style.backgroundImage = new StyleBackground(sprite);
    }

    /*
     * Starts the game by loading the Maze scene
     */
    private void StartGame()
    {
        SceneManager.LoadScene("MazeScene");
    }

    /*
     * Shows the tutorial panel via the GameUIManager
     */
    private void ShowTutorial()
    {
        Debug.Log("🔵 STEP 1: CitySelectionMenu.ShowTutorial() CALLED");

        // Try to find GameUIManager if not already assigned
        if (gameUIManager == null)
        {
            gameUIManager = FindFirstObjectByType<GameUIManager>();
            Debug.Log($"🔵 STEP 2: GameUIManager found: {gameUIManager != null}");
        }

        // Show the tutorial panel if GameUIManager is available
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

    /*
     * Placeholder for settings button functionality
     */
    private void ShowScores()
    {
        Debug.Log("⚙️ Settings button clicked");
        // TODO: Implement settings functionality later
        SceneManager.LoadScene("LeaderboardScene");
    }

    /*
     * Shows the profile panel via the GameUIManager
     */
    private void ShowProfile()
    {
        if (gameUIManager != null)
            gameUIManager.ShowProfile();
        else
            Debug.LogError("❌ Cannot show profile: gameUIManager is null!");
    }

    /*
     * Quits the game (or stops play mode in the editor)
     */
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
         // clear current user data
        StartCoroutine(DBManager.LogOut());
        SceneManager.LoadScene("UI");
    }
}