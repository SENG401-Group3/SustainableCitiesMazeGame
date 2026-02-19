using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIDocument welcomeDoc;
    [SerializeField] private UIDocument signupDoc;
    [SerializeField] private UIDocument loginDoc;
    [SerializeField] private UIDocument leaderboardDoc;
    [SerializeField] private UIDocument aboutDoc;

    private VisualElement welcome;
    private VisualElement signup;
    private VisualElement login;
    private VisualElement leaderboard;
    private VisualElement about;

    void Awake()
    {
        welcome = welcomeDoc.rootVisualElement;
        signup = signupDoc.rootVisualElement;
        login = loginDoc.rootVisualElement;
        leaderboard = leaderboardDoc.rootVisualElement;
        about = aboutDoc.rootVisualElement;
    }

    void Start()
    {
        ShowWelcome(); // Show main menu at start
    }

    public void ShowWelcome()
    {
        welcome.style.display = DisplayStyle.Flex;
        signup.style.display = DisplayStyle.None;
        login.style.display = DisplayStyle.None;
        leaderboard.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.None;
    }

    public void ShowSignup()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.Flex;
        login.style.display = DisplayStyle.None;
        leaderboard.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.None;
    }

    public void ShowLogin()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.None;
        login.style.display = DisplayStyle.Flex;
        leaderboard.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.None;
    }

    public void ShowLeaderboard()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.None;
        login.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.None;
        leaderboard.style.display = DisplayStyle.Flex;
    }

    public void ShowAbout()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.None;
        login.style.display = DisplayStyle.None;
        leaderboard.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.Flex;
    }
}
