using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour
{
  [SerializeField]
  CameraController cam;

  [SerializeField]
  GameObject wallPrefab;

  // create a grid for the maze
  int[,] mazeGrid = null;
  Wall[,] mazeWalls = null;
  Wall[,] boundaries = new Wall[2,2];

  [SerializeField]
  int mazeDimsX;

  [SerializeField]
  int mazeDimsY;

  // check if a tile is a corner piece
  private bool isCorner(int x, int y){
    if(x > 0){
      if(y > 0){
        if(mazeGrid[x-1,y-1] < 0 && mazeGrid[x,y-1] >= 0 && mazeGrid[x-1,y] >= 0)
          return true;
      }if(y + 1 < mazeDimsY){
        if(mazeGrid[x-1,y+1] < 0 && mazeGrid[x,y+1] >= 0 && mazeGrid[x-1,y] >= 0)
          return true;
      }
    }if(x + 1 < mazeDimsX){
      if(y > 0){
        if(mazeGrid[x+1,y-1] < 0 && mazeGrid[x,y-1] >= 0 && mazeGrid[x+1,y] >= 0)
          return true;
      }if(y + 1 < mazeDimsY){
        if(mazeGrid[x+1,y+1] < 0 && mazeGrid[x,y+1] >= 0 && mazeGrid[x+1,y] >= 0)
          return true;
      }
    }

    return false;
  }

  // visit a tile, change its visit count and update the visit count of its neightbours
  private void visitTile(int x, int y){
    // visit a tile, giving it a visit count of -1 for open path
    if(mazeGrid[x, y] >= 0)
      mazeGrid[x,y] = -1;
    else
      mazeGrid[x,y] = -2;

    List<Vector2Int> visitableTile = new List<Vector2Int>();

    // loop over all the neighbours of this tile
    for(int i = -1; i < 2; i++){
      for(int j = -1; j < 2; j++){
        if(x + i < 0 || x + i >= mazeDimsX || y + j < 0 || y + j >= mazeDimsY || 
            (i == 0 && j == 0) || mazeGrid[x+i,y+j] < 0 || (Math.Abs(i) + Math.Abs(j) > 1)){
          continue;
        }

        // increase the visit count of its neighbours
        if(mazeGrid[x,y] >= -1)
          mazeGrid[x+i,y+j] += 1;

        // find a random neighbour with visit count < 2, making sure its not a corner piece
        if(mazeGrid[x+i,y+j] < 2 && !isCorner(x+i, y+j)){
          visitableTile.Add(new Vector2Int(x+i, y+j));
        }
      }
    }
    if(visitableTile.Count == 0) return;

    // choose a random neighbour to move to next
    Vector2Int visitableNeighbour = visitableTile[UnityEngine.Random.Range(0, visitableTile.Count)];

    // rerun the algorithm for this neighbour (recursively)
    visitTile(visitableNeighbour.x, visitableNeighbour.y);

    // rerun the algorithm to backtrack
    visitTile(x, y);
    return;
  }

  private void generateMaze(){
    mazeGrid = new int[mazeDimsX, mazeDimsY]; 

    visitTile(0,0); // hardcode the start tile as 0, 0 for now


  }

  public Vector2 GetRoomSize(){
    SpriteRenderer[] spriteRenderers = wallPrefab.GetComponentsInChildren<SpriteRenderer>();

    Vector3 minBounds = Vector3.positiveInfinity;
    Vector3 maxBounds = Vector3.negativeInfinity;

    foreach(SpriteRenderer ren in spriteRenderers){
      minBounds = Vector3.Min(minBounds, ren.bounds.min);
      maxBounds = Vector3.Max(maxBounds, ren.bounds.max);
    }

    return new Vector2(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y);
  }

  private void setCamera(){
    Vector2 dims = GetRoomSize();

    float minValue = Mathf.Max(5.5f * (dims.x - 1), 5.5f * (dims.y - 1));
    cam.SetCameraSize(minValue * 0.75f);
  }
  
  private void renderMaze(){
    Vector2 dims = GetRoomSize();
    for(int i = 0; i < mazeDimsX; i++){
      for(int j = 0; j < mazeDimsY; j++){
        if(mazeGrid[i,j] >= 0){
          GameObject wall = Instantiate(wallPrefab,
              new Vector3(i * dims.x, j * dims.y, 0.0f),
              Quaternion.identity);

          wall.name = "Room_" + i.ToString() + "_" + j.ToString();
          mazeWalls[i, j] = wall.GetComponent<Wall>();
        }
      }
    }

    GameObject boundary_00 = Instantiate(wallPrefab,
        new Vector3((mazeDimsX - 1) * dims.x/2, -1 * dims.y, 0.0f),
        Quaternion.identity);

    boundary_00.transform.localScale = new Vector3(mazeDimsX + 2, 1, 1);
    boundaries[0,0] = boundary_00.GetComponent<Wall>();

    GameObject boundary_01 = Instantiate(wallPrefab,
        new Vector3((mazeDimsX - 1) * dims.x/2, mazeDimsY * dims.y, 0.0f),
        Quaternion.identity);

    boundary_01.transform.localScale = new Vector3(mazeDimsX + 2, 1, 1);
    boundaries[1,0] = boundary_01.GetComponent<Wall>();


    GameObject boundary_10 = Instantiate(wallPrefab,
        new Vector3(-1 * dims.x, (mazeDimsY - 1) * dims.y/2, 0.0f),
        Quaternion.identity);

    boundary_10.transform.localScale = new Vector3(1, mazeDimsY + 2, 1);
    boundaries[0,0] = boundary_10.GetComponent<Wall>();

    GameObject boundary_11 = Instantiate(wallPrefab,
        new Vector3(mazeDimsX * dims.y, (mazeDimsY - 1) * dims.y/2, 0.0f),
        Quaternion.identity);

    boundary_11.transform.localScale = new Vector3(1, mazeDimsY + 2, 1);
    boundaries[1,0] = boundary_11.GetComponent<Wall>();
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    Debug.Log("Generating maze: ");
    Debug.Log("X: " + mazeDimsX + ", Y: " + mazeDimsY);
    mazeWalls = new Wall[mazeDimsX, mazeDimsY];
    generateMaze();
    renderMaze();
    setCamera();
  }

  // Update is called once per frame
  void Update()
  {

  }
}
