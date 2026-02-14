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

    private Button registerButton;
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
    }

    private void OnBackClicked()
    {
        uiManager.ShowWelcome();
    }

    private void OnLoginClicked()
    {
        root.style.display = DisplayStyle.None;
    }

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("password", passwordInput.text);
        WWW www = new WWW("http://localhost/SQLConnect/register.php", form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("User created successfully");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); // replace 0 with going to login or main menu scene
        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.text);
        }
    }

    public void VerifyInputs()
    {
        // place constraints on username and password here, such as length, special characters, etc.
    }
}
