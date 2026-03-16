using UnityEngine;
using System.Collections;

public class PickupAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float floatHeight = 0.5f;
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float scalePulseAmount = 0.2f;
    [SerializeField] private float scalePulseSpeed = 3f;

    [Header("Pickup Effect")]
    [SerializeField] private GameObject pickupEffectPrefab;
    [SerializeField] private AudioClip pickupSound;
    // [SerializeField] private float destroyDelay = 0.5f;  // REMOVED - unused

    private Vector3 startPosition;
    private float floatOffset;
    private bool isPickedUp = false;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    void Start()
    {
        Debug.Log($"✅ PickupAnimation START on {gameObject.name}");

        // Store original position and scale
        startPosition = transform.position;
        originalScale = transform.localScale;
        floatOffset = Random.Range(0f, Mathf.PI * 2f);

        // Get sprite renderer for fade effects
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start floating animation
        StartCoroutine(FloatAnimation());
    }

    IEnumerator FloatAnimation()
    {
        Debug.Log($"🔄 FloatAnimation STARTED on {gameObject.name}");

        while (!isPickedUp)
        {
            // Floating up and down
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed + floatOffset) * floatHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // Slow rotation
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Scale pulse (using original scale as base)
            float pulse = 1f + Mathf.Sin(Time.time * scalePulseSpeed) * scalePulseAmount;
            transform.localScale = originalScale * pulse;

            yield return null;
        }

        Debug.Log($"⏹️ FloatAnimation STOPPED on {gameObject.name}");
    }

    public void OnPickup()
    {
        Debug.Log($"🎯 OnPickup CALLED on {gameObject.name}");

        if (isPickedUp)
        {
            Debug.Log($"⚠️ Already picked up, ignoring");
            return;
        }

        isPickedUp = true;
        Debug.Log($"✅ isPickedUp set to TRUE");

        // Stop floating animation
        StopAllCoroutines();
        Debug.Log($"🛑 All coroutines stopped");

        // Instantiate particle effect if assigned
        if (pickupEffectPrefab != null)
        {
            Debug.Log($"✨ Instantiating effect: {pickupEffectPrefab.name}");
            Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log($"⚠️ No pickupEffectPrefab assigned");
        }

        // Play sound if assigned
        if (pickupSound != null)
        {
            Debug.Log($"🔊 Playing sound");
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
        else
        {
            Debug.Log($"⚠️ No pickupSound assigned");
        }

        Debug.Log($"🎬 Starting PickupEffectAnimation");
        StartCoroutine(PickupEffectAnimation());
    }

    IEnumerator PickupEffectAnimation()
    {
        float elapsed = 0f;
        float duration = 1.0f;  // Longer for visibility
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(5f, 5f, 1f);  // Force big size
        float startRotation = transform.eulerAngles.z;

        Debug.Log($"📊 Duration: {duration}, StartScale: {startScale}, TargetScale: {targetScale}, HasRenderer: {spriteRenderer != null}");

        // Flash yellow at start if we have a renderer
        Color originalColor = Color.white;
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.yellow;
            Debug.Log($"💛 Flashing yellow");
        }

        // Main animation loop
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Scale up dramatically
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            // Spin multiple times
            float spin = Mathf.Lerp(0f, 720f, t);
            transform.rotation = Quaternion.Euler(0, 0, startRotation + spin);

            // Fade out
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 1f - t;
                spriteRenderer.color = color;
            }

            yield return null;
        }

        Debug.Log($"🏁 PickupEffectAnimation COMPLETE, destroying {gameObject.name}");
        Destroy(gameObject);
    }
}