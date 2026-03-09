using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIDocument welcomeDoc;
    [SerializeField] private UIDocument signupDoc;
    [SerializeField] private UIDocument loginDoc;
    [SerializeField] private UIDocument aboutDoc;
    [SerializeField] private UIDocument profileDoc;
    [SerializeField] private UIDocument resetPasswordDoc;
    [SerializeField] private ProfileManager profileManager;

    private VisualElement welcome;
    private VisualElement signup;
    private VisualElement login;
    private VisualElement about;
    private VisualElement profile;
    private VisualElement resetPassword;

    void Awake()
    {
        welcome = welcomeDoc.rootVisualElement;
        signup = signupDoc.rootVisualElement;
        login = loginDoc.rootVisualElement;
        about = aboutDoc.rootVisualElement;
        profile = profileDoc.rootVisualElement;
        resetPassword = resetPasswordDoc.rootVisualElement;
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
        about.style.display = DisplayStyle.None;
        profile.style.display = DisplayStyle.None;
        resetPassword.style.display = DisplayStyle.None;
    }

    public void ShowSignup()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.Flex;
        login.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.None;
        profile.style.display = DisplayStyle.None;
        resetPassword.style.display = DisplayStyle.None;
    }

    public void ShowLogin()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.None;
        login.style.display = DisplayStyle.Flex;
        about.style.display = DisplayStyle.None;
        profile.style.display = DisplayStyle.None;
        resetPassword.style.display = DisplayStyle.None;
    }

    public void ShowAbout()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.None;
        login.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.Flex;
        profile.style.display = DisplayStyle.None;
        resetPassword.style.display = DisplayStyle.None;
    }

    public void ShowProfile()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.None;
        login.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.None;
        resetPassword.style.display = DisplayStyle.None;

        profile.style.display = DisplayStyle.Flex;
        profileManager.LoadProfile();
    }

    public void ShowResetPassword()
    {
        welcome.style.display = DisplayStyle.None;
        signup.style.display = DisplayStyle.None;
        login.style.display = DisplayStyle.None;
        about.style.display = DisplayStyle.None;
        profile.style.display = DisplayStyle.None;

        resetPassword.style.display = DisplayStyle.Flex;
    }
}
