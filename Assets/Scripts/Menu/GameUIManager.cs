using UnityEngine;
using UnityEngine.UIElements;

/*
 * Manages all UI panels in the game including selection menu, tutorial,
 * profile, and profile editor. Controls which panel is visible at any time.
 */

public class GameUIManager : MonoBehaviour
{
    [Header("UI Document References")]
    [SerializeField] private UIDocument selectionDoc;      // Main menu/city selection UI
    [SerializeField] private UIDocument tutorialDoc;       // Tutorial panel UI
    [SerializeField] private UIDocument profileDoc;        // Player profile UI
    [SerializeField] private UIDocument profileEditorDoc;  // Profile editor UI

    [Header("Managers")]
    [SerializeField] private ProfileManager profileManager; // Reference to profile manager for loading data

    // VisualElement references for each panel
    private VisualElement selection;      // Root element of selection panel
    private VisualElement tutorial;        // Root element of tutorial panel
    private VisualElement profile;         // Root element of profile panel
    private VisualElement profileEditor;   // Root element of profile editor panel

    /*
     * Unity's Awake method - initializes VisualElement references from UIDocuments
     */
    void Awake()
    {
        // Add null checks for each UIDocument to prevent errors
        if (selectionDoc != null)
            selection = selectionDoc.rootVisualElement;
        else
            Debug.LogError("❌ GameUIManager: selectionDoc is not assigned!");

        if (tutorialDoc != null)
            tutorial = tutorialDoc.rootVisualElement;
        else
            Debug.LogError("❌ GameUIManager: tutorialDoc is not assigned!");

        if (profileDoc != null)
            profile = profileDoc.rootVisualElement;
        else
            Debug.LogError("❌ GameUIManager: profileDoc is not assigned!");

        if (profileEditorDoc != null)
            profileEditor = profileEditorDoc.rootVisualElement;
        else
            Debug.LogError("❌ GameUIManager: profileEditorDoc is not assigned!");
    }

    /*
     * Unity's Start method - shows the selection panel by default
     */
    void Start()
    {
        ShowSelection();
        Debug.Log("🟢 GameUIManager.Start() called");
        Debug.Log($"🟢 tutorialDoc assigned: {tutorialDoc != null}");
    }

    /*
     * Shows the main menu/city selection panel and hides all others
     */
    public void ShowSelection()
    {
        Debug.Log("📢 ShowSelection() called");
        HideAll();

        if (selection != null)
            selection.style.display = DisplayStyle.Flex;
        else
            Debug.LogError("❌ Cannot show selection: selection is null!");
    }

    /*
     * Shows the tutorial panel and hides all others
     */
    public void ShowTutorial()
    {
        Debug.Log("📢 ShowTutorial() called");
        Debug.Log($"📢 tutorialDoc assigned: {tutorialDoc != null}");

        HideAll();

        if (tutorial != null)
        {
            tutorial.style.display = DisplayStyle.Flex;
            tutorial.BringToFront(); // Ensure tutorial appears on top
            Debug.Log("✅ Tutorial panel should now be visible");
        }
        else
        {
            Debug.LogError("❌ Cannot show tutorial: tutorial is null!");
        }
    }

    /*
     * Shows the profile panel and loads profile data
     */
    public void ShowProfile()
    {
        Debug.Log("📢 ShowProfile() called");
        HideAll();

        if (profile != null)
        {
            profile.style.display = DisplayStyle.Flex;

            // Load profile data if profile manager is available
            if (profileManager != null)
                profileManager.LoadProfile();
            else
                Debug.LogError("❌ Cannot load profile: profileManager is null!");
        }
        else
        {
            Debug.LogError("❌ Cannot show profile: profile is null!");
        }
    }

    /*
     * Shows the profile editor panel
     */
    public void ShowEditProfile()
    {
        Debug.Log("📢 ShowEditProfile() called");
        HideAll();

        if (profileEditor != null)
            profileEditor.style.display = DisplayStyle.Flex;
        else
            Debug.LogError("❌ Cannot show profile editor: profileEditor is null!");
    }

    /*
     * Hides the tutorial panel and returns to selection menu
     */
    public void HideTutorial()
    {
        Debug.Log("📖 [HIDE TUTORIAL] Called");

        if (tutorial != null)
        {
            tutorial.style.display = DisplayStyle.None;
            Debug.Log("✅ Tutorial panel hidden");
        }
        else
        {
            Debug.LogError("❌ tutorial is null in HideTutorial!");
        }

        // Return to main menu after hiding tutorial
        ShowSelection();
    }

    /*
     * Hides all UI panels by setting their display style to None
     */
    private void HideAll()
    {
        if (selection != null) selection.style.display = DisplayStyle.None;
        if (tutorial != null) tutorial.style.display = DisplayStyle.None;
        if (profile != null) profile.style.display = DisplayStyle.None;
        if (profileEditor != null) profileEditor.style.display = DisplayStyle.None;
    }
}