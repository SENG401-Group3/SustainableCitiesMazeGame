using UnityEngine;
using UnityEngine.UIElements;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameUIManager gameUIManager;
    private VisualElement root;
    private Button backButton;

    private void Awake()
    {
        // Fetch the panel as soon as it is initialized
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        // Retrieving and configuring buttons
        backButton = root.Q<Button>("BackButton");
        backButton.clicked += OnBackClicked;
    }

    private void OnBackClicked()
    {
        gameUIManager.ShowSelection();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
