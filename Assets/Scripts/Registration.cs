using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;
// using Button = UnityEngine.UIElements.Button;

public class Registration : MonoBehaviour
{
    // private VisualElement root;
    // [SerializeField] private UIDocument welcomeDoc;
    // private VisualElement welcomePanel;
    public InputField emailInput;
    public InputField firstnameInput;
    public InputField lastnameInput;
    public InputField usernameInput;
    public InputField passwordInput;

    public Button submitButton;
    // private Button backButton;
    // private Button createButton;

    // private void Awake()
    // {
    //     // fetch the panel as soon as it is initialized
    //     root = GetComponent<UIDocument>().rootVisualElement;
    //     welcomePanel = welcomeDoc.rootVisualElement;
    // }

    // private void OnEnable()
    // {
    //     backButton = root.Q<Button>("BackButton");
    //     backButton.clicked += OnBackClicked;

    //     createButton = root.Q<Button>("CreateAccountButton");
    //     createButton.clicked += OnCreateAccountClicked;
    // }

    // private void OnBackClicked()
    // {
    //     root.style.display = DisplayStyle.None;
    //     welcomePanel.style.display = DisplayStyle.Flex;
    // }

    // private void OnCreateAccountClicked()
    // {}

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", emailInput.text);
        form.AddField("firstname", firstnameInput.text);
        form.AddField("lastname", lastnameInput.text);
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
        // place constraints on input fields here, such as length, special characters, etc.
    }
}
