using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private VisualElement root;
    private VisualElement loadingSpinner;
    [SerializeField] private UIManager uiManager;
    //private VisualElement welcomePanel;
    private TextField usernameInput;
    private TextField passwordInput;
    private Label successLabel;
    private Toggle passwordToggle;
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

        loadingSpinner = root.Q<VisualElement>("LoadingSpinner");

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
        usernameInput.value = "";
        passwordInput.value = "";
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
        UIAnimator.Instance.FadeInElement(loadingSpinner, 0.2f);
        UIAnimator.Instance.RotateElement(loadingSpinner, 360f);

        successLabel.text = "Logging in...";

        UIAnimator.Instance.FadeInElement(successLabel, 0.2f);
        UIAnimator.Instance.PulseElement(successLabel);

        yield return UIAnimator.Instance.AnimateLoading(successLabel, "Logging in", 2f);

        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.value.Trim());
        form.AddField("password", passwordInput.value.Trim());

        using (UnityWebRequest request = UnityWebRequest.Post(DBManager.hostname + "/login.php", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Server response: " + request.downloadHandler.text);
                UserData data = JsonUtility.FromJson<UserData>(request.downloadHandler.text);

                if (data.error != null)
                {
                    Debug.Log("Login error: " + data.error);
                    successLabel.text = data.error;
                }

                else {
                    Debug.Log("Username is: " + data.username);

                    DBManager.firstname = data.firstname;
                    DBManager.lastname = data.lastname;
                    DBManager.username = data.username;
                    DBManager.highScore = data.highscore;
                    DBManager.cityNumber = data.citynumber;
                    DBManager.currentScore = data.currentscore;

                    usernameInput.value = "";
                    passwordInput.value = "";

                    UIAnimator.Instance.FadeOutElement(loadingSpinner, 0.2f);

                    successLabel.text = "✓ Login successful!";
                    UIAnimator.Instance.PulseElement(successLabel);
                    //successLabel.style.visibility = Visibility.Visible;

                    yield return new WaitForSeconds(1f);
                    Debug.Log("Saved username: " + DBManager.username);

                    UIAnimator.Instance.FadeOutElement(successLabel, 0.2f);

                    SceneManager.LoadScene("CitySelection");
                    successLabel.text = "";
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

    public class UserData
    {
        public string username;
        public string firstname;
        public string lastname;
        public int highscore;
        public int citynumber;
        public int currentscore;
        public string error;
    }

    
    /*public void VerifyInputs()
    {
        // place constraints on username and password here, such as length, special characters, etc.
    }*/
}
