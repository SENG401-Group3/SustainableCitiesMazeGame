using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class CityGameManager : MonoBehaviour
{
    [Header("City Configuration")]
    public int cityNumber = 1;
    public string cityName = "Least Sustainable City";

    [Header("Game Objects")]
    public GameObject maze;
    public GameObject treasureBox;
    public GameObject questionPanel;
    public GameObject artifactCollectionPanel;
    public GameObject completePanel;

    [Header("UI Elements")]
    public Text questionText;
    public Button[] answerButtons;
    public Text scoreText;
    public Text artifactsText;
    public Text timeText;
    public Text feedbackText;

    [Header("Game Settings")]
    public int scorePerQuestion = 100;
    public int artifactReward = 1;
    public float timeLimit = 60f;

    [Header("City Questions")]
    public QuestionData[] cityQuestions;

    [System.Serializable]
    public class QuestionData
    {
        public string question;
        public string[] answers;
        public int correctAnswerIndex;
        public string sustainabilityFact;
    }

    private int playerScore = 0;
    private int artifactsCollected = 0;
    private float currentTime;
    private bool isGameActive = true;
    private bool hasCompleted = false;
    private int currentQuestionIndex = 0;

    void Start()
    {
        currentTime = timeLimit;
        LoadDefaultQuestions();
        LoadPlayerProgress();
        UpdateUI();
    }

    void Update()
    {
        if (isGameActive && !hasCompleted)
        {
            currentTime -= Time.deltaTime;
            if (timeText != null)
                timeText.text = $"Time: {Mathf.Max(0, currentTime):F0}s";

            if (currentTime <= 0)
            {
                GameOver("Time's Up!");
            }
        }
    }

    void LoadDefaultQuestions()
    {
        if (cityQuestions == null || cityQuestions.Length == 0)
        {
            switch (cityNumber)
            {
                case 1:
                    cityQuestions = new QuestionData[]
                    {
                        new QuestionData
                        {
                            question = "What is the biggest environmental problem in polluted cities?",
                            answers = new string[] { "Too many trees", "Air pollution", "Too much rain", "Lots of birds" },
                            correctAnswerIndex = 1,
                            sustainabilityFact = "Air pollution causes 7 million premature deaths every year!"
                        },
                        new QuestionData
                        {
                            question = "How long does plastic take to decompose?",
                            answers = new string[] { "1 year", "10 years", "100 years", "450+ years" },
                            correctAnswerIndex = 3,
                            sustainabilityFact = "Plastic can take up to 1000 years to decompose in landfills!"
                        }
                    };
                    break;
                case 2:
                    cityQuestions = new QuestionData[]
                    {
                        new QuestionData
                        {
                            question = "Which material is most commonly recycled?",
                            answers = new string[] { "Aluminum", "Paper", "Glass", "Plastic" },
                            correctAnswerIndex = 1,
                            sustainabilityFact = "Recycling one aluminum can saves enough energy to power a TV for 3 hours!"
                        }
                    };
                    break;
                case 3:
                    cityQuestions = new QuestionData[]
                    {
                        new QuestionData
                        {
                            question = "Which is a renewable energy source?",
                            answers = new string[] { "Coal", "Natural gas", "Solar power", "Nuclear" },
                            correctAnswerIndex = 2,
                            sustainabilityFact = "Solar energy is the most abundant energy source on Earth!"
                        }
                    };
                    break;
                case 4:
                    cityQuestions = new QuestionData[]
                    {
                        new QuestionData
                        {
                            question = "What is a green building feature?",
                            answers = new string[] { "More windows", "Solar panels", "Taller structure", "Parking lot" },
                            correctAnswerIndex = 1,
                            sustainabilityFact = "Green buildings can reduce energy use by 30-50%!"
                        }
                    };
                    break;
                case 5:
                    cityQuestions = new QuestionData[]
                    {
                        new QuestionData
                        {
                            question = "What makes a city carbon neutral?",
                            answers = new string[] { "No cars", "Balancing emissions", "More trees", "Less people" },
                            correctAnswerIndex = 1,
                            sustainabilityFact = "Carbon neutral means removing as much CO2 as you emit!"
                        }
                    };
                    break;
            }
        }
    }

    public void OnTreasureBoxFound()
    {
        if (!hasCompleted)
        {
            ShowQuestion();
        }
    }

    void ShowQuestion()
    {
        Time.timeScale = 0;

        if (questionPanel != null)
        {
            questionPanel.SetActive(true);

            if (currentQuestionIndex < cityQuestions.Length)
            {
                QuestionData q = cityQuestions[currentQuestionIndex];
                questionText.text = q.question;

                for (int i = 0; i < answerButtons.Length; i++)
                {
                    if (i < q.answers.Length)
                    {
                        answerButtons[i].gameObject.SetActive(true);
                        answerButtons[i].GetComponentInChildren<Text>().text = q.answers[i];

                        int answerIndex = i;
                        answerButtons[i].onClick.RemoveAllListeners();
                        answerButtons[i].onClick.AddListener(() => CheckAnswer(answerIndex, q.correctAnswerIndex, q.sustainabilityFact));
                    }
                    else
                    {
                        answerButtons[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    void CheckAnswer(int selectedIndex, int correctIndex, string fact)
    {
        if (selectedIndex == correctIndex)
        {
            playerScore += scorePerQuestion;
            artifactsCollected += artifactReward;

            feedbackText.text = $"Correct! +{scorePerQuestion} points!";
            feedbackText.color = Color.green;

            ShowSustainabilityFact(fact);

            currentQuestionIndex++;

            if (currentQuestionIndex >= cityQuestions.Length)
            {
                CompleteCity();
            }
            else
            {
                Invoke("HideQuestionPanel", 2f);
            }
        }
        else
        {
            feedbackText.text = "Try again!";
            feedbackText.color = Color.red;
            feedbackText.text += $"\nHint: {fact}";
        }

        UpdateUI();
    }

    void ShowSustainabilityFact(string fact)
    {
        if (artifactCollectionPanel != null)
        {
            artifactCollectionPanel.SetActive(true);
            Text artifactText = artifactCollectionPanel.GetComponentInChildren<Text>();
            if (artifactText != null)
            {
                artifactText.text = $"Artifact Collected!\n\n{fact}";
            }

            Invoke("HideArtifactPanel", 3f);
        }
    }

    void HideArtifactPanel()
    {
        if (artifactCollectionPanel != null)
            artifactCollectionPanel.SetActive(false);
    }

    void HideQuestionPanel()
    {
        if (questionPanel != null)
            questionPanel.SetActive(false);
        Time.timeScale = 1;
    }

    void CompleteCity()
    {
        hasCompleted = true;

        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        PlayerPrefs.SetInt("TotalScore", totalScore + playerScore);

        int totalArtifacts = PlayerPrefs.GetInt("ArtifactsCollected", 0);
        PlayerPrefs.SetInt("ArtifactsCollected", totalArtifacts + artifactsCollected);

        PlayerPrefs.SetInt($"City{cityNumber}Score", playerScore);
        PlayerPrefs.SetInt($"City{cityNumber}Completed", 1);
        PlayerPrefs.SetInt($"City{cityNumber}Artifacts", artifactsCollected);

        if (cityNumber < 5)
        {
            PlayerPrefs.SetInt("LastCompletedCity", cityNumber);
        }

        PlayerPrefs.Save();

        if (completePanel != null)
        {
            completePanel.SetActive(true);
            Text completeText = completePanel.GetComponentInChildren<Text>();
            if (completeText != null)
            {
                if (cityNumber < 5)
                {
                    completeText.text = $"CITY COMPLETE!\n\nScore: {playerScore}\nArtifacts: {artifactsCollected}\n\nYou've unlocked the next city!";
                }
                else
                {
                    completeText.text = $"CONGRATULATIONS!\n\nYou completed all 5 cities!\n\nFinal Score: {PlayerPrefs.GetInt("TotalScore", 0)}";
                }
            }
        }

        Time.timeScale = 0;
    }

    public void GoToNextCity()
    {
        Time.timeScale = 1;
        if (cityNumber < 5)
        {
            string nextScene = GetCitySceneName(cityNumber + 1);
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            SceneManager.LoadScene("CitySelection");
        }
    }

    string GetCitySceneName(int cityNumber)
    {
        switch (cityNumber)
        {
            case 1: return "City1_LeastSustainable";
            case 2: return "City2_SomewhatSustainable";
            case 3: return "City3_ModeratelySustainable";
            case 4: return "City4_VerySustainable";
            case 5: return "City5_MostSustainable";
            default: return "City1_LeastSustainable";
        }
    }

    public void RestartCity()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToCitySelection()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("CitySelection");
    }

    void GameOver(string reason)
    {
        isGameActive = false;
        Debug.Log($"Game Over: {reason}");
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