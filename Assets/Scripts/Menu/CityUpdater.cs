using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

/* Handles city progression logic, score saving, and scene transitions.
 This component persists between scenes to track which city the player is in.*/

public class CityUpdater : MonoBehaviour
{
    //Current city index (1-5) that the player is playing
    private int currentCity;

    // Singleton instance for global access
    public static CityUpdater Instance { get; private set; }

    // Unity's Awake method - sets up singleton pattern and persistence
    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep alive when loading new scenes
            Debug.Log("✅ CityUpdater set to persist between scenes");
        }
        else
        {
            Debug.Log("❌ Duplicate CityUpdater detected - destroying");
            DestroyImmediate(gameObject); // Use DestroyImmediate for tests
        }
    }

    // Subscribes to scene loaded events when the object is enabled
    private void OnEnable()
    {
        // Subscribe to scene loaded event to update city when scenes change
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unsubscribes from scene loaded events to prevent memory leaks
    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /* Called whenever a new scene is loaded
     Updates the current city from PlayerPrefs
    
    <param name="scene">The scene that was loaded</param>
    <param name="mode">How the scene was loaded</param> */
    /*private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update currentCity whenever a new scene loads
        RefreshCurrentCity();
        Debug.Log($"Scene loaded: {scene.name}. Current city updated to: {currentCity}");
    }*/

    // Unity's Start method - initializes the current city
    private void Start()
    {
        RefreshCurrentCity();
    }

    // Reads the current city from PlayerPrefs and updates the local variable
    private void RefreshCurrentCity()
    {
        currentCity = PlayerPrefs.GetInt("CurrentCity", 1);
        Debug.Log($"CityUpdater refreshed. Current city: {currentCity}");
    }

    /* Main method called when a city is completed.
    Saves the player's score, updates progression, and loads the appropriate next scene.*/
    public void CompleteCity()
    {
        // Get the latest city value from PlayerPrefs
        RefreshCurrentCity();

        // Find the CityGameManager to retrieve the player's score for this city
        CityGameManager gameManager = FindFirstObjectByType<CityGameManager>();

        // Get player score from this city (default to 0 if manager not found)
        int playerScore = gameManager != null ? gameManager.GetPlayerScore() : 0;

        Debug.Log($"=== Completing City {currentCity} ===");
        Debug.Log($"PlayerScore from this city: {playerScore}");

        /* SAVE TOTAL PROGRESS
        Add this city's score to the overall total score*/
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        int newTotalScore = totalScore + playerScore;
        PlayerPrefs.SetInt("TotalScore", newTotalScore);

        Debug.Log($"Previous TotalScore: {totalScore}");
        Debug.Log($"New TotalScore: {newTotalScore}");

        /* SAVE PER-CITY PROGRESS 
         Store this city's score and mark it as completed*/
        PlayerPrefs.SetInt($"City{currentCity}Score", playerScore);
        PlayerPrefs.SetInt($"City{currentCity}Completed", 1);

        // Clear temporary progress for this city (e.g., in-progress scores)
        if (gameManager != null)
        {
            gameManager.ClearTempProgress();
        }

        //HANDLE CITY PROGRESSION 
        if (currentCity < 5)
        {
            // Move to the next city
            int nextCity = currentCity + 1;
            PlayerPrefs.SetInt("CurrentCity", nextCity);
            PlayerPrefs.SetInt("LastCompletedCity", currentCity);
            PlayerPrefs.SetInt("ShowSustainabilityMessage", nextCity);

            Debug.Log($"✅ City {currentCity} complete! Moving to City {nextCity}");

            // Save all PlayerPrefs changes
            PlayerPrefs.Save();

            // Force refresh of current city after saving
            RefreshCurrentCity();

            // Return to city selection menu to show the next city
            SceneManager.LoadScene("CitySelection");
        }
        else
        {
            // CITY 5 COMPLETED - GAME OVER
            Debug.Log("🎉 GAME COMPLETE! City 5 finished.");

            // Set GameComplete flag to trigger leaderboard (keep CurrentCity as 5 for now)
            PlayerPrefs.SetInt("GameComplete", 1);

            // Save the final total
            PlayerPrefs.Save();

            // Go to leaderboard scene directly to show final score
            SceneManager.LoadScene("LeaderboardScene");
        }
    }
    
    /*IEnumerator SaveProgress(int score)
    {
        if (DBManager.currentScore + score > DBManager.highScore)
        {
            DBManager.highScore = DBManager.currentScore + score;
        }
        
        WWWForm form = new WWWForm();
        form.AddField("username", DBManager.username);
        form.AddField("highscore", DBManager.highScore);
        form.AddField("citynumber", currentCity + 1);
        form.AddField("currentscore", score);

        using (UnityWebRequest request = UnityWebRequest.Post(DBManager.hostname + "/saveplayerprogress.php", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Successfully saved progress!");
            }
            else
            {
                Debug.Log("Error updating score: " + request.error);
            }
        }
    }*/
}