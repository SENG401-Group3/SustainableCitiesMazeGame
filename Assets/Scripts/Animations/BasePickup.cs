using UnityEngine;

/*
 * Base class for all pickup items.
 * Handles common pickup behavior including animation.
 */

public abstract class BasePickup : MonoBehaviour
{
    protected PickupAnimation pickupAnimation;

    protected virtual void Start()
    {
        pickupAnimation = GetComponent<PickupAnimation>();
        if (pickupAnimation == null)
            Debug.LogError($"❌ {GetType().Name}: PickupAnimation component missing on {gameObject.name}!");
        else
            Debug.Log($"✅ {GetType().Name}: PickupAnimation found on {gameObject.name}");
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"💥 COLLISION DETECTED on {gameObject.name} with {other.name}");

            if (pickupAnimation != null)
            {
                Debug.Log($"📞 Calling OnPickup() on {gameObject.name}");
                pickupAnimation.OnPickup();
            }
            else
            {
                Debug.LogError($"❌ No PickupAnimation component on {gameObject.name}!");
                return;
            }

            // Call the specific pickup behavior
            OnPickupCollected(other);
        }
    }

    // Abstract method that each specific pickup must implement
    protected abstract void OnPickupCollected(Collider2D player);
}