using UnityEngine;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private UIDocument selectionDoc;
    [SerializeField] private UIDocument tutorialDoc;
    [SerializeField] private UIDocument profileDoc;
    [SerializeField] private ProfileManager profileManager;

    private VisualElement selection;
    private VisualElement tutorial;
    private VisualElement profile;
    private Button tutorialBackButton; // Add this

    void Awake()
    {
        // Check each UIDocument before accessing rootVisualElement
        if (selectionDoc == null)
            Debug.LogError("❌ GameUIManager: selectionDoc is not assigned!");
        else
            selection = selectionDoc.rootVisualElement;

        if (tutorialDoc == null)
            Debug.LogError("❌ GameUIManager: tutorialDoc is not assigned!");
        else
        {
            tutorial = tutorialDoc.rootVisualElement;

            // Find the back button in the tutorial panel
            tutorialBackButton = tutorial?.Q<Button>("BackButton");
            if (tutorialBackButton != null)
            {
                tutorialBackButton.clicked += OnTutorialBackClicked;
                Debug.Log("✅ Tutorial back button connected in GameUIManager");
            }
            else
                Debug.LogError("❌ Could not find 'BackButton' in tutorial panel!");
        }

        if (profileDoc == null)
            Debug.LogError("❌ GameUIManager: profileDoc is not assigned!");
        else
            profile = profileDoc.rootVisualElement;
    }

    void Start()
    {
        // Check for ProfileManager
        if (profileManager == null)
        {
            profileManager = FindFirstObjectByType<ProfileManager>();
            if (profileManager == null)
                Debug.LogError("❌ GameUIManager: ProfileManager not found in scene!");
        }

        // Hide all panels initially
        HideAllPanels();
    }

    void HideAllPanels()
    {
        if (selection != null) selection.style.display = DisplayStyle.None;
        if (tutorial != null) tutorial.style.display = DisplayStyle.None;
        if (profile != null) profile.style.display = DisplayStyle.None;
    }

    public void ShowTutorial()
    {
        Debug.Log("📖 Showing tutorial panel");
        HideAllPanels();
        if (tutorial != null)
        {
            tutorial.style.display = DisplayStyle.Flex;
            tutorial.BringToFront();
        }
    }

    public void HideTutorial()
    {
        Debug.Log("📖 Hiding tutorial panel - returning to main menu");
        if (tutorial != null)
            tutorial.style.display = DisplayStyle.None;

        // Don't hide selection - CitySelectionMenu handles its own visibility
    }

    private void OnTutorialBackClicked()
    {
        Debug.Log("🔙 Tutorial back button clicked");
        HideTutorial();
    }

    public void ShowProfile()
    {
        Debug.Log("👤 Showing profile panel");
        HideAllPanels();
        if (profile != null)
        {
            profile.style.display = DisplayStyle.Flex;
            profile.BringToFront();

            if (profileManager != null)
                profileManager.LoadProfile();
        }
    }

    public void ShowSelection()
    {
        Debug.Log("📢 Showing selection panel");
        HideAllPanels();
        if (selection != null)
            selection.style.display = DisplayStyle.Flex;
    }

    private void OnDestroy()
    {
        // Clean up event subscription
        if (tutorialBackButton != null)
            tutorialBackButton.clicked -= OnTutorialBackClicked;
    }
}