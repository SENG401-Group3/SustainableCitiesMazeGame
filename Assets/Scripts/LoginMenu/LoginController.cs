using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

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
    private Button forgotPasswordButton;

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

        forgotPasswordButton = root.Q<Button>("ForgotPasswordButton");
        forgotPasswordButton.clicked += OnForgotPasswordClicked;

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

    private void OnForgotPasswordClicked()
    {
        uiManager.ShowResetPassword();
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

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/SQLConnect/login.php", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text.Trim();
                Debug.Log("Server response: " + response);

                if (response == "0")
                {
                    DBManager.username = usernameInput.value.Trim();

                    usernameInput.value = "";
                    passwordInput.value = "";

                    successLabel.text = "Login successful!";
                    successLabel.style.visibility = Visibility.Visible;

                    yield return new WaitForSeconds(1f);
                    Debug.Log("Saved username: " + DBManager.username);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                    successLabel.text = "";
                }
                else
                {
                    successLabel.text = "Login failed!";
                    successLabel.style.visibility = Visibility.Visible;
                    Debug.Log("User login failed. Error #" + response);
                }
            }
            else
            {
                Debug.Log("Network Error: " + request.error);
                successLabel.text = "Connection error!";
                successLabel.style.visibility = Visibility.Visible;
            }
        }
    }


    /*public void VerifyInputs()
    {
        // place constraints on username and password here, such as length, special characters, etc.
    }*/
}
