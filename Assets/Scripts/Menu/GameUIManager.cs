using UnityEngine;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private UIDocument selectionDoc;
    [SerializeField] private UIDocument tutorialDoc;
    [SerializeField] private UIDocument profileDoc;
    [SerializeField] private ProfileManager profileManager;
    [SerializeField] private UIDocument profileEditorDoc;

    private VisualElement selection;
    private VisualElement tutorial;
    private VisualElement profile;
    private VisualElement profileEditor;

    void Awake()
    {
        // Add null checks for each UIDocument
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

    void Start()
    {
        ShowSelection(); // Show main menu at start
        Debug.Log("🟢 GameUIManager.Start() called");
        Debug.Log($"🟢 tutorialDoc assigned: {tutorialDoc != null}");
        ShowSelection();
    }

    public void ShowSelection()
    {
        HideAll();
        if (selection != null)
            selection.style.display = DisplayStyle.Flex;
        else
            Debug.LogError("❌ Cannot show selection: selection is null!");
    }

    public void ShowTutorial()
    {
        Debug.Log("🟢 STEP 4: GameUIManager.ShowTutorial() CALLED");
        Debug.Log($"🟢 tutorialDoc assigned: {tutorialDoc != null}");

        if (tutorialDoc != null)
        {
            Debug.Log($"🟢 tutorialDoc name: {tutorialDoc.gameObject.name}");
            Debug.Log($"🟢 tutorialDoc rootVisualElement exists: {tutorialDoc.rootVisualElement != null}");
        }

        Debug.Log($"🟢 tutorial VisualElement before assignment: {tutorial != null}");

        // Make sure tutorial is assigned
        if (tutorial == null && tutorialDoc != null)
        {
            tutorial = tutorialDoc.rootVisualElement;
            Debug.Log($"🟢 Re-assigned tutorial: {tutorial != null}");
        }

        HideAll();

        if (tutorial != null)
        {
            tutorial.style.display = DisplayStyle.Flex;
            tutorial.BringToFront();
            Debug.Log($"✅ Tutorial display set to: {tutorial.style.display.value}");
        }
        else
        {
            Debug.LogError("❌ Cannot show tutorial: tutorial is null!");
        }
    }

    public void ShowProfile()
    {
        HideAll();
        if (profile != null)
        {
            profile.style.display = DisplayStyle.Flex;
            if (profileManager != null)
                profileManager.LoadProfile();
            else
                Debug.LogError("❌ Cannot load profile: profileManager is null!");
        }
        else
            Debug.LogError("❌ Cannot show profile: profile is null!");
    }

    public void ShowEditProfile()
    {
        HideAll();
        if (profileEditor != null)
            profileEditor.style.display = DisplayStyle.Flex;
        else
            Debug.LogError("❌ Cannot show profile editor: profileEditor is null!");
    }

    private void HideAll()
    {
        if (selection != null) selection.style.display = DisplayStyle.None;
        if (tutorial != null) tutorial.style.display = DisplayStyle.None;
        if (profile != null) profile.style.display = DisplayStyle.None;
        if (profileEditor != null) profileEditor.style.display = DisplayStyle.None;
    }
}