using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SimplePlayerAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    [Header("Sprites")]
    public Sprite idleSprite;     // Row 2, Column 1 (frame 3) - kneeling/ducked
    public Sprite walkSprite;      // Row 1, Column 2 (frame 1) - legs apart

    [Header("Settings")]
    public float idleThreshold = 0.1f;

    private Vector2 lastMovementDirection;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();

        // Check if sprites are assigned
        if (idleSprite == null) Debug.LogError("❌ idleSprite not assigned in Inspector!");
        if (walkSprite == null) Debug.LogError("❌ walkSprite not assigned in Inspector!");

        // Set default sprite
        if (idleSprite != null)
            spriteRenderer.sprite = idleSprite;

        lastMovementDirection = Vector2.right; // Default facing right
    }

    void Update()
    {
        if (playerController == null || spriteRenderer == null) return;

        // Get velocity from PlayerController
        Vector2 currentVelocity = playerController.GetCurrentVelocity();
        bool isMoving = currentVelocity.magnitude > idleThreshold;

        if (isMoving)
        {
            // Store movement direction (if actually moving)
            if (currentVelocity.magnitude > 0.01f)
            {
                lastMovementDirection = currentVelocity.normalized;
            }

            // Show walk sprite
            if (walkSprite != null)
                spriteRenderer.sprite = walkSprite;

            // Flip based on horizontal direction
            if (lastMovementDirection.x < -0.1f) // Moving LEFT
            {
                spriteRenderer.flipX = true;
            }
            else if (lastMovementDirection.x > 0.1f) // Moving RIGHT
            {
                spriteRenderer.flipX = false;
            }
            // For up/down, keep current flip state
        }
        else
        {
            // Show idle sprite
            if (idleSprite != null)
                spriteRenderer.sprite = idleSprite;

            // Keep the last direction's flip for idle
            if (lastMovementDirection.x < -0.1f)
            {
                spriteRenderer.flipX = true;
            }
            else if (lastMovementDirection.x > 0.1f)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}