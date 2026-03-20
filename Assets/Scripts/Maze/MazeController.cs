using UnityEngine;

public class MazeController : MonoBehaviour
{
  // objects to connect and add
  [SerializeField]
  private GameObject wallPrefab;

  [SerializeField]
  private MazeGrid maze;

  [SerializeField]
  private CameraController cam;

  // items we will be 'spawning'
  [SerializeField]
  private GameObject art;
  [SerializeField]
  private GameObject hintScroll;
  [SerializeField]
  private GameObject helperItem;
  [SerializeField]
  private GameObject pitfall;

  // these just have to be passed from the scene into the objects since the objects
  // are generated at runtime
  [SerializeField]
  private MessageHandler messageHandler;
  [SerializeField]
  private PlayerItemController itemController;

  // variables and containers
  [SerializeField]
  private int mazeDimsX;

  [SerializeField]
  private int mazeDimsY;

  [SerializeField]
  private float camTilesZoom;

  [SerializeField]
  private int wallThickness;

  private Wall[,] mazeWalls = null;
  private Wall[,] boundaries = new Wall[2,2];

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

    float minValue = Mathf.Max(camTilesZoom * (dims.x - 1), camTilesZoom * (dims.y - 1));
    cam.SetCameraSize(minValue * 0.75f);
  }

  private void renderMaze(int[,] mazeGrid){
    Vector2 dims = GetRoomSize();
    for(int i = -wallThickness; i < mazeDimsX+wallThickness; i++){
      for(int j = -wallThickness; j < mazeDimsY+wallThickness; j++){
        if(i < 0 || j < 0 || i >= mazeDimsX || j >= mazeDimsY || mazeGrid[i,j] >= 0){
          GameObject wall = Instantiate(wallPrefab,
              new Vector3(i * dims.x, j * dims.y, 0.0f),
              Quaternion.identity);

          wall.name = "Room_" + i.ToString() + "_" + j.ToString();
          mazeWalls[i+wallThickness, j+wallThickness] = wall.GetComponent<Wall>();
        }
      }
    }

    // create the boundaries of the maze
    // added an inner scope so I can fold it away lol
    /*
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
    */

  }

  private void spawnArtifact(){
    // create the artifact and get the interactable component
    GameObject artifact = Instantiate(art,
        new Vector3(-100, -100, -100),
        Quaternion.identity);
    Interactable interactable = artifact.GetComponent<Artifact>();

    // get the maximum distance found in the maze
    int maxDist = maze.getMaxDistance();

    // spawn the artifact at a given depth
    interactable.spawn(new Vector2Int((int)(maxDist * 0.5), (int)(maxDist * 0.8)), maze, GetRoomSize());
  }

  private void spawnHelperItems(){
    // get the maximum distance found in the maze
    int maxDist = maze.getMaxDistance();

    // spawn the HintScroll
    GameObject scroll = Instantiate(hintScroll,
        new Vector3(-100, -100, -100),
        Quaternion.identity);
    HintScroll interactable = scroll.GetComponent<HintScroll>();
    interactable.setMessageHandler(messageHandler);

    // spawn the artifact at a given depth
    interactable.spawn(new Vector2Int((int)(maxDist * 0.8), maxDist), maze, GetRoomSize());


    // spawn other items
    int numHelperItems = 5;
    for(int i = 0; i < numHelperItems; i++){

      // spawn the other items
      GameObject hItem = Instantiate(helperItem,
          new Vector3(-100, -100, -100),
          Quaternion.identity);
      HelperItem item = hItem.GetComponent<HelperItem>();
      
      // set the required attributes in the object
      item.setId(0);
      item.setMessageHandler(messageHandler);
      item.itemController = itemController;

      // spawn the item at a given depth
      item.spawn(new Vector2Int(0, maxDist), maze, GetRoomSize());

    }

    // spawn pitfalls
    int numPitfalls = 50;
    for(int i = 0; i < numPitfalls; i++){

      // spawn the HintScroll
      GameObject pfall = Instantiate(pitfall,
          new Vector3(-100, -100, -100),
          Quaternion.identity);
      Pitfall pf = pfall.GetComponent<Pitfall>();
      
      // set the required attributes in the object
      pf.setMessageHandler(messageHandler);

      // spawn the artifact at a given depth
      pf.spawn(new Vector2Int(0, maxDist), maze, GetRoomSize());

    }
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    setCamera();

    // create the maze object
    // I am just using a preexisting maze object
    mazeWalls = new Wall[mazeDimsX+wallThickness*2, mazeDimsY+wallThickness*2];

    // set the dimensions of the maze
    maze.mazeDimsX = mazeDimsX;
    maze.mazeDimsY = mazeDimsY;

    // generate the maze
    // render the maze
    renderMaze(maze.generateMaze());

    // add the artifact to the maze
    spawnArtifact();

    // populate the maze with other items
    spawnHelperItems();
  }
}
