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
    public int statueEvent = 0;
    /// <summary>
    /// Position where portal would spawn player.
    /// </summary>
    Vector3 spawnPosition = new Vector3(2, 1, 0);

    public GameObject player;
    public GameObject minimap;
    public GameObject tetrisAlert;
    public GameObject statue;
    public Canvas gameOverCanvas;
    public Canvas inventoryCanvas;
    public Canvas textCanvas;
    public Timer clock;
    public Text leftRoomCount;
    public Text displayText;
    public Coroutine textCoroutine;

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
        clock.gameObject.SetActive(true);
        leftRoomCount.gameObject.SetActive(true);
        TetriminoSpawner.Instance.MakeInitialTetrimino();
        player.transform.position = MapManager.currentRoom.roomInGame.transform.Find("portal spot").position + spawnPosition;
        Camera.main.transform.position = player.transform.position + new Vector3(0, 0, -1);
        StartCoroutine(TestCoroutine());
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
        leftRoomCount.text = "x" + (MapManager.Instance.stageClearCondition[MapManager.currentStage] - MapManager.Instance.clearedRoomCount).ToString();
    }
    
    public void DisplayText(string _text)
    {
        StopCoroutine(textCoroutine);
        textCoroutine = StartCoroutine(DisplayTextCoroutine(_text));
    }

    public IEnumerator DisplayTextCoroutine(string _text)
    {
        displayText.text = _text;
        for (int i = 255; i >= 0; i -= 10)
        {
            yield return null;
            displayText.transform.position = player.transform.position + new Vector3(0, 2, 0);
            displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, (float)i / 255);
        }
        displayText.text = "";
    }

    void Awake()
    {
        inventoryCanvas = Instantiate(inventoryCanvas);
        gameOverCanvas = Instantiate(gameOverCanvas);
        textCanvas = Instantiate(textCanvas);
        InventoryManager.Instance.ui = inventoryCanvas.GetComponent<InventoryUI>();
        gameState = GameState.Ingame;
        minimap = GameObject.Find("Minimap");
        player = GameObject.Find("Player");
        MapManager.currentRoom = GameObject.Find("Room Tutorial").GetComponent<Room>();
        clock = GameObject.Find("Clock").GetComponent<Timer>();
        leftRoomCount = GameObject.Find("LeftRoom").GetComponent<Text>();
        tetrisAlert = GameObject.Find("TetrisAlert");
    }

    // Use this for initialization
    void Start ()
    {
        player.transform.position = MapManager.currentRoom.roomInGame.transform.Find("player spot").position + spawnPosition;
        Camera.main.transform.position = player.transform.position + new Vector3(0, 0, -1);
        inventoryCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        minimap.SetActive(false);
        clock.gameObject.SetActive(false);
        leftRoomCount.gameObject.SetActive(false);
        displayText = Instantiate(displayText, textCanvas.transform);
        isTutorial = true;
        textCoroutine = StartCoroutine(DisplayTextCoroutine(""));
    }

    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(Camera.main.GetComponent<CameraController>().ChangeScene(GameState.Tetris));
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

        if(statueEvent == 0 && MapManager.Instance.clearedRoomCount == 9)
        {
            statueEvent = 1;
        }
    }
}
