using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class Registration : MonoBehaviour
{
    private VisualElement root;
    [SerializeField] UIManager uiManager;
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
        // Fetch the panel as soon as it is initialized
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        // Retrieving and configuring buttons
        backButton = root.Q<Button>("BackButton");
        backButton.clicked += OnBackClicked;

        createButton = root.Q<Button>("CreateAccountButton");
        createButton.clicked += OnCreateAccountClicked;

        // Retrieve the input fields
        firstnameInput = root.Q<TextField>("FirstNameField");
        lastnameInput = root.Q<TextField>("LastNameField");
        usernameInput = root.Q<TextField>("UsernameField");

        passwordInput = root.Q<TextField>("PasswordField");
        passwordToggle = root.Q<Toggle>("ShowPasswordToggle");

        successLabel = root.Q<Label>("SuccessStatement");

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

    private void OnCreateAccountClicked()
    {
        bool verified = VerifyInputs();
        if (verified) CallRegister();
    }

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("firstname", firstnameInput.value.Trim());
        form.AddField("lastname", lastnameInput.value.Trim());
        form.AddField("username", usernameInput.value.Trim());
        form.AddField("password", passwordInput.value.Trim());

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/SQLConnect/register.php", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;

                if (response == "0")
                {
                    Debug.Log("User created successfully");
                    successLabel.text = "Account created successfully!";
                    successLabel.style.visibility = Visibility.Visible;

                    firstnameInput.value = "";
                    lastnameInput.value = "";
                    usernameInput.value = "";
                    passwordInput.value = "";

                    yield return new WaitForSeconds(2f);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
                else
                {
                    Debug.Log("User creation failed. Error #" + response);
                    successLabel.text = "Failed to create user";
                    successLabel.style.visibility = Visibility.Visible;
                }
            }
            else
            {
                Debug.Log("Network Error: " + request.error);
                ShowError("Connection failed. Check server.");
            }
        }
    }

    public bool VerifyInputs()
    {
        // Placing constraints on input fields, such as length, special characters, and more
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
            return false;
        }

        if (username.Length < 6 || password.Length < 8 || password.Length > 20)
        {
            ShowError("Your username should be at least 6 characters long and your password should be 8 - 20 charcters long.");
            return false;
        }

        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_@!]+$"))
        {
            ShowError("Only letters, numbers, and a few special characters (_ @ !)");
            return false;
        }

        if (!Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[\W_]).+$"))
        {
            ShowError("Password must contain at least 1 uppercase letter and 1 special character");
            return false;
        }

        return true;
    }

    public void ShowError(string message)
    {
        successLabel.text = message;
        successLabel.style.visibility = Visibility.Visible;
    }
}
