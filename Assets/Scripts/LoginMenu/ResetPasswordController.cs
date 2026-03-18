using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class ResetPasswordController : MonoBehaviour
{
    private VisualElement root;
    [SerializeField] UIManager uiManager;
    private TextField usernameInput;
    private TextField newPasswordInput;
    private TextField confirmPasswordInput;
    private Toggle passwordToggle;
    private Label successLabel;
    private Button backButton;
    private Button resetButton;

    private void Awake()
    {
        // fetch the panel as soon as it is initialized
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        backButton = root.Q<Button>("BackButton");
        backButton.clicked += OnBackClicked;

        resetButton = root.Q<Button>("ResetButton");
        resetButton.clicked += OnResetPasswordClicked;

        // Retrieve the input fields
        usernameInput = root.Q<TextField>("UsernameField");
        newPasswordInput = root.Q<TextField>("NewPasswordField");
        confirmPasswordInput = root.Q<TextField>("ConfirmPasswordField");
        passwordToggle = root.Q<Toggle>("ShowPasswordToggle");
        successLabel = root.Q<Label>("SuccessStatement");

        // Set password fields to masked initially
        newPasswordInput.isPasswordField = true;
        confirmPasswordInput.isPasswordField = true;

        // Adding listener to toggle
        passwordToggle.RegisterValueChangedCallback(evt =>
        {
            newPasswordInput.isPasswordField = !evt.newValue; // true = masked, false = visible
            confirmPasswordInput.isPasswordField = !evt.newValue; // true = masked, false = visible
        });
    }

    private void OnBackClicked()
    {
        uiManager.ShowLogin();
    }

    private void OnResetPasswordClicked()
    {
        CallResetPassword();
    }

    private void CallResetPassword()
    {
        StartCoroutine(ResetPassword());
    }

    IEnumerator ResetPassword()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("newPassword", newPasswordInput.text);
        form.AddField("confirmPassword", confirmPasswordInput.text);

        using (UnityWebRequest www = UnityWebRequest.Post(DBManager.hostname + "/resetpassword.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
                successLabel.text = "An error occurred. Please try again.";
            }
            else
            {
                string response = www.downloadHandler.text;
                if (response == "0")
                {
                    successLabel.text = "Password reset successful!";
                }
                else
                {
                    Debug.LogError("Reset password failed. Error #: " + response);
                    successLabel.text = "Reset password failed. " + response;
                }
            }
        }
    }
}
