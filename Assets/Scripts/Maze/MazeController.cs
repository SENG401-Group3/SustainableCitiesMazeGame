using UnityEngine;

public class MazeController : MonoBehaviour
{
  // objects to connect and add
  [SerializeField]
  GameObject wallPrefab;

  [SerializeField]
  MazeGrid maze;

  [SerializeField]
  CameraController cam;


  // variables and containers
  [SerializeField]
  int mazeDimsX;

  [SerializeField]
  int mazeDimsY;

  Wall[,] mazeWalls = null;
  Wall[,] boundaries = new Wall[2,2];

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
  
  private void renderMaze(int[,] mazeGrid){
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

    // create the boundaries of the maze
    // added an inner scope so I can fold it away lol
    {
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

  }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      setCamera();

      // create the maze object
        // I am just using a preexisting maze object
      mazeWalls = new Wall[mazeDimsX, mazeDimsY];

      // set the dimensions of the maze
      maze.mazeDimsX = mazeDimsX;
      maze.mazeDimsY = mazeDimsY;

      // generate the maze
      // render the maze
      renderMaze(maze.generateMaze());
        
      // add the artifact to the maze
      
      // populate the maze with other items
    }
}
