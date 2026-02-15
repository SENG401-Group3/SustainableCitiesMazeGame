using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.UIElements;

public class LoginController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private VisualElement root;
    [SerializeField] private UIManager uiManager;
    //private VisualElement welcomePanel;
    private TextField usernameInput;
    private TextField passwordInput;
    private Label successLabel;
    private Toggle passwordToggle;

    //private Button registerButton;
    private Button backButton;
    private Button loginButton;

    private void Awake()
    {
        // fetch the panel as soon as it is initialized
        root = GetComponent<UIDocument>().rootVisualElement;
        //welcomePanel = welcomeDoc.rootVisualElement;
    }

    private void OnEnable()
    {
        backButton = root.Q<Button>("BackButton");
        backButton.clicked += OnBackClicked;

        loginButton = root.Q<Button>("LoginButton");
        loginButton.clicked += OnLoginClicked;

        // Retrieve the input fields
        usernameInput = root.Q<TextField>("UsernameField");
        successLabel = root.Q<Label>("SuccessStatement");

        passwordInput = root.Q<TextField>("PasswordField");
        passwordToggle = root.Q<Toggle>("ShowPasswordToggle");

        // Set password field to masked initially
        passwordInput.isPasswordField = true;

        // Adding listener to toggle
        passwordToggle.RegisterValueChangedCallback(evt =>
        {
            passwordInput.isPasswordField = !evt.newValue; // true = masked, false = visible
        });
    }

    private void OnBackClicked()
    {
        uiManager.ShowWelcome();
    }

    private void OnLoginClicked()
    {
        CallLogin();
    }

    public void CallLogin()
    {
        StartCoroutine(LoginPlayer());
    }

    IEnumerator LoginPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.value.Trim());
        form.AddField("password", passwordInput.value.Trim());

        WWW www = new WWW("http://localhost/SQLConnect/login.php", form);
        yield return www;

        string response = www.text.Trim();
        Debug.Log("Server response: " + response);

        if (response == "0")
        {
            DBManager.username = usernameInput.value;

            usernameInput.value = "";
            passwordInput.value = "";
            successLabel.style.visibility = Visibility.Visible;

            yield return new WaitForSeconds(3f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else
        {
            successLabel.text = "Login failed!";
            successLabel.style.visibility = Visibility.Visible;
            Debug.Log("User login failed. Error #" + response);
        }
    }

    public void VerifyInputs()
    {
        // place constraints on username and password here, such as length, special characters, etc.
    }
}
