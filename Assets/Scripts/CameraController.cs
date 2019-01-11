using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public MapManager mapManager;
    public LayerMask roomLayer;
    public GameObject player;
    /// <summary>
    /// Coroutine controls scene changing.
    /// </summary>
    public static Coroutine sceneChanger;
    public static bool isSceneChanging = false;
    Coroutine fadeIn;
    Coroutine fadeOut;
    /*
     * If camera is in Tetris view, ideal position is (108, 240, -1)
     * size 300
     * */
    readonly float camX = 9.5f;
    readonly float camY = 4f;
    public Vector3 tetrisCameraCoord = new Vector3(108, 240, -1);
    public const float tetrisCameraSize = 300f;
    public const float inGameCameraSize = 4.5f;


    GameManager.GameState lastGameState;

    Vector3 destination;

    private void Awake()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
    }

    // Use this for initialization
    void Start()
    {
        lastGameState = GameManager.GameState.Ingame;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeState();
        GotoDestination();
        if(GameManager.gameState == GameManager.GameState.Ingame)
        {
            MapManager.originPos = transform.position;
        }
    }
    
    IEnumerator ChangeScene(Vector3 cameraDestination, float sizeDestination, GameManager.GameState _gameState)
    {
        GameObject grid = GameObject.Find("Grid");
        isSceneChanging = true;
        if (fadeIn != null)
            StopCoroutine(fadeIn);
        if (fadeOut != null)
            StopCoroutine(fadeOut);
        if(GameManager.gameState == GameManager.GameState.Ingame)
        {
            fadeIn = StartCoroutine(mapManager.RoomFadeIn(MapManager.currentRoom));
            grid.transform.position = new Vector3(0, 0, 0);
        }
        else if(GameManager.gameState == GameManager.GameState.Tetris)
        {
            fadeOut = StartCoroutine(mapManager.RoomFadeOut(MapManager.currentRoom));
            grid.transform.position = new Vector3(0, 0, 2);
        }
        while((_gameState == GameManager.GameState.Tetris && GetComponent<Camera>().orthographicSize < sizeDestination - 1) || (_gameState == GameManager.GameState.Ingame && GetComponent<Camera>().orthographicSize > sizeDestination + 0.0001))
        {
            yield return new WaitForSeconds(0.01f);
            Vector2 coord = Vector2.Lerp(transform.position, cameraDestination, Mathf.Sqrt(Time.deltaTime));
            transform.position = new Vector3(coord.x, coord.y, -1);
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, sizeDestination, Mathf.Sqrt(Time.deltaTime));
            MapManager.originPos = transform.position;
        }
        transform.position = cameraDestination;
        GetComponent<Camera>().orthographicSize = sizeDestination;
        isSceneChanging = false;
    }

    void GotoDestination()
    {
        // TODO: Change this.
        /*Vector3 pos = GameObject.Find("Player").transform.position;
        pos.z = -1;
        transform.position = pos;*/

        if (GameManager.gameState == GameManager.GameState.Ingame && isSceneChanging != true)
        {
            if(MapManager.isRoomFalling != true)
            {
                float posx = player.transform.position.x;
                float posy = player.transform.position.y;
            
                if (!MapManager.currentRoom.isRoomCleared)
                {

                    if (RoomCol(1) != -1)
                    {
                        posy = RoomCol(1) - camY;
                    }
                    if (RoomCol(2) != -1)
                    {
                        posy = RoomCol(2) + camY;
                    }
                    if (RoomCol(3) != -1)
                    {
                        posx = RoomCol(3) + camX;
                    }
                    if (RoomCol(4) != -1)
                    {
                        posx = RoomCol(4) - camX;
                    }

                    if (RoomCol(3) != -1 && RoomCol(4) != -1)
                    {
                        float middle = Player.tx * 24f + 12f;
                        if (middle - RoomCol(3) > 20f)
                        {
                            posx = RoomCol(3) + camX;
                        }
                        else if (RoomCol(4) - middle > 20f)
                        {
                            posx = RoomCol(4) - camX;
                        }
                        else
                        {
                            posx = player.transform.position.x;
                        }

                        //방의 중심과 비교하여 어느게 더 가까운가
                    }
                }
                transform.position = Vector3.Lerp(transform.position, new Vector3(posx, posy, -1), 2f * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, transform.position.y, -1); //카메라를 원래 z축으로 이동
            }
            /*else if(MapManager.isRoomFalling == true)
            {
                transform.position = player.transform.position + new Vector3(0, 0.2f, -1);
            }*/
        }
        //  Camera.main.transform.position = new Vector3(posx, posy, -10);
    }

    float RoomCol(int dir)
    {
        //1:up
        //2:down
        //3:left
        //4:right
        if (!(dir >= 0 && dir <= 4)) return -1;
        float distance = 0f;
        Vector2 direction = Vector2.up;
        Vector2 position = new Vector2(player.transform.position.x, player.transform.position.y);
        if (dir == 1)
        {
            direction = Vector2.up;
            distance = camY;
        }
        if (dir == 2)
        {
            direction = Vector2.down;
            distance = camY;
        }
        if (dir == 3)
        {
            direction = Vector2.left;
            distance = camX;
        }
        if (dir == 4)
        {
            direction = Vector2.right;
            distance = camX;
        }

        RaycastHit2D hit1 = Physics2D.Raycast(position, direction, distance, roomLayer);

        //Debug.DrawRay(position, direction,Color.yellow);
        if (hit1.collider != null)
        {
            if (dir == 1 || dir == 2)
                return hit1.point.y;
            else return hit1.point.x;
        }

        return -1;
    }

    public void ChangeState()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Vector3 cameraDestination = new Vector3(0, 0, 0);
            float sizeDestination = 0f;
            if (GameManager.gameState == GameManager.GameState.Ingame)
            {
                cameraDestination = tetrisCameraCoord;
                sizeDestination = tetrisCameraSize;
                GameManager.gameState = GameManager.GameState.Tetris;
            }
            else if (GameManager.gameState == GameManager.GameState.Tetris)
            {
                cameraDestination = player.transform.position;
                sizeDestination = inGameCameraSize;
                GameManager.gameState = GameManager.GameState.Ingame;
            }
            if (sceneChanger != null)
                StopCoroutine(sceneChanger);
            sceneChanger = StartCoroutine(ChangeScene(cameraDestination, sizeDestination, GameManager.gameState));
        }
    }
}