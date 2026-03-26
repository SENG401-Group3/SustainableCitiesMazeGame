using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


/* Manages the player's score during gameplay.
 Handles progress saving, scoring, and communication with CityUpdater. */

public class CityGameManager : MonoBehaviour
{
    [Header("Session Data")]
    private int sessionScore = 0;   // Score earned in current city session
    private int currentCity;       // Current city index (1-5)

    
    /// Singleton instance for global access
    
    public static CityGameManager Instance { get; private set; }

    /*Unity's Awake method - sets up singleton pattern and persistence*/
    
    public void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep alive when loading new scenes
            Debug.Log("✅ CityGameManager set to persist between scenes");
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            Debug.Log("❌ Duplicate CityGameManager destroyed");
        }
    }

    
    // Unity's Start method - initializes city data and loads progress
 
    public void Start()
    {
        currentCity = PlayerPrefs.GetInt("CurrentCity", 1);

        // Safety check: If on City 1 but TotalScore is abnormally high, reset
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        if (currentCity == 1 && totalScore > 100)
        {
            Debug.LogWarning($"⚠️ Detected possible score accumulation! TotalScore is {totalScore} but we're on City 1. Resetting...");
            PlayerPrefs.SetInt("TotalScore", 0);
            PlayerPrefs.Save();
            totalScore = 0;
        }

        Debug.Log($"🏁 CityGameManager Started for City {currentCity}. Initial sessionScore: {sessionScore}, TotalScore: {totalScore}");

        // Reset session data when starting a new game (City 1)
        if (currentCity == 1)
        {
            sessionScore = 0;
            Debug.Log("🆕 City 1 detected - session score reset to 0");
        }

        // Load any existing progress for this city
        LoadCityProgress();
    }

    
    // Loads saved progress for the current city from PlayerPrefs
    private void LoadCityProgress()
    {
        // Retrieve temporary score for this city (if any)
        int savedScore = PlayerPrefs.GetInt($"City{currentCity}TempScore", -1);

        if (savedScore > 0)
        {
            sessionScore = savedScore;
            Debug.Log($"📂 Loaded saved progress for City {currentCity}: Score={sessionScore}");
        }

        UpdateUI();
    }

   
    // Adds points when player answers correctly
    public void AddScore(int points)
    {
        sessionScore += points;

        Debug.Log($"➕ Added {points} points. City {currentCity} total now: {sessionScore}");

        // Save progress after each addition
        StartCoroutine(SaveProgress(points));

        UpdateUI();

        // Database update code is commented out to avoid null reference errors
        // StartCoroutine(UpdateScore(points));
    }

    // Saves temporary progress for the current city to PlayerPrefs
    
    IEnumerator SaveProgress(int points)
    {
        DBManager.currentScore += points;
        /*PlayerPrefs.SetInt($"City{currentCity}TempScore", sessionScore);
        PlayerPrefs.Save();

        Debug.Log($"💾 Saved progress for City {currentCity}: Score={sessionScore}");*/
        Debug.Log("city number after answer: " + DBManager.cityNumber);

        if(DBManager.cityNumber + 1 > 5)
        {
            Debug.Log("city number on cycling: " + DBManager.cityNumber);
            DBManager.cityNumber = 1;

            if (DBManager.currentScore > DBManager.highScore)
            {
                DBManager.highScore = DBManager.currentScore;
            }

            DBManager.currentScore = 0;
        }
        else
        {
            Debug.Log("city number before increment: " + DBManager.cityNumber);
            DBManager.cityNumber += 1;

            if (DBManager.currentScore > DBManager.highScore)
            {
                DBManager.highScore = DBManager.currentScore;
            }
        }

        WWWForm form = new WWWForm();
        form.AddField("username", DBManager.username);
        form.AddField("highscore", DBManager.highScore);
        form.AddField("citynumber", DBManager.cityNumber);
        form.AddField("currentscore", DBManager.currentScore);

        using (UnityWebRequest request = UnityWebRequest.Post(DBManager.hostname + "/saveplayerprogress.php", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Successfully saved!");
            }
            else
            {
                Debug.Log("Error saving progress: " + request.error);
            }
        }
    }


    /// Gets the current session score (called by CityUpdater)
    
    public int GetPlayerScore()
    {
        Debug.Log($"📤 GetPlayerScore called for City {currentCity}: returning {sessionScore}");
        return sessionScore;
    }

  
    /// Updates UI elements with current score
  
    private void UpdateUI()
    {
        // UI text elements have been removed per request
        // Keeping method structure for potential future UI implementation
        Debug.Log($"🖥️ UI Update would show: Score={sessionScore}");
    }

    
    /*Clears temporary progress when a city is completed
    Called by CityUpdater*/
   
    public void ClearTempProgress()
    {
        Debug.Log($"🧹 Clearing progress for City {currentCity}. Final score: {sessionScore}");

        sessionScore = 0;

        PlayerPrefs.DeleteKey($"City{currentCity}TempScore");
        PlayerPrefs.Save();

        UpdateUI();
    }
}