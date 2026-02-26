using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons; // 4 buttons
    public TextMeshProUGUI feedbackText;

    [Header("Question Data (To be filled by teammate)")]
    public string question;
    public string[] answers; // 4 possible answers
    public int correctAnswerIndex;
    public string progressMessage; // e.g., "Moving to next sustainable city"

    private int currentCity;

    void Start()
    {
        // Load which city the player is on (for future use)
        currentCity = PlayerPrefs.GetInt("CurrentCity", 1);

        // Display the question (data will be set in Inspector by teammate)
        if (questionText != null)
            questionText.text = question;

        // Set up answer buttons
        SetupAnswerButtons();
    }

    void SetupAnswerButtons()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < answers.Length && !string.IsNullOrEmpty(answers[i]))
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[i];

                int answerIndex = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(answerIndex));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void CheckAnswer(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;

            // Show progress message (will be set in Inspector)
            Debug.Log(progressMessage);

            // Return to maze after delay
            Invoke("ReturnToMaze", 2f);
        }
        else
        {
            feedbackText.text = "Try again!";
            feedbackText.color = Color.red;
        }
    }

    void ReturnToMaze()
    {
        SceneManager.LoadScene("0"); // Load maze scene
    }
}