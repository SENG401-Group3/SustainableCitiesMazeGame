using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIDocument welcomeDoc;
    [SerializeField] private UIDocument signupDoc;
    [SerializeField] private UIDocument loginDoc;
    [SerializeField] private UIDocument aboutDoc;

    private VisualElement welcome;
    private VisualElement signup;
    private VisualElement login;
    private VisualElement about;

    void Awake()
    {
        welcome = welcomeDoc.rootVisualElement;
        signup = signupDoc.rootVisualElement;
        login = loginDoc.rootVisualElement;
        about = aboutDoc.rootVisualElement;
    }

    void Start()
    {
        ShowWelcome(); // Show main menu at start
    }

    public void ShowWelcome()
    {
        HideAll();
        welcome.style.display = DisplayStyle.Flex;
    }

    public void ShowSignup()
    {
        HideAll();
        signup.style.display = DisplayStyle.Flex;
    }

    public void ShowLogin()
    {
        HideAll();
        login.style.display = DisplayStyle.Flex;
    }

    public void ShowAbout()
    {
        HideAll();
        about.style.display = DisplayStyle.Flex;
    }

    public void HideAll()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.None;
        login.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.None;
    }
}
