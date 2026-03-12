using UnityEngine;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private UIDocument selectionDoc;
    [SerializeField] private UIDocument tutorialDoc;
    [SerializeField] private UIDocument scoresDoc;
    [SerializeField] private UIDocument profileDoc;
    [SerializeField] private ProfileManager profileManager;
    [SerializeField] private UIDocument profileEditorDoc;

    private VisualElement selection;
    private VisualElement tutorial;
    private VisualElement scores;
    private VisualElement profile;
    private VisualElement profileEditor;

    void Awake()
    {
        selection = selectionDoc.rootVisualElement;
        tutorial = tutorialDoc.rootVisualElement;
        scores = scoresDoc.rootVisualElement;
        profile = profileDoc.rootVisualElement;
        profileEditor = profileEditorDoc.rootVisualElement;
    }

    void Start()
    {
        ShowSelection(); // Show main menu at start
    }

    public void ShowSelection()
    {
        HideAll();
        selection.style.display = DisplayStyle.Flex;
    }

    public void ShowTutorial()
    {
        HideAll();
        tutorial.style.display = DisplayStyle.Flex;
    }

    public void ShowScores()
    {
        HideAll();
        scores.style.display = DisplayStyle.Flex;
    }

    public void ShowProfile()
    {
        HideAll();
        profile.style.display = DisplayStyle.Flex;
        profileManager.LoadProfile();
    }

    public void ShowEditProfile()
    {
        HideAll();
        profileEditor.style.display = DisplayStyle.Flex;
    }

    private void HideAll()
    {
        selection.style.display = DisplayStyle.None;
        tutorial.style.display = DisplayStyle.None;
        scores.style.display = DisplayStyle.None;
        profile.style.display = DisplayStyle.None;
        profileEditor.style.display = DisplayStyle.None;
    }
}
