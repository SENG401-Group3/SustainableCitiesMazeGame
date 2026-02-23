using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private Vector2Int tile;
    private Vector2 offset;

    public void spawn(int depth){
      // get a valid spawning tile from the maze controller
      
      // update posiiton to that tile + a random offset
      
      // update the tile recording

    }

    public abstract void handleCollision();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
