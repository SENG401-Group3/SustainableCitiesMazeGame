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
    private Button menuButton;
    private Button editButton;
    private Button logoutButton;
    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement; // fetching the root element
    }

    public void OnEnable()
    {
        // fetching the labels
        namesLabel = root.Q<Label>("NamesContainer");
        scoreLabel = root.Q<Label>("ScoreContainer");
        greetingLabel = root.Q<Label>("GreetingLabel");

        // fetching the buttons for input and adding events
        menuButton = root.Q<Button>("MenuButton");
        menuButton.clicked += OnMenuClicked;

        editButton = root.Q<Button>("EditButton");
        editButton.clicked += OnEditClicked;

        logoutButton = root.Q<Button>("LogoutButton");
        logoutButton.clicked += OnLogoutClicked;
    }

    public void OnMenuClicked()
    {
        gameUIManager.ShowSelection(); //load the game scene on play button
    }

    public void OnEditClicked()
    {
        gameUIManager.ShowEditProfile();
    }

    public void OnLogoutClicked()
    {
        // clear current user data
        DBManager.firstname = null;
        DBManager.lastname = null;
        DBManager.username = null;
        DBManager.score = 0;
        SceneManager.LoadScene("UI");
    }

    // public method being called on login. I made it this way because of issues with username being fetched before the panel is rendered
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

                greetingLabel.text = "Welcome " + data.firstname + " " + data.lastname;

                namesLabel.text = "Full Name: " + data.firstname + " " + data.lastname + "\n"
                + "Username: " + data.username;
                scoreLabel.text = "Score: " + data.score;
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

    // Update is called once per frame
    void Update()
    {

    }
}
