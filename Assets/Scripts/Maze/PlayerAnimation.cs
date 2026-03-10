using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    private Vector2 lastDirection = Vector2.down;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        if (animator == null)
            Debug.LogError("❌ Animator not found on player!");
        if (playerController == null)
            Debug.LogError("❌ PlayerController not found on player!");

        Debug.Log("✅ PlayerAnimation started");
    }

    void Update()
    {
        if (animator == null || playerController == null) return;

        Vector2 velocity = playerController.GetCurrentVelocity();
        bool isMoving = velocity.magnitude > 0.1f;

        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            lastDirection = velocity.normalized;
        }

        animator.SetFloat("MoveX", lastDirection.x);
        animator.SetFloat("MoveY", lastDirection.y);
    }
}