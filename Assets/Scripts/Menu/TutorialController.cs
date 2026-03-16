using UnityEngine;
using UnityEngine.UIElements;

/*
 * Controls the tutorial panel UI, specifically handling the back button
 * functionality to return to the main menu.
 */

public class TutorialController : MonoBehaviour
{
    /*
     * Unity's OnEnable method - called when the GameObject becomes enabled and active
     * Sets up the back button click event handler
     */
    private void OnEnable()
    {
        // Get the root VisualElement from the UIDocument component
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find the back button in the UXML hierarchy
        var backButton = root.Q<Button>("BackButton");

        // Check if back button was found
        if (backButton != null)
        {
            Debug.Log("✅ Back button found!");

            // Add click event handler to the back button
            backButton.clicked += () => {
                Debug.Log("✅ Back button clicked!");

                // Find the GameUIManager in the scene
                var gameUIManager = FindFirstObjectByType<GameUIManager>();

                // Hide the tutorial panel if GameUIManager exists
                if (gameUIManager != null)
                {
                    gameUIManager.HideTutorial();
                }
            };
        }
        else
        {
            // Log error if back button couldn't be found in UXML
            Debug.LogError("❌ Back button NOT found!");
        }
    }
}