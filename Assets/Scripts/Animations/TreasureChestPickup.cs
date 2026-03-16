using UnityEngine;

public class TreasureChestPickup : MonoBehaviour
{
    private PickupAnimation pickupAnimation;

    void Start()
    {
        pickupAnimation = GetComponent<PickupAnimation>();
        if (pickupAnimation == null)
            Debug.LogError("❌ TreasureChestPickup: PickupAnimation component missing!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎁 Treasure chest collected!");

            if (pickupAnimation != null)
            {
                pickupAnimation.OnPickup();
                // Load question scene after animation completes
                Invoke("LoadQuestionScene", 0.3f);
            }
            else
            {
                LoadQuestionScene();
            }
        }
    }

    void LoadQuestionScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("QuestionScene");
    }
}