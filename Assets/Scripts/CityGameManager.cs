using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CityGameManager : MonoBehaviour
{
    [Header("City Configuration")]
    public int cityNumber = 1; // 1 = least sustainable, 5 = most

    [Header("Game Objects")]
    public GameObject maze;
    public GameObject treasureBox;
    public GameObject artifactCollectionPanel; // For the progress message

    [Header("UI Elements")]
    public Text scoreText;
    public Text artifactsText;

    [Header("Game Settings")]
    public int scorePerQuestion = 100;      // Kept for future use
    public int artifactReward = 1;           // Kept for future use

    private int playerScore = 0;
    private int artifactsCollected = 0;
    private bool hasCompleted = false;

    void Start()
    {
        // Load which city the player is on from saved data
        cityNumber = PlayerPrefs.GetInt("CurrentCity", 1);

        LoadPlayerProgress();
        UpdateUI();
    }

    // Called by TreasureBox script when player touches the box
    public void OnTreasureBoxFound()
    {
        if (!hasCompleted)
        {
            // TEMPORARY: Just show a message and complete the city
            ShowProgressMessage("You found the treasure! Moving to next city...");
            Invoke("CompleteCity", 2f); // Wait 2 seconds, then complete
        }
    }

    // Shows a temporary message (will be replaced by question panel later)
    void ShowProgressMessage(string message)
    {
        if (artifactCollectionPanel != null)
        {
            artifactCollectionPanel.SetActive(true);
            Text artifactText = artifactCollectionPanel.GetComponentInChildren<Text>();
            if (artifactText != null)
            {
                artifactText.text = message;
            }
        }
    }

    void CompleteCity()
    {
        hasCompleted = true;

        // Award points (temporary – will be tied to questions later)
        playerScore += scorePerQuestion;
        artifactsCollected += artifactReward;

        // Save total progress
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        PlayerPrefs.SetInt("TotalScore", totalScore + playerScore);

        int totalArtifacts = PlayerPrefs.GetInt("ArtifactsCollected", 0);
        PlayerPrefs.SetInt("ArtifactsCollected", totalArtifacts + artifactsCollected);

        // Save per-city progress
        PlayerPrefs.SetInt($"City{cityNumber}Score", playerScore);
        PlayerPrefs.SetInt($"City{cityNumber}Completed", 1);
        PlayerPrefs.SetInt($"City{cityNumber}Artifacts", artifactsCollected);

        // Unlock next city (if not the last)
        if (cityNumber < 5)
        {
            PlayerPrefs.SetInt("CurrentCity", cityNumber + 1);
            PlayerPrefs.SetInt("LastCompletedCity", cityNumber);
            PlayerPrefs.SetInt("ShowSustainabilityMessage", cityNumber + 1);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentCity", 5);
        }

        PlayerPrefs.Save();

        // Hide the message panel
        if (artifactCollectionPanel != null)
            artifactCollectionPanel.SetActive(false);

        // Return to main menu
        SceneManager.LoadScene("CitySelection");
    }

    void LoadPlayerProgress()
    {
        playerScore = PlayerPrefs.GetInt($"City{cityNumber}Score", 0);
        artifactsCollected = PlayerPrefs.GetInt($"City{cityNumber}Artifacts", 0);

        bool alreadyCompleted = PlayerPrefs.GetInt($"City{cityNumber}Completed", 0) == 1;
        if (alreadyCompleted)
        {
            hasCompleted = true;
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {playerScore}";
        if (artifactsText != null)
            artifactsText.text = $"Artifacts: {artifactsCollected}";
    }
}