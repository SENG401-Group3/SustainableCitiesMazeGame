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
        var uiDoc = GetComponent<UIDocument>();
        if (uiDoc == null)
        {
            Debug.LogError("❌ TutorialController: No UIDocument component found!");
            return;
        }

        root = uiDoc.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("❌ TutorialController: root VisualElement is null!");
        }
    }

    private void OnEnable()
    {
        if (root == null)
        {
            Debug.LogError("❌ TutorialController: root is null in OnEnable!");
            return;
        }

        // Auto-find GameUIManager if not assigned
        if (gameUIManager == null)
        {
            gameUIManager = FindFirstObjectByType<GameUIManager>();
            Debug.Log($"🔍 TutorialController: GameUIManager found: {gameUIManager != null}");
        }

        // Find the back button
        backButton = root.Q<Button>("BackButton");
        if (backButton != null)
        {
            backButton.clicked += OnBackClicked;
            Debug.Log("✅ TutorialController: BackButton connected");
        }
        else
        {
            Debug.LogError("❌ TutorialController: Could not find 'BackButton' in UXML!");
        }
    }

    private void OnDisable()
    {
        if (backButton != null)
        {
            backButton.clicked -= OnBackClicked;
        }
    }

    private void OnBackClicked()
    {
        Debug.Log("🔙 TutorialController: Back button clicked!");

        if (gameUIManager != null)
        {
            Debug.Log("✅ Calling gameUIManager.ShowSelection()");
            gameUIManager.ShowSelection();
        }
        else
        {
            Debug.LogError("❌ TutorialController: gameUIManager is null!");
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}