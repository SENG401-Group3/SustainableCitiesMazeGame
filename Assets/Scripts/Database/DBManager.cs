using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class DBManager
{
    public static string hostname = "http://sustainabilitymazegame.mysql.database.azure.com/SQLConnect";
    public static string firstname;
    public static string lastname;
    public static string username;
    public static int highScore;
    public static int cityNumber;
    public static int currentScore;
    public static bool LoggedIn { get {return username != null;}}
    public static IEnumerator LogOut()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("highscore", highScore);
        form.AddField("citynumber", cityNumber);
        form.AddField("currentscore", currentScore);

        using (UnityWebRequest request = UnityWebRequest.Post(hostname + "/saveplayerprogress.php", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Successfully saved and logged out!");
            }
            else
            {
                Debug.Log("Error updating score: " + request.error);
            }
        }
        
        username = null;
        firstname = null;
        lastname = null;
        highScore = 0;
        cityNumber = 0;
        currentScore = 0;
    }
}
