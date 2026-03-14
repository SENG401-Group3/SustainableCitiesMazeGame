using UnityEngine;
using UnityEngine.UIElements;

public class TutorialController : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var backButton = root.Q<Button>("BackButton");

        if (backButton != null)
        {
            Debug.Log("✅ Back button found!");
            backButton.clicked += () => {
                Debug.Log("✅ Back button clicked!");
                var gameUIManager = FindFirstObjectByType<GameUIManager>();
                if (gameUIManager != null)
                {
                    gameUIManager.HideTutorial();
                }
            };
        }
        else
        {
            Debug.LogError("❌ Back button NOT found!");
        }
    }
}