using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class MazeGrid : MonoBehaviour
{
  // create a grid for the maze
  int[,] mazeGrid = null;
  int[,] gridDistances = null;

  public int mazeDimsX {get; set;} = 0;
  public int mazeDimsY {get; set;} = 0;



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
  private void visitTile(int x, int y, int dist){
    // update the minimum distance to this tile
    if(gridDistances[x,y] == 0 || gridDistances[x,y] > dist)
      gridDistances[x,y] = dist;

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
    visitTile(visitableNeighbour.x, visitableNeighbour.y, dist+1);

    // rerun the algorithm to backtrack
    visitTile(x, y, dist);
    return;
  }

  // note, maze grid dimensions must be set before 
  public int[,] generateMaze(){
    if(mazeDimsX == 0 || mazeDimsY == 0)
      throw new Exception("maze dims must be > 0");

    gridDistances = new int[mazeDimsX, mazeDimsY];
    mazeGrid = new int[mazeDimsX, mazeDimsY]; 

    visitTile(0, 0, 0); // hardcode the start tile as 0, 0 for now

    return mazeGrid;
  }

  public List<Vector2Int> getSpawnableTiles(Vector2Int distanceRange){
    // create a list
    List<Vector2Int> spawnableTiles = new List<Vector2Int>();

    // chec every distance, add to the spawnable tiles if in the range
    for(int x = 0; x < mazeDimsX; x++){
      for(int y = 0; y < mazeDimsY; y++){
        if(gridDistances[x,y] > distanceRange.x && gridDistances[x,y] <= distanceRange.y)
          spawnableTiles.Add(new Vector2Int(x, y));
      }
    }

    return spawnableTiles;
  }

  public int getMaxDistance(){
    int maxDist = 0;

    for(int x = 0; x < mazeDimsX; x++){
      for(int y = 0; y < mazeDimsY; y++){
        if(gridDistances[x,y] > maxDist)
          maxDist = gridDistances[x,y];
      }
    }

    return maxDist;
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
  }
}
