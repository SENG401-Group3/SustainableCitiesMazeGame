using UnityEngine;
using UnityEngine.UI;

public class CityGameManager : MonoBehaviour
{
    public Text scoreText, artifactsText;
    private int sessionScore = 0;
    private int artifactsCollected = 0;

    public void AddScoreAndArtifacts(int points)
    {
        sessionScore += points;
        artifactsCollected += 1;
        UpdateUI();
    }

    // GETTERS FOR CITYUPDATER
    public int GetPlayerScore() => sessionScore;
    public int GetArtifactsCollected() => artifactsCollected;

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {sessionScore}";
        if (artifactsText != null) artifactsText.text = $"Artifacts: {artifactsCollected}";
    }
}