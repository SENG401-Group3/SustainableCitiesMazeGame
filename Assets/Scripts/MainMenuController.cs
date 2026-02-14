using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuController : MonoBehaviour
{
    private VisualElement root;
    [SerializeField] private UIManager uiManager;
    public Button loginButton;
    public Button signUpButton;
    public Button aboutButton;
    private void Awake()
    {
        // fetch the panel as soon as it is initialized
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        loginButton = root.Q<Button>("LoginButton");
        loginButton.clicked += OnLoginClicked;

        signUpButton = root.Q<Button>("SignUpButton");
        signUpButton.clicked += OnSignUpClicked;


        aboutButton = root.Q<Button>("AboutButton");
        aboutButton.clicked += OnAboutClicked;
    }

    private void OnLoginClicked()
    {
        uiManager.ShowLogin();
    }

    private void OnSignUpClicked()
    {
       uiManager.ShowSignup();
    }

    private void OnAboutClicked()
    {
        uiManager.ShowAbout();
    }

    void Start()
    {
        /*HideAll();
        root.style.display = DisplayStyle.Flex;*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
