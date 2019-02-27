using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    /// <summary>
    /// Which state this game is.
    /// change later
    /// </summary>
    public static GameState gameState;

    public bool isTutorial = false;
    private bool statueEvent = false;
    /// <summary>
    /// Position where portal would spawn player.
    /// </summary>
    Vector3 spawnPosition = new Vector3(2, 1, 0);

    public GameObject player;
    public GameObject minimap;
    public GameObject statue;
    public Canvas gameOverCanvas;
    public Canvas inventoryCanvas;
    public Canvas textCanvas;
    public Timer clock;
    public Text leftRoomCount;

    // method
    // Constructor - protect calling raw constructor
    protected GameManager() { }

    /// <summary>
    /// Ends the tutorial and start real game.
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(1f);
        isTutorial = false;
        Destroy(MapManager.currentRoom.gameObject);
        minimap.SetActive(true);
        TetriminoSpawner.Instance.MakeInitialTetrimino();
        player.transform.position = MapManager.currentRoom.roomInGame.transform.Find("portal spot").position + spawnPosition;
        Camera.main.transform.position = player.transform.position + new Vector3(0, 0, -1);
    }

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void CountLeftRoom()
    {
        leftRoomCount.text = "Left Room : " + (MapManager.Instance.stageClearCondition[MapManager.currentStage] - MapManager.Instance.clearedRoomCount).ToString();
    }

    void Awake()
    {
        inventoryCanvas = Instantiate(inventoryCanvas);
        gameOverCanvas = Instantiate(gameOverCanvas);
        textCanvas = Instantiate(textCanvas);
        InventoryManager.Instance.ui = inventoryCanvas.GetComponent<InventoryUI>();
        inventoryCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        gameState = GameState.Ingame;
        minimap = GameObject.Find("Minimap");
        minimap.SetActive(false);
        player = GameObject.Find("Player");
        MapManager.currentRoom = GameObject.Find("Room Tutorial").GetComponent<Room>();
        player.transform.position = MapManager.currentRoom.roomInGame.transform.Find("player spot").position + spawnPosition;
        Camera.main.transform.position = player.transform.position + new Vector3(0, 0, -1);
        clock = GameObject.Find("Clock").GetComponent<Timer>();
        leftRoomCount = GameObject.Find("LeftRoom").GetComponent<Text>();
        isTutorial = true;
    }

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(CameraController.isSceneChanging != true)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !isTutorial)
            {
                if (gameState == GameState.Ingame)
                {
                    StartCoroutine(Camera.main.GetComponent<CameraController>().ChangeScene(GameState.Tetris));
                }
                else if (gameState == GameState.Tetris)
                {
                    StartCoroutine(Camera.main.GetComponent<CameraController>().ChangeScene(GameState.Ingame));
                }
            }
            else if(Input.GetKeyDown(KeyCode.I))
            {
                if(gameState == GameState.Ingame)
                {
                    inventoryCanvas.gameObject.SetActive(true);
                    InventoryManager.Instance.SetOnPosition();
                    gameState = GameState.Inventory;
                }
                else if(gameState == GameState.Inventory)
                {
                    inventoryCanvas.gameObject.SetActive(false);
                    gameState = GameState.Ingame;
                }
            }
            else if (Input.GetButtonDown("Interaction"))
            {
                if (gameState == GameState.Portal && MapManager.currentRoom != MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y])
                {
                    player.transform.position = MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y].portal.transform.position + spawnPosition;
                    MapManager.Instance.ChangeRoom(MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y]);
                    MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y].portalSurface.GetComponent<SpriteRenderer>().sprite =
                        MapManager.Instance.portalExist;
                    StartCoroutine(Camera.main.GetComponent<CameraController>().ChangeScene(GameState.Ingame));
                }
            }
            else if(Input.GetButtonDown("Cancel"))
            {
                if(gameState == GameState.Portal)
                {
                    MapManager.mapGrid[(int)MapManager.currentRoom.mapCoord.x, (int)MapManager.currentRoom.mapCoord.y].portalSurface.GetComponent<SpriteRenderer>().sprite =
                        MapManager.Instance.portalExist;
                    MapManager.mapGrid[(int)MapManager.currentRoom.mapCoord.x, (int)MapManager.currentRoom.mapCoord.y].portalSurface.GetComponent<SpriteRenderer>().sprite =
                        MapManager.Instance.portalExist;
                    StartCoroutine(Camera.main.GetComponent<CameraController>().ChangeScene(GameState.Ingame));
                }
                else if(gameState == GameState.Inventory)
                {
                    inventoryCanvas.gameObject.SetActive(false);
                    gameState = GameState.Ingame;
                }
            }
        }
        if(gameState == GameState.GameOver)
        {
            if (gameOverCanvas.isActiveAndEnabled == false)
            {
                Debug.Log("Game Over");
                gameOverCanvas.gameObject.SetActive(true);
            }
        }
        CountLeftRoom();

        if(!statueEvent && MapManager.Instance.clearedRoomCount == 9)
        {
            foreach(Transform obj in MapManager.currentRoom.transform)
            {
                if (obj.CompareTag("Enemy")) {
                    GameObject body = obj.GetChild(0).gameObject;
                    if (body.GetComponent<EnemyGround>() != null && body.GetComponent<SpriteRenderer>().isVisible)
                    {
                        Vector3 pos = obj.position;
                        body.gameObject.GetComponent<EnemyGround>().ResetForPool();
                        GameObject tempStatue = Instantiate(statue, pos, Quaternion.identity);
                        statueEvent = true;
                    }
                }
            }
        }
    }
}
