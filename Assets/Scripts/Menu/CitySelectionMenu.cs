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

    // YOUR city progression variables
    public static int pendingCityIndex = -1;
    public static string pendingMessage = null;
    private int currentCityIndex = 1;
    public Sprite[] cityBackgrounds; // YOUR backgrounds

    private void Awake()
    {
        Debug.Log("🔵 Awake() called");

        // Check for UIDocument component
        var uiDoc = GetComponent<UIDocument>();
        if (uiDoc == null)
        {
            Debug.LogError("❌ CitySelectionMenu: No UIDocument component found on this GameObject!");
            return;
        }

        root = uiDoc.rootVisualElement;

        if (root == null)
            Debug.LogError("❌ CitySelectionMenu: rootVisualElement is null!");
        else
            Debug.Log("✅ rootVisualElement found");
    }

    private void OnEnable()
    {
        Debug.Log("🟢 OnEnable() called");

        if (root == null)
        {
            Debug.LogError("❌ root is null in OnEnable!");
            return;
        }

        // Find background
        backgroundContainer = root.Q<VisualElement>("Background");
        if (backgroundContainer == null)
            Debug.LogError("❌ Could not find 'Background' in UXML!");
        else
            Debug.Log("✅ Found 'Background' element");

        // Find all buttons with null checks
        playButton = root.Q<Button>("PlayButton");
        if (playButton != null)
        {
            playButton.clicked += StartGame;
            Debug.Log("✅ Found 'PlayButton'");
        }
        else
            Debug.LogError("❌ Could not find 'PlayButton' in UXML!");

        tutorialButton = root.Q<Button>("TutorialButton");
        if (tutorialButton != null)
        {
            tutorialButton.clicked += ShowTutorial;
            Debug.Log("✅ Found 'TutorialButton'");
        }
        else
            Debug.LogError("❌ Could not find 'TutorialButton' in UXML!");

        settingsButton = root.Q<Button>("SettingsButton");
        if (settingsButton != null)
        {
            settingsButton.clicked += ShowSettings;
            Debug.Log("✅ Found 'SettingsButton'");
        }
        else
            Debug.LogError("❌ Could not find 'SettingsButton' in UXML!");

        profileButton = root.Q<Button>("ProfileButton");
        if (profileButton != null)
        {
            profileButton.clicked += ShowProfile;
            Debug.Log("✅ Found 'ProfileButton'");
        }
        else
            Debug.LogError("❌ Could not find 'ProfileButton' in UXML!");

        quitButton = root.Q<Button>("QuitButton");
        if (quitButton != null)
        {
            quitButton.clicked += QuitGame;
            Debug.Log("✅ Found 'QuitButton'");
        }
        else
            Debug.LogError("❌ Could not find 'QuitButton' in UXML!");
    }

    private void OnDisable()
    {
        Debug.Log("🔴 OnDisable() called");

        // Safely unsubscribe from events
        if (playButton != null) playButton.clicked -= StartGame;
        if (tutorialButton != null) tutorialButton.clicked -= ShowTutorial;
        if (settingsButton != null) settingsButton.clicked -= ShowSettings;
        if (profileButton != null) profileButton.clicked -= ShowProfile;
        if (quitButton != null) quitButton.clicked -= QuitGame;
    }

    void Start()
    {
        Debug.Log("🟢 Start() called");

        // Find GameUIManager if not assigned
        if (gameUIManager == null)
        {
            gameUIManager = FindFirstObjectByType<GameUIManager>();
            if (gameUIManager == null)
                Debug.LogError("❌ GameUIManager not found in scene! Make sure it exists.");
            else
                Debug.Log("✅ GameUIManager found");
        }

        // YOUR city progression logic
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

        // DEBUG: Check root properties
        if (root != null)
        {
            Debug.Log($"📊 Root opacity: {root.style.opacity.value}");
            Debug.Log($"📊 Root display: {root.style.display.value}");
            Debug.Log($"📊 Root visible: {root.visible}");
            Debug.Log($"📊 Root worldBound: {root.worldBound}");
        }

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Debug.Log("🎬 FadeIn coroutine started");

        if (root == null)
        {
            Debug.LogError("❌ root is null in FadeIn!");
            yield break;
        }

        root.style.opacity = 0;
        Debug.Log("📊 Opacity set to 0");

        yield return new WaitForSeconds(0.2f);

        root.style.opacity = 1;
        Debug.Log("📊 Opacity set to 1");

        // Check again after fade
        Debug.Log($"📊 Final opacity: {root.style.opacity.value}");
        Debug.Log($"📊 Final display: {root.style.display.value}");
    }

    void UpdateBackground()
    {
        if (backgroundContainer == null)
        {
            Debug.LogError("❌ backgroundContainer is null in UpdateBackground!");
            return;
        }

        if (cityBackgrounds == null || cityBackgrounds.Length == 0)
        {
            Debug.LogError("❌ cityBackgrounds is null or empty!");
            return;
        }

        int index = currentCityIndex - 1;
        if (index < 0 || index >= cityBackgrounds.Length)
        {
            Debug.LogError($"❌ Invalid city index: {index}");
            return;
        }

        Sprite sprite = cityBackgrounds[index];
        if (sprite != null)
        {
            backgroundContainer.style.backgroundImage = new StyleBackground(sprite);
            Debug.Log($"✅ Background updated to city {currentCityIndex}");
        }
        else
        {
            Debug.LogError($"❌ Sprite is null for city {currentCityIndex}");
        }
    }

    private void StartGame()
    {
        Debug.Log("🎮 StartGame() called");
        SceneManager.LoadScene("MazeScene");
    }

    private void ShowTutorial()
    {
        Debug.Log("📖 ShowTutorial() called");

        // Make sure GameUIManager is enabled
        if (gameUIManager == null)
        {
            gameUIManager = FindFirstObjectByType<GameUIManager>();
        }

        if (gameUIManager != null)
        {
            // Make sure the GameObject is active
            gameUIManager.gameObject.SetActive(true);
            gameUIManager.ShowTutorial();
        }
        else
            Debug.LogError("❌ Cannot show tutorial: gameUIManager is null!");
    }

    private void ShowSettings()
    {
        Debug.Log("⚙️ ShowSettings() called");
    }

    private void ShowProfile()
    {
        Debug.Log("👤 ShowProfile() called");
        if (gameUIManager != null)
            gameUIManager.ShowProfile();
        else
            Debug.LogError("❌ Cannot show profile: gameUIManager is null!");
    }

    private void QuitGame()
    {
        Debug.Log("👋 QuitGame() called");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}