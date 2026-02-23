using UnityEngine;
using UnityEngine.SceneManagement;

public class CityUpdater : MonoBehaviour
{
    private int currentCity;

    void Start()
    {
        currentCity = PlayerPrefs.GetInt("CurrentCity", 1);
    }

    public void CompleteCity()
    {
        // Use FindFirstObjectByType instead of FindObjectOfType
        CityGameManager gameManager = FindFirstObjectByType<CityGameManager>();

        int playerScore = gameManager != null ? gameManager.GetPlayerScore() : 0;
        int artifactsCollected = gameManager != null ? gameManager.GetArtifactsCollected() : 0;

        // Save total progress
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        PlayerPrefs.SetInt("TotalScore", totalScore + playerScore);

        int totalArtifacts = PlayerPrefs.GetInt("ArtifactsCollected", 0);
        PlayerPrefs.SetInt("ArtifactsCollected", totalArtifacts + artifactsCollected);

        // Save per-city progress
        PlayerPrefs.SetInt($"City{currentCity}Score", playerScore);
        PlayerPrefs.SetInt($"City{currentCity}Completed", 1);
        PlayerPrefs.SetInt($"City{currentCity}Artifacts", artifactsCollected);

        // Unlock next city (if not the last)
        if (currentCity < 5)
        {
            PlayerPrefs.SetInt("CurrentCity", currentCity + 1);
            PlayerPrefs.SetInt("LastCompletedCity", currentCity);
            PlayerPrefs.SetInt("ShowSustainabilityMessage", currentCity + 1);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentCity", 5);
        }

        PlayerPrefs.Save();

        // Return to main menu
        SceneManager.LoadScene("CitySelection");
    }
}