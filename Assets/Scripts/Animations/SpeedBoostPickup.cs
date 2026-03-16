using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    private PickupAnimation pickupAnimation;

    void Start()
    {
        // Get reference to PickupAnimation component
        pickupAnimation = GetComponent<PickupAnimation>();

        if (pickupAnimation == null)
            Debug.LogError($"❌ SpeedBoostPickup: PickupAnimation component missing on {gameObject.name}!");
        else
            Debug.Log($"✅ SpeedBoostPickup: PickupAnimation found on {gameObject.name}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player collided with the item
        if (other.CompareTag("Player"))
        {
            Debug.Log($"💥 COLLISION DETECTED on {gameObject.name} with {other.name}");

            // Verify PickupAnimation component exists
            PickupAnimation anim = GetComponent<PickupAnimation>();
            Debug.Log($"🔍 PickupAnimation component found: {anim != null}");

            // Trigger pickup animation
            if (anim != null)
            {
                Debug.Log($"📞 Calling OnPickup() on {gameObject.name}");
                anim.OnPickup();
            }
            else
            {
                Debug.LogError($"❌ No PickupAnimation component on {gameObject.name}!");
                return;
            }

            // Apply speed boost effect to player
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // The PlayerController's notify system will handle the speed boost
                Debug.Log($"✅ Speed boost effect applied to player");
            }
            else
            {
                Debug.LogError($"❌ PlayerController not found on {other.name}!");
            }
        }
    }
}