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
        ShowSelection(); // Show main menu at start
    }

    /*
     * Shows the main menu/city selection panel and hides all others
     */
    public void ShowSelection()
    {
        Debug.Log("📢 ShowSelection() called");
        HideAll();
        selection.style.display = DisplayStyle.Flex;
    }

    /*
     * Shows the tutorial panel and hides all others
     */
    public void ShowTutorial()
    {
        HideAll();
        tutorial.style.display = DisplayStyle.Flex;
    }

    public void ShowProfile()
    {
        Debug.Log("📢 ShowProfile() called");
        HideAll();
        profile.style.display = DisplayStyle.Flex;
        profileManager.LoadProfile();
    }

    /*
     * Shows the profile editor panel
     */
    public void ShowEditProfile()
    {
        Debug.Log("📢 ShowEditProfile() called");
        HideAll();
        profileEditor.style.display = DisplayStyle.Flex;
    }

    /*
     * Hides all UI panels by setting their display style to None
     */
    private void HideAll()
    {
        selection.style.display = DisplayStyle.None;
        tutorial.style.display = DisplayStyle.None;
        profile.style.display = DisplayStyle.None;
        profileEditor.style.display = DisplayStyle.None;
    }
}
