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
        Debug.Log("🔵 GameUIManager.Awake() called");

        // Add null checks for each UIDocument to prevent errors
        if (selectionDoc != null)
        {
            selection = selectionDoc.rootVisualElement;
            Debug.Log($"✅ selectionDoc assigned: {selection != null}");
        }
        else
            Debug.LogError("❌ GameUIManager: selectionDoc is not assigned!");

        if (tutorialDoc != null)
        {
            tutorial = tutorialDoc.rootVisualElement;
            Debug.Log($"✅ tutorialDoc assigned: {tutorial != null}");
        }
        else
            Debug.LogError("❌ GameUIManager: tutorialDoc is not assigned!");

        if (profileDoc != null)
        {
            profile = profileDoc.rootVisualElement;
            Debug.Log($"✅ profileDoc assigned: {profile != null}");
        }
        else
            Debug.LogError("❌ GameUIManager: profileDoc is not assigned!");

        if (profileEditorDoc != null)
        {
            profileEditor = profileEditorDoc.rootVisualElement;
            Debug.Log($"✅ profileEditorDoc assigned: {profileEditor != null}");
        }
        else
            Debug.LogError("❌ GameUIManager: profileEditorDoc is not assigned!");
    }

    /*
     * Unity's Start method - shows the selection panel by default
     */
    void Start()
    {
        Debug.Log("🟢 GameUIManager.Start() called");
        ShowSelection(); // Show main menu at start
    }

    /*
     * Shows the main menu/city selection panel and hides all others
     */
    public void ShowSelection()
    {
        Debug.Log("📢 ShowSelection() called");
        Debug.Log($"   selection is null: {selection == null}");

        if (selection == null)
        {
            Debug.LogError("❌ selection is null! Make sure selectionDoc is assigned.");
            return;
        }

        HideAll();
        selection.style.display = DisplayStyle.Flex;
        Debug.Log("✅ Selection panel should now be visible");
    }

    /*
     * Shows the tutorial panel and hides all others
     */
    public void ShowTutorial()
    {
        Debug.Log("📢 ShowTutorial() called");
        Debug.Log($"   tutorial is null: {tutorial == null}");

        if (tutorial == null)
        {
            Debug.LogError("❌ tutorial is null! Make sure tutorialDoc is assigned.");
            return;
        }

        HideAll();
        tutorial.style.display = DisplayStyle.Flex;
        tutorial.BringToFront();
        Debug.Log("✅ Tutorial panel should now be visible");
    }

    public void ShowProfile()
    {
        Debug.Log("📢 ShowProfile() called");
        Debug.Log($"   profile is null: {profile == null}");

        if (profile == null)
        {
            Debug.LogError("❌ profile is null! Make sure profileDoc is assigned.");
            return;
        }

        HideAll();
        profile.style.display = DisplayStyle.Flex;

        if (profileManager != null)
            profileManager.LoadProfile();
        else
            Debug.LogError("❌ Cannot load profile: profileManager is null!");
    }

    /*
     * Shows the profile editor panel
     */
    public void ShowEditProfile()
    {
        Debug.Log("📢 ShowEditProfile() called");
        Debug.Log($"   profileEditor is null: {profileEditor == null}");

        if (profileEditor == null)
        {
            Debug.LogError("❌ profileEditor is null! Make sure profileEditorDoc is assigned.");
            return;
        }

        HideAll();
        profileEditor.style.display = DisplayStyle.Flex;
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