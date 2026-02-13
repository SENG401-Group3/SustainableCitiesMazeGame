using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    private Animator animator;
    private bool isCollected = false;

    void Start()
    {
        // Get the Animator component automatically added by Unity
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player touched the artifact AND it's not already collected
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Play the pickup animation
            if (animator != null)
            {
                // Play animation directly by name - NO TRIGGER NEEDED!
                animator.Play("ArtifactPickup", 0, 0f);
            }

            // Disable the collider so player can't collect it again
            GetComponent<Collider2D>().enabled = false;

            // Destroy the artifact after 1 second (after animation finishes)
            Destroy(gameObject, 1f);

            // TODO: Later we'll add code to show question screen here
            Debug.Log("Artifact collected! Show question screen next.");
        }
    }
}