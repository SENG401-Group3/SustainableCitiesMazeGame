using UnityEngine;
using UnityEngine.Networking;

public class SaveUserProgressController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", PlayerPrefs.GetString("username"));
        form.AddField("city", PlayerPrefs.GetString("city"));
        form.AddField("score", PlayerPrefs.GetInt("score"));

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/SQLConnect/saveplayerprogress.php", form))
        {
            request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Progress saved successfully");
            }
            else
            {
                Debug.Log("Network Error: " + request.error);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
