using UnityEngine;

public class Pitfall : Interactable
{
    public override void handleCollision(Collider2D other)
    {
      // play an animation that describes a sustainability pitfall
      // then return to the menu without updating anything
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
