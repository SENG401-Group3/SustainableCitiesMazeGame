using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuController : MonoBehaviour
{
    private VisualElement root;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TypewriterText typewriter;
    private Button loginButton;
    private Button signUpButton;
    private Button guestButton;
    private Button aboutButton;
    private Label welcomeLabel;

    private void Awake()
    {
        // fetch the panel as soon as it is initialized
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        // Fetching the buttons and adding events
        loginButton = root.Q<Button>("LoginButton");
        loginButton.clicked += OnLoginClicked;

        signUpButton = root.Q<Button>("SignUpButton");
        signUpButton.clicked += OnSignUpClicked;

        guestButton = root.Q<Button>("GuestButton");
        guestButton.clicked += OnGuestClicked;

        aboutButton = root.Q<Button>("AboutButton");
        aboutButton.clicked += OnAboutClicked;

        //Typing the heading label
        welcomeLabel = root.Q<Label>("WelcomeLabel");
        /*Debug.Log(TypewriterText.Instance);*/
        typewriter.StartTyping(welcomeLabel, welcomeLabel.text);
    }

    private void OnLoginClicked()
    {
        uiManager.ShowLogin();
    }

    private void OnSignUpClicked()
    {
       uiManager.ShowSignup();
    }

    private void OnGuestClicked()
    {
        DBManager.username = "Guest";
        UnityEngine.SceneManagement.SceneManager.LoadScene("Story");
    }

    private void OnAboutClicked()
    {
        uiManager.ShowAbout();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
