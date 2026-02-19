using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;

public class LeaderboardController : MonoBehaviour
{
    private VisualElement root;
    [SerializeField] UIManager uiManager;

    private Button backButton;

    private void Awake()
    {
        // fetch the panel as soon as it is initialized
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        backButton = root.Q<Button>("BackButton");
        backButton.clicked += OnBackClicked;
        CallLeaderboard();
    }

    private void OnBackClicked()
    {
        uiManager.ShowWelcome();
    }

    public void CallLeaderboard()
    {
        StartCoroutine(Leaderboard());
    }

    IEnumerator Leaderboard()
    {
        WWW www = new WWW("http://localhost/SQLConnect/leaderboard.php");
        
        yield return www;

        string response = www.text;
        Debug.Log("Server Response: " + response);
        if (response[0] == '0')
        {
            string[] entries = www.text.Split('\n');
            for (int i = 1; i < entries.Length - 1; i++)
            {
                string[] entry = entries[i].Split('\t');
                Debug.Log(entry[0] + "\t\t" + entry[1]);
            }
        }
        else
        {
            Debug.Log("Failed to retrieve leaderboard. Error #" + www.text);
        }
    }
}
