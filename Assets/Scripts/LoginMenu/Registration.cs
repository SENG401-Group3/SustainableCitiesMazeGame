using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;
 
public class Registration : MonoBehaviour
{
    private VisualElement root;
    private VisualElement loadingSpinner;
    [SerializeField] private UIManager uiManager;
 
    private TextField firstnameInput;
    private TextField lastnameInput;
    private TextField usernameInput;
    private TextField passwordInput;
 
    private Label successLabel;
    private Toggle passwordToggle;
    private Button backButton;
    private Button createButton;
 
    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }
 
    private void OnEnable()
    {
        backButton = root.Q<Button>("BackButton");
        createButton = root.Q<Button>("CreateAccountButton");
 
        firstnameInput = root.Q<TextField>("FirstNameField");
        lastnameInput = root.Q<TextField>("LastNameField");
        usernameInput = root.Q<TextField>("UsernameField");
        passwordInput = root.Q<TextField>("PasswordField");
 
        passwordToggle = root.Q<Toggle>("ShowPasswordToggle");
        successLabel = root.Q<Label>("SuccessStatement");

        loadingSpinner = root.Q<VisualElement>("LoadingSpinner");
 
        if (backButton != null)
            backButton.clicked += OnBackClicked;
 
        if (createButton != null)
            createButton.clicked += OnCreateAccountClicked;
 
        if (passwordInput != null)
            passwordInput.isPasswordField = true;
 
        if (passwordToggle != null)
        {
            passwordToggle.RegisterValueChangedCallback(evt =>
            {
                if (passwordInput != null)
                    passwordInput.isPasswordField = !evt.newValue;
            });
        }
 
        if (successLabel != null)
            successLabel.style.display = DisplayStyle.None;
    }
 
    private void OnDisable()
    {
        if (backButton != null)
            backButton.clicked -= OnBackClicked;
 
        if (createButton != null)
            createButton.clicked -= OnCreateAccountClicked;
    }
 
    private void OnBackClicked()
    {
        firstnameInput.value = "";
        lastnameInput.value = "";
        usernameInput.value = "";
        passwordInput.value = "";
        
        if (uiManager != null)
            uiManager.ShowWelcome();
    }
 
    private void OnCreateAccountClicked()
    {
        Debug.Log("Create account clicked!");
        if (VerifyInputs())
        {
            Debug.Log("Input verified!");
            CallRegister();
        }
    }
 
    private void CallRegister()
    {
        Debug.Log("Call Register called!");
        StartCoroutine(Register());
    }
 
    private IEnumerator Register()
    {
        Debug.Log("Register Coroutine started!");
        UIAnimator.Instance.FadeInElement(loadingSpinner, 0.2f);
        UIAnimator.Instance.RotateElement(loadingSpinner, 360f);

        successLabel.text = "Creating user...";

        UIAnimator.Instance.FadeInElement(successLabel, 0.2f);
        UIAnimator.Instance.PulseElement(successLabel);

        yield return UIAnimator.Instance.AnimateLoading(successLabel, "Creating user", 2f);

        string firstname = firstnameInput.value.Trim();
        string lastname = lastnameInput.value.Trim();
        string username = usernameInput.value.Trim();
        string password = passwordInput.value.Trim();
 
        WWWForm form = new WWWForm();
        form.AddField("firstname", firstname);
        form.AddField("lastname", lastname);
        form.AddField("username", username);
        form.AddField("password", password);
 
        string url = DBManager.hostname + "/register.php";
 
        Debug.Log("REGISTER URL: " + url);
        Debug.Log("firstname: " + firstname);
        Debug.Log("lastname: " + lastname);
        Debug.Log("username: " + username);
 
        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            request.timeout = 20;
 
            yield return request.SendWebRequest();
            Debug.Log("Result: " + request.result);
            Debug.Log("Error: " + request.error);
            Debug.Log("Response Code: " + request.responseCode);
            Debug.Log("Raw Response: [" + request.downloadHandler.text + "]");
 
            #if UNITY_2020_1_OR_NEWER
                bool hasError = request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError;
            #else
                bool hasError = request.isNetworkError || request.isHttpError;
            #endif
 
            string responseText = request.downloadHandler != null
                ? request.downloadHandler.text.Trim()
                : "";
 
            Debug.Log("Register Response Code: " + request.responseCode);
            Debug.Log("Register Response Text: " + responseText);
 
            if (hasError)
            {
                Debug.LogError("Registration request failed.");
                Debug.LogError("Network/HTTP Error: " + request.error);
                ShowError("Connection failed. Check server.");
                yield break;
            }
 
            if (responseText == "0")
            {
                UIAnimator.Instance.FadeOutElement(loadingSpinner, 0.2f);

                successLabel.text = "✓ Created User!";
                UIAnimator.Instance.PulseElement(successLabel);
                
                Debug.Log("User created successfully.");
                //DBManager.username = usernameInput.text;
 
                DBManager.firstname = firstnameInput.value;
                DBManager.lastname = lastnameInput.value;
                DBManager.username = usernameInput.value;
                DBManager.highScore = 0;
                DBManager.cityNumber = 1;
                DBManager.currentScore = 0;
 
                firstnameInput.value = "";
                lastnameInput.value = "";
                usernameInput.value = "";
                passwordInput.value = "";
 
                yield return new WaitForSeconds(1f);
                UIAnimator.Instance.FadeOutElement(successLabel, 0.2f);
 
                successLabel.text = "";
                UnityEngine.SceneManagement.SceneManager.LoadScene("Story");
            }
            else
            {
                Debug.LogWarning("User creation failed. Server response: " + responseText);
 
                if (string.IsNullOrWhiteSpace(responseText))
                    ShowError("Failed to create user.");
                else
                    ShowError(responseText);
            }
        }
    }
 
    public bool VerifyInputs()
    {
        Debug.Log("Verifying input!");
        string firstname = firstnameInput.value.Trim();
        string lastname = lastnameInput.value.Trim();
        string username = usernameInput.value.Trim();
        string password = passwordInput.value.Trim();
 
        if (string.IsNullOrEmpty(firstname) ||
            string.IsNullOrEmpty(lastname) ||
            string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(password))
        {
            ShowError("All fields must be filled!");
            Debug.Log("Verifying done!");
            return false;
        }
 
        if (username.Length < 6 || password.Length < 8 || password.Length > 20)
        {
            ShowError("Your username should be at least 6 characters long and your password should be 8 - 20 characters long.");
            Debug.Log("Verifying done!");
            return false;
        }
 
        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_@!]+$"))
        {
            ShowError("Only letters, numbers, and a few special characters (_ @ !)");
            Debug.Log("Verifying done!");
            return false;
        }
 
        if (!Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[\W_]).+$"))
        {
            ShowError("Password must contain at least 1 uppercase letter and 1 special character");
            Debug.Log("Verifying done!");
            return false;
        }
        Debug.Log("Verifying done!");
 
        return true;
    }
 
    public void ShowError(string message)
    {
        Debug.Log("Show Error called!");
        Debug.Log("Success label: " + successLabel);
        if (successLabel != null)
        {
            successLabel.text = message;
            successLabel.style.display = DisplayStyle.Flex;
            successLabel.style.visibility = Visibility.Visible;
        }
    }
}