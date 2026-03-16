using UnityEngine;
using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public abstract class Interactable : MonoBehaviour
{
  private Vector2Int tile;
  private Vector2 offset;
  protected bool collided = false;
  
  protected MessageHandler messageHandler = null;
  public void setMessageHandler(MessageHandler mh){
    messageHandler = mh;
  }

  public void spawn(Vector2Int depth, MazeGrid tiles, Vector2 roomSize){
    // get a valid spawning tile from the maze grid
    List<Vector2Int> spawnableTiles = tiles.getSpawnableTiles(depth);
    Vector2Int spawnableTile = spawnableTiles[UnityEngine.Random.Range(0, spawnableTiles.Count)];

    // update posiiton to that tile + a random offset
    float random = UnityEngine.Random.Range(0f, 260f);
    offset = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    transform.position = new Vector3(spawnableTile.x * roomSize.x + offset.x,
        spawnableTile.y * roomSize.y + offset.y, 0.0f);

    // update the tile recording
    tile = spawnableTile;
  }

  void OnTriggerEnter2D(Collider2D other){
    if(!collided){
    handleCollision(other);
    collided = true;
    }
  }

  public abstract void handleCollision(Collider2D other);
}
