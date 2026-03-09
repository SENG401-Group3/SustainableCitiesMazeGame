using UnityEngine;
using UnityEngine.SceneManagement;

public class CityUpdater : MonoBehaviour
{
    private int currentCity;

    // Add this for singleton pattern
    public static CityUpdater Instance;

    void Awake()
    {
        // Singleton pattern to keep only one instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This keeps it alive when loading new scenes
            Debug.Log("✅ CityUpdater set to persist between scenes");
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate if one already exists
            Debug.Log("❌ Duplicate CityUpdater destroyed");
        }
    }

    void OnEnable()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update currentCity whenever a new scene loads
        RefreshCurrentCity();
        Debug.Log($"Scene loaded: {scene.name}. Current city updated to: {currentCity}");
    }

    void Start()
    {
        RefreshCurrentCity();
    }

    void RefreshCurrentCity()
    {
        currentCity = PlayerPrefs.GetInt("CurrentCity", 1);
        Debug.Log($"CityUpdater refreshed. Current city: {currentCity}");
    }

    public void CompleteCity()
    {
        // Get the latest city value from PlayerPrefs
        RefreshCurrentCity();

        CityGameManager gameManager = FindFirstObjectByType<CityGameManager>();

        int playerScore = gameManager != null ? gameManager.GetPlayerScore() : 0;
        int artifactsCollected = gameManager != null ? gameManager.GetArtifactsCollected() : 0;

        Debug.Log($"=== Completing City {currentCity} ===");
        Debug.Log($"PlayerScore from this city: {playerScore}");
        Debug.Log($"Artifacts: {artifactsCollected}");

        // Save total progress
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        int newTotalScore = totalScore + playerScore;
        PlayerPrefs.SetInt("TotalScore", newTotalScore);

        Debug.Log($"Previous TotalScore: {totalScore}");
        Debug.Log($"New TotalScore: {newTotalScore}");

        int totalArtifacts = PlayerPrefs.GetInt("ArtifactsCollected", 0);
        PlayerPrefs.SetInt("ArtifactsCollected", totalArtifacts + artifactsCollected);

        // Save per-city progress
        PlayerPrefs.SetInt($"City{currentCity}Score", playerScore);
        PlayerPrefs.SetInt($"City{currentCity}Completed", 1);
        PlayerPrefs.SetInt($"City{currentCity}Artifacts", artifactsCollected);

        // Clear temporary progress for this city
        if (gameManager != null)
        {
            gameManager.ClearTempProgress();
        }

        // Handle city progression
        if (currentCity < 5)
        {
            // Move to next city
            int nextCity = currentCity + 1;
            PlayerPrefs.SetInt("CurrentCity", nextCity);
            PlayerPrefs.SetInt("LastCompletedCity", currentCity);
            PlayerPrefs.SetInt("ShowSustainabilityMessage", nextCity);

            Debug.Log($"✅ City {currentCity} complete! Moving to City {nextCity}");

            PlayerPrefs.Save();

            // IMPORTANT: Force refresh after saving
            RefreshCurrentCity();

            SceneManager.LoadScene("CitySelection");
        }
        else
        {
            // CITY 5 COMPLETED - GAME OVER
            Debug.Log("🎉 GAME COMPLETE! City 5 finished.");

            // Set GameComplete flag (keep CurrentCity as 5 for now)
            PlayerPrefs.SetInt("GameComplete", 1);

            // Save the final total
            PlayerPrefs.Save();

            // Go to leaderboard scene directly
            SceneManager.LoadScene("LeaderboardScene");
        }
    }
}