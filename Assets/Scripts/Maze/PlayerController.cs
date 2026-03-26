using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Patterns;

public class PlayerController : MonoBehaviour, Observer
{
  private const double invalidPos = -1E3;

  private GameObject player;

  [SerializeField]
  private MazeController mazeController;

  [SerializeField]
  private float timeslice;

  [SerializeField]
  private float maxVel;

  [SerializeField]
  private float minVel;

  [SerializeField]
  private float acceleration;

  [SerializeField]
  private float velDecay;

  [SerializeField]
  private PlayerItemController itemController;

  private Vector2 targetPosition;
  private float targetDistance;
  private float startingVelocity;
  private bool teleporting;

  Rigidbody2D rb;
  Animator animator;

    public enum Directions{
      UP,
      DOWN,
      LEFT,
      RIGHT
    }
    
    private Vector2  velocity;
    List<Directions> pressedDirections;

    private void updateVelocity(List<Directions> pressedKeys){
      if(pressedKeys.Contains(Directions.DOWN))
        velocity.y -= acceleration*timeslice;
      else if(pressedKeys.Contains(Directions.UP))
        velocity.y += acceleration*timeslice;
      else
        velocity.y /= velDecay;

      if(pressedKeys.Contains(Directions.LEFT))
        velocity.x -= acceleration*timeslice;
      else if(pressedKeys.Contains(Directions.RIGHT))
        velocity.x += acceleration*timeslice;
      else
        velocity.x /= velDecay;

      if(velocity.magnitude > maxVel){
        velocity /= velocity.magnitude;
        velocity *= maxVel;
      }

      rb.linearVelocity = velocity;
    }

    private void updateVelocity(Vector2 mouseClickPosition){
      float curDist = Vector2.Distance(mouseClickPosition, (Vector2)player.transform.position);
      if(curDist < 1E-2){
        targetPosition.x = (float)invalidPos;
        targetPosition.y = (float)invalidPos;
        return;
      }
      float velocityScalar = Math.Abs((float)(-4*maxVel*curDist*(curDist-targetDistance)/(Math.Pow(targetDistance,2)))) + minVel;
      velocityScalar += Math.Abs(startingVelocity * (targetDistance - curDist*2));
      velocity = velocityScalar*(mouseClickPosition - (Vector2)player.transform.position)/curDist;

      rb.linearVelocity = velocity;
    }
  
    private void handletouch(Vector3 clickPos){
        // ignore clicks on buttons
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(clickPos);

        if(!teleporting){
          targetPosition.x = worldPos.x;
          targetPosition.y = worldPos.y;

          // record the starting velocity so that clicking again doesnt slow down the character
          startingVelocity = rb.linearVelocity.magnitude;
          targetDistance = Vector2.Distance(targetPosition, (Vector2)player.transform.position);
        }else if(worldPos.x >= 0 && worldPos.y >= 0){
          // make sure we are teleporting on an open tile
          Vector2 roomSize = mazeController.GetRoomSize();
          Vector2Int tile_index = new Vector2Int();
          tile_index.x =(int)((worldPos.x + roomSize.x/2) / roomSize.x);
          tile_index.y =(int)((worldPos.y + roomSize.y/2) / roomSize.y);

          if(mazeController.getMazeGrid()
              .getSpawnableTiles( new Vector2Int(0, mazeController.getMazeGrid()
              .getMaxDistance())).Contains(tile_index))
          {
            teleporting = false;
            player.transform.position = worldPos;
          }

        }

    }

    private void handleInputs(){
      pressedDirections.Clear();

      if(Touch.activeTouches.Count > 0){
        // Get the first touch
        Vector3 touchPos = Touch.activeTouches[0].screenPosition;

        handletouch(touchPos);

      // prioritize mouse movement
      }else if(Mouse.current.leftButton.wasPressedThisFrame){
        // read the mouse click point
        Vector3 mousePos = Mouse.current.position.ReadValue();

        handletouch(mousePos);
      // allow for keyboard control
      }else{

        if(Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed){
          pressedDirections.Add(Directions.RIGHT);
        }
        else if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed){
          pressedDirections.Add(Directions.LEFT);
        }
        if(Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed){
          pressedDirections.Add(Directions.UP);
        }
        else if(Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed){
          pressedDirections.Add(Directions.DOWN);
        }
      }

      // update the velocity if there are keyboard inputs, decay it otherwise, and prioritize the mouse
      if(pressedDirections.Count > 0){
        updateVelocity(pressedDirections);
        targetPosition.x = (float)invalidPos;
        targetPosition.y = (float)invalidPos;
      }
      else if(targetPosition.x > invalidPos && targetPosition.y > invalidPos)
        updateVelocity(targetPosition);
      else
        updateVelocity(pressedDirections);
    }

    public void notify(){
      // if notified, check if we have to update properties based on the items picked up
      // this works for the item pickup system
      if(itemController.hasItem(HelperItem.itemName.SpeedBoost)){
        maxVel *= (float)1.2;
        itemController.useItem(HelperItem.itemName.SpeedBoost);
      }
    }

    public void onTeleportClick(){
      if(!teleporting && itemController.hasItem(HelperItem.itemName.Teleport)){
        itemController.useItem(HelperItem.itemName.Teleport);
        teleporting = true; // now press handler knows to look out for teleport
      }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      player = gameObject;
      rb = GetComponent<Rigidbody2D>();
        
      velocity = new Vector2(0,0);
      targetPosition = new Vector2((float)invalidPos, (float)invalidPos);
      pressedDirections = new List<Directions>();
      teleporting = false;

      itemController.addObserver(this);

      Input.simulateMouseWithTouches = true;
      UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
    }

    // Update is called once per frame
    void Update()
    {
      handleInputs();
    }

    
    public Vector2 GetCurrentVelocity()
    {
        return rb.linearVelocity; // Returns the current movement speed and direction
    }

}
