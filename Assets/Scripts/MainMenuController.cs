using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private VisualElement root;
    [SerializeField] private UIDocument signupDoc;
    [SerializeField] private UIDocument loginDoc;
    [SerializeField] private UIDocument aboutDoc;
    private VisualElement signupPanel;
    private VisualElement loginPanel;
    private VisualElement aboutPanel;
    public VisualElement login;
    public Button loginButton;
    public Button signUpButton;
    public Button aboutButton;
    private void Awake()
    {
        // fetch the panel as soon as it is initialized
        root = GetComponent<UIDocument>().rootVisualElement;
        signupPanel = signupDoc.rootVisualElement;
        loginPanel = loginDoc.rootVisualElement;
        aboutPanel = aboutDoc.rootVisualElement;
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
        root.style.display = DisplayStyle.None;
        loginPanel.style.display = DisplayStyle.Flex;
    }

    private void OnSignUpClicked()
    {
        root.style.display = DisplayStyle.None;
        signupPanel.style.display = DisplayStyle.Flex;
    }

    private void OnAboutClicked()
    {
        root.style.display = DisplayStyle.None;
        aboutPanel.style.display = DisplayStyle.Flex;
    }

    void Start()
    {
        HideAll();
        root.style.display = DisplayStyle.Flex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HideAll()
    {
        root.style.display = DisplayStyle.None;
        signupPanel.style.display = DisplayStyle.None;
        loginPanel.style.display = DisplayStyle.None;
        aboutPanel.style.display = DisplayStyle.None;
    }
}
