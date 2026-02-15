using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;
using UnityEngine.UIElements;

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

    //public Button submitButton;
    private Button backButton;
    private Button createButton;

    private void Awake()
    {
        // fetch the panel as soon as it is initialized
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
        CallRegister();
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
        WWW www = new WWW("http://localhost/SQLConnect/register.php", form);
        

        yield return www;
        if (www.text == "0")
        {
            Debug.Log("User created successfully");
            successLabel.style.visibility = Visibility.Visible;

            usernameInput.value = "";
            passwordInput.value = "";

            yield return new WaitForSeconds(3f); //wait 3 seconds
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); // replace 0 with going to login or main menu scene
        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.text);
            successLabel.text = "Failed to create user";
            successLabel.style.visibility = Visibility.Visible;
        }
    }

    public void VerifyInputs()
    {
        // place constraints on input fields here, such as length, special characters, etc.
    }
}
