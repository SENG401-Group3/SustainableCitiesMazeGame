using UnityEngine;
using Patterns;
using System;

public class CameraController : MonoBehaviour
{
  [SerializeField]
  Rigidbody2D player;

  // flag to not update while we're showing the maze
  private bool zooming;

  // Move the camera to a specific position in the scene
  // position: vector coordinate of the camera in the scene
  public void SetCameraPosition(Vector2 position){
    Camera.main.transform.position = new Vector3(position.x, position.y, -100.0f);
  }

  // set how zoomed in the camera is
  // size: unity length units
  public void SetCameraSize(float size){
    Camera.main.orthographicSize = size;
  }

  // make the camera track the player
  void trackCamera(){
    SetCameraPosition(
        new Vector2(player.transform.position.x,
          player.transform.position.y));
  }

  // bring the camera back in after a telescope useage
  // has to be a separate function so we can invoke it with delay
  private void ZoomCameraIn(){
    SetCameraSize(5.5f*4.12f * 0.75f);
    zooming = false;
  }

  // handle a telescope usage
  public void onTelescopeClick(){
    zooming = true;

    // run the animation
    Debug.Log("running animation");
    SetCameraPosition(new Vector2(5.12f*12.5f, 5.12f*12.5f));
    SetCameraSize(25*4.12f*0.5f);
    Invoke("ZoomCameraIn", 8.0f);
    Debug.Log("animation finished");
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    SetCameraPosition(new Vector2(0,0));
    zooming = false;
  }

  // Update is called once per frame
  void Update()
  {
    if(!zooming)
      trackCamera();
  }
}
