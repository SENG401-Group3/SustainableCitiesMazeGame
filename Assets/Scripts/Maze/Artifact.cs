using UnityEngine;

public class Artifact : Interactable
{
    private Animator animator;
    private bool isCollected = false;

    public override void handleCollision(Collider2D other)
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

            Debug.Log("Artifact collected! Show question screen next.");
            // load question scene
            // TODO: find proper scene for the questions. Currently boots back to tutorial
            // not sure how to find the correct number
            UnityEngine.SceneManagement.SceneManager.LoadScene("QuestionScene");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Animator component automatically added by Unity
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
