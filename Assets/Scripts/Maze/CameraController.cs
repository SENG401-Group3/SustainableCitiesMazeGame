using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField]
  Rigidbody2D player;

  [SerializeField]
  public Vector3 offset = new Vector3(0, 5, -8);

  [SerializeField]
  public float followSharpness = 15f;

  public void SetCameraPosition(Vector2 position){
    Camera.main.transform.position = new Vector3(position.x, position.y, -100.0f);
  }
  
  public void SetCameraSize(float size){
    Camera.main.orthographicSize = size;
  }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      SetCameraPosition(new Vector2(0,0));
    }

    // Update is called once per frame
    void Update()
    {
      SetCameraPosition(
          new Vector2(player.transform.position.x,
            player.transform.position.y));
        
    }
}
