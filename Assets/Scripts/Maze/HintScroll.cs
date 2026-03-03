using UnityEngine;

public class HintScroll : Interactable
{
    public override void handleCollision(Collider2D other)
    {
      // update player prefs so we can display a hint in the question page
      PlayerPrefs.SetInt("HintScrollCollected", 1);

      // create a text popup that says "hint scroll collected!"

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
