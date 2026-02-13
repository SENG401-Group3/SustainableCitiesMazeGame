using UnityEngine;
using UnityEngine.UIElements;

public class AboutController : MonoBehaviour
{
    private VisualElement root;
    [SerializeField] private UIDocument welcomeDoc;
    private VisualElement welcomePanel;
    private Button backButton;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        welcomePanel = welcomeDoc.rootVisualElement;
    }

    private void OnEnable()
    {
        backButton = root.Q<Button>("BackButton");
        backButton.clicked += GoBack;
    }

    private void GoBack()
    {
        root.style.display = DisplayStyle.None;
        welcomePanel.style.display = DisplayStyle.Flex;
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
