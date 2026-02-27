using UnityEngine;

public class Pitfall : Interactable
{
    public override void handleCollision(Collider2D other)
    {
      // play an animation and take us to a scene tha describes a sustainability pitfall
      // then take use to the update player without incrementing the maze ar changing the sus score
      return;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
