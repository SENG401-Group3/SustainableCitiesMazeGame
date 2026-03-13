using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CityGameManager : MonoBehaviour
{
    public Text scoreText, artifactsText;
    private int sessionScore = 0;
    private int artifactsCollected = 0;

    private int currentCity;

    // Add this for singleton pattern to avoid duplicates
    public static CityGameManager Instance;

    void Awake()
    {
        // Singleton pattern to keep only one instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This keeps it alive when loading new scenes
            Debug.Log("✅ CityGameManager set to persist between scenes");
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate if one already exists
            Debug.Log("❌ Duplicate CityGameManager destroyed");
        }
    }

    void Start()
    {
        currentCity = PlayerPrefs.GetInt("CurrentCity", 1);

        // SAFETY CHECK: If we're on City 1 and TotalScore is high, something's wrong
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        if (currentCity == 1 && totalScore > 100)
        {
            Debug.LogWarning($"⚠️ Detected possible score accumulation! TotalScore is {totalScore} but we're on City 1. Resetting...");
            PlayerPrefs.SetInt("TotalScore", 0);
            PlayerPrefs.Save();
            totalScore = 0;
        }

        Debug.Log($"🏁 CityGameManager Started for City {currentCity}. Initial sessionScore: {sessionScore}, TotalScore: {totalScore}");

        // If we're starting a new game (City 1), make sure session is clean
        if (currentCity == 1)
        {
            sessionScore = 0;
            artifactsCollected = 0;
            Debug.Log("🆕 City 1 detected - session score reset to 0");
        }

        // Try to load any existing progress for this city
        LoadCityProgress();
    }

    void LoadCityProgress()
    {
        // Check if we have saved progress for this city
        int savedScore = PlayerPrefs.GetInt($"City{currentCity}TempScore", -1);
        int savedArtifacts = PlayerPrefs.GetInt($"City{currentCity}TempArtifacts", -1);

        if (savedScore > 0)
        {
            sessionScore = savedScore;
            artifactsCollected = savedArtifacts;
            Debug.Log($"📂 Loaded saved progress for City {currentCity}: Score={sessionScore}, Artifacts={artifactsCollected}");
        }

        UpdateUI();
    }

    public void AddScoreAndArtifacts(int points)
    {
        sessionScore += points;
        artifactsCollected += 1;

        Debug.Log($"➕ Added {points} points. City {currentCity} total now: {sessionScore}");

        // Save progress after each addition
        SaveCityProgress();

        UpdateUI();

        // DATABASE CODE COMMENTED OUT - causing null reference errors
        // StartCoroutine(UpdateScore(points));
    }

    /*
    private IEnumerator UpdateScore(int score)
    {
        Debug.Log("Sending score update...");

        WWWForm form = new WWWForm();
        form.AddField("username", DBManager.username);
        form.AddField("points", score);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SQLConnect/updatescore.php", form))
        {
            yield return www.SendWebRequest();

            Debug.Log("Server returned: " + www.downloadHandler.text);

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"❌ Failed to update score: {www.error}");
            }
            else
            {
                Debug.Log($"✅ Score updated successfully on server: {score}");
            }
        }
    }
    */

    void SaveCityProgress()
    {
        // Save temporary progress for this city
        PlayerPrefs.SetInt($"City{currentCity}TempScore", sessionScore);
        PlayerPrefs.SetInt($"City{currentCity}TempArtifacts", artifactsCollected);
        PlayerPrefs.Save();

        Debug.Log($"💾 Saved progress for City {currentCity}: Score={sessionScore}, Artifacts={artifactsCollected}");
    }

    // GETTERS FOR CITYUPDATER
    public int GetPlayerScore()
    {
        Debug.Log($"📤 GetPlayerScore called for City {currentCity}: returning {sessionScore}");
        return sessionScore;
    }

    public int GetArtifactsCollected()
    {
        Debug.Log($"📤 GetArtifactsCollected called: returning {artifactsCollected}");
        return artifactsCollected;
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {sessionScore}";
            Debug.Log($"🖥️ UI Updated: scoreText = {sessionScore}");
        }
        if (artifactsText != null) artifactsText.text = $"Artifacts: {artifactsCollected}";
    }

    // Clear temp progress when city is completed
    public void ClearTempProgress()
    {
        Debug.Log($"🧹 Clearing progress for City {currentCity}. Final score: {sessionScore}");

        sessionScore = 0;
        artifactsCollected = 0;

        PlayerPrefs.DeleteKey($"City{currentCity}TempScore");
        PlayerPrefs.DeleteKey($"City{currentCity}TempArtifacts");
        PlayerPrefs.Save();

        UpdateUI();
    }
}