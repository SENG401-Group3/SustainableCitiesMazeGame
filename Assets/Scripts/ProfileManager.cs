using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] private GameUIManager gameUIManager;
    private VisualElement root;
    private Label greetingLabel;
    private Label namesLabel;
    private Label scoreLabel;
    private Button playButton;
    private Button editButton;
    private Button logoutButton;

    private void Awake()
    {
        // Check for UIDocument component
        var uiDoc = GetComponent<UIDocument>();
        if (uiDoc == null)
        {
            Debug.LogError("❌ ProfileManager: No UIDocument component found on this GameObject!");
            return;
        }

        root = uiDoc.rootVisualElement;

        if (root == null)
            Debug.LogError("❌ ProfileManager: rootVisualElement is null!");
    }

    public void OnEnable()
    {
        if (root == null) return;

        // Fetch the labels with null checks
        namesLabel = root.Q<Label>("NamesContainer");
        if (namesLabel == null)
            Debug.LogError("❌ ProfileManager: Could not find 'NamesContainer' in UXML!");

        scoreLabel = root.Q<Label>("ScoreContainer");
        if (scoreLabel == null)
            Debug.LogError("❌ ProfileManager: Could not find 'ScoreContainer' in UXML!");

        greetingLabel = root.Q<Label>("GreetingLabel");
        if (greetingLabel == null)
            Debug.LogError("❌ ProfileManager: Could not find 'GreetingLabel' in UXML!");

        // Fetch buttons with null checks
        playButton = root.Q<Button>("PlayButton");
        if (playButton != null)
            playButton.clicked += OnPlayClicked;
        else
            Debug.LogError("❌ ProfileManager: Could not find 'PlayButton' in UXML!");

        editButton = root.Q<Button>("EditButton");
        if (editButton != null)
            editButton.clicked += OnEditClicked;
        else
            Debug.LogError("❌ ProfileManager: Could not find 'EditButton' in UXML!");

        logoutButton = root.Q<Button>("LogoutButton");
        if (logoutButton != null)
            logoutButton.clicked += OnLogoutClicked;
        else
            Debug.LogError("❌ ProfileManager: Could not find 'LogoutButton' in UXML!");
    }

    private void OnDisable()
    {
        // Safely unsubscribe from events
        if (playButton != null) playButton.clicked -= OnPlayClicked;
        if (editButton != null) editButton.clicked -= OnEditClicked;
        if (logoutButton != null) logoutButton.clicked -= OnLogoutClicked;
    }

    private void Start()
    {
        // Find GameUIManager if not assigned
        if (gameUIManager == null)
        {
            gameUIManager = FindFirstObjectByType<GameUIManager>();
            if (gameUIManager == null)
                Debug.LogError("❌ ProfileManager: GameUIManager not found in scene!");
        }
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene("MazeScene"); //load the game scene on play button
    }

    public void OnEditClicked()
    {
        Debug.Log("Edit button clicked - implement edit functionality here");
    }

    public void OnLogoutClicked()
    {
        // clear current user data
        DBManager.firstname = null;
        DBManager.lastname = null;
        DBManager.username = null;
        DBManager.score = 0;

        if (gameUIManager != null)
            gameUIManager.ShowSelection(); // Return to main menu
        else
            Debug.LogError("❌ Cannot logout: gameUIManager is null!");
    }

    public void LoadProfile()
    {
        Debug.Log("Loading profile for: " + DBManager.username);

        // ensures that the user is not null so it can fetch their data from the db
        if (!string.IsNullOrEmpty(DBManager.username))
        {
            StartCoroutine(GetUserData());
        }
        else
        {
            Debug.LogError("Username is NULL when trying to load profile!");
        }
    }

    IEnumerator GetUserData()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", DBManager.username);

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/SQLConnect/getuser.php", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                UserData data = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
                Debug.Log("Server response: " + request.downloadHandler.text);

                DBManager.firstname = data.firstname;
                DBManager.lastname = data.lastname;
                DBManager.username = data.username;
                DBManager.score = data.score;

                if (greetingLabel != null)
                    greetingLabel.text = "Welcome " + data.firstname + " " + data.lastname;
                else
                    Debug.LogError("❌ greetingLabel is null when trying to update text!");

                if (namesLabel != null)
                    namesLabel.text = "Full Name: " + data.firstname + " " + data.lastname + "\n"
                        + "Username: " + data.username;
                else
                    Debug.LogError("❌ namesLabel is null when trying to update text!");

                if (scoreLabel != null)
                    scoreLabel.text = "Score: " + data.score;
                else
                    Debug.LogError("❌ scoreLabel is null when trying to update text!");
            }
            else
            {
                Debug.LogError("Failed to get user data: " + request.error);
            }
        }
    }

    // helper class for parsing the returned json object from the getUser request
    [System.Serializable]
    public class UserData
    {
        public string username;
        public string firstname;
        public string lastname;
        public int score;
    }
}