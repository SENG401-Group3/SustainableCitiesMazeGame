using UnityEngine;

public class HintScrollPickup : MonoBehaviour
{
    private PickupAnimation pickupAnimation;

    [Header("Hint Settings")]
    [SerializeField] private string hintMessage = "Try looking for hidden paths!";

    void Start()
    {
        pickupAnimation = GetComponent<PickupAnimation>();
        if (pickupAnimation == null)
            Debug.LogError("❌ HintScrollPickup: PickupAnimation component missing!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("📜 Hint scroll collected!");

            if (pickupAnimation != null)
                pickupAnimation.OnPickup();

            // Add your hint logic here
            Debug.Log($"💡 Hint: {hintMessage}");

            // If you have a message handler
            // messageHandler.addMessage(hintMessage);
        }
    }
}