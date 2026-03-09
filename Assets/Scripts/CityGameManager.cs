using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CityGameManager : MonoBehaviour
{
    [Header("City Configuration")]
    public int cityNumber = 1; // 1 = least sustainable, 5 = most

    [Header("UI Elements")]
    public Text scoreText;
    public Text artifactsText;

    [Header("Game Settings")]
    public int scorePerQuestion = 100;
    public int artifactReward = 1;

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

    // Called by ArtifactPickup.cs when player touches the treasure box
    public void OnTreasureBoxFound()
    {
        if (!hasCompleted)
        {
            // Load the question scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("QuestionScene");
        }
    }

    // Called by QuestionManager.cs after all questions are answered correctly
    public void AddScoreAndArtifacts()
    {
        Debug.Log("AddScoreAndArtifacts called");
        playerScore += scorePerQuestion;
        artifactsCollected += artifactReward;
        UpdateUI();

        StartCoroutine(UpdateScore(scorePerQuestion));
    }

    private IEnumerator UpdateScore(int score)
    {
        Debug.Log("Sending score update...");

        WWWForm form = new WWWForm();
        form.AddField("username", DBManager.username);
        form.AddField("score", score);

        using (UnityWebRequest www = UnityWebRequest.Post("https://yourserver.com/updatescore.php", form))
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

    // Public getters for other scripts
    public int GetCurrentCity() { return cityNumber; }
    public int GetPlayerScore() { return playerScore; }
    public int GetArtifactsCollected() { return artifactsCollected; }
}