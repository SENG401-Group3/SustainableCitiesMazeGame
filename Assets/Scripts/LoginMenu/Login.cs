using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;

    public Button submitButton;

    public void CallLogin()
    {
        StartCoroutine(LoginPlayer());
    }

    IEnumerator LoginPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("password", passwordInput.text);
        WWW www = new WWW("http://localhost/SQLConnect/login.php", form);
        yield return www;
        if (www.text[0] == '0')
        {
            DBManager.username = usernameInput.text;
            DBManager.score = int.Parse(www.text.Split('\t')[1]);
        }
        else
        {
            Debug.Log("User login failed. Error #" + www.text);
        }
    }

    public void VerifyInputs()
    {
        // place constraints on username and password here, such as length, special characters, etc.
    }
}
