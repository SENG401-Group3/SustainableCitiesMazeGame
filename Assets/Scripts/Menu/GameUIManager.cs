using UnityEngine;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private UIDocument selectionDoc;
    [SerializeField] private UIDocument tutorialDoc;
    [SerializeField] private UIDocument scoresDoc;
    [SerializeField] private UIDocument profileDoc;
    [SerializeField] private ProfileManager profileManager;

    private VisualElement selection;
    private VisualElement tutorial;
    private VisualElement scores;
    private VisualElement profile;

    void Awake()
    {
        selection = selectionDoc.rootVisualElement;
        tutorial = tutorialDoc.rootVisualElement;
        scores = scoresDoc.rootVisualElement;
        profile = profileDoc.rootVisualElement;
    }

    void Start()
    {
        ShowSelection(); // Show main menu at start
    }

    public void ShowSelection()
    {
        selection.style.display = DisplayStyle.Flex;
        tutorial.style.display = DisplayStyle.None;
        scores.style.display = DisplayStyle.None;
        profile.style.display = DisplayStyle.None;
    }

    public void ShowTutorial()
    {
        selection.style.display = DisplayStyle.None;
        tutorial.style.display = DisplayStyle.Flex;
        scores.style.display = DisplayStyle.None;
        profile.style.display = DisplayStyle.None;
    }

    public void ShowScores()
    {
        selection.style.display = DisplayStyle.None;
        tutorial.style.display = DisplayStyle.None;
        scores.style.display = DisplayStyle.Flex;
        profile.style.display = DisplayStyle.None;
    }

    public void ShowProfile()
    {
        selection.style.display = DisplayStyle.None;
        tutorial.style.display = DisplayStyle.None;
        scores.style.display = DisplayStyle.None;
        profile.style.display = DisplayStyle.Flex;
        profileManager.LoadProfile();
    }

    /*public void ShowProfile()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.None;
        login.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.None;

        profile.style.display = DisplayStyle.Flex;
        profileManager.LoadProfile();
    }*/
}
