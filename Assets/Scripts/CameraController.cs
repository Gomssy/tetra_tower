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
    /// <summary>
    /// Check if scene is changing now.
    /// </summary>
    public static bool isSceneChanging = false;
    /// <summary>
    /// Coroutine controls room fade in when camera zoom in.
    /// </summary>
    Coroutine fadeIn;
    /// <summary>
    /// Coroutine controls room fade out when camera zoom out.
    /// </summary>
    Coroutine fadeOut;
    readonly float camX = 9.5f;
    readonly float camY = 4f;
    public Vector3 tetrisCameraCoord = new Vector3(108, 240, -1);

    public Vector3 originPos;
    public const float tetrisCameraSize = 300f;
    public const float inGameCameraSize = 4.5f;
    

    private void Awake()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GotoDestination();
        if (GameManager.gameState == GameManager.GameState.Ingame)
            originPos = player.transform.position + new Vector3(0, 0, -1);
        else if (GameManager.gameState == GameManager.GameState.Tetris)
            originPos = tetrisCameraCoord;
    }

    /// <summary>
    /// Shake camera.
    /// </summary>
    /// <param name="_amount">Amount of shaking camera.</param>
    /// <returns></returns>
    public IEnumerator CameraShake(float _amount)
    {
        float amount = _amount;
        while (amount > 0)
        {
            transform.position = new Vector3(0.2f * Random.insideUnitCircle.x * amount * GetComponent<Camera>().orthographicSize + originPos.x, 
                Random.insideUnitCircle.y * amount * GetComponent<Camera>().orthographicSize + originPos.y, originPos.z);
            amount -= _amount / 40;
            yield return null;
        }
        transform.localPosition = originPos;
    }

    /// <summary>
    /// Change scene between tetris and ingame.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ChangeScene()
    {
        GameObject grid = GameObject.Find("Grid");
        float sizeDestination = 0;
        isSceneChanging = true;
        if (GameManager.gameState == GameManager.GameState.Ingame)
        {
            StartCoroutine(mapManager.RoomFadeIn(MapManager.currentRoom));
            grid.transform.position = new Vector3(0, 0, 0);
            sizeDestination = inGameCameraSize;
        }
        else if (GameManager.gameState == GameManager.GameState.Tetris)
        {
            StartCoroutine(mapManager.RoomFadeOut(MapManager.currentRoom));
            grid.transform.position = new Vector3(0, 0, 2);
            sizeDestination = tetrisCameraSize;
        }
        while ((GameManager.gameState == GameManager.GameState.Tetris && GetComponent<Camera>().orthographicSize < sizeDestination - 5) ||
            (GameManager.gameState == GameManager.GameState.Ingame && GetComponent<Camera>().orthographicSize > sizeDestination + 0.05))
        {
            yield return null;
            Vector2 coord = Vector2.Lerp(transform.position, originPos, Mathf.Sqrt(Time.deltaTime));
            transform.position = new Vector3(coord.x, coord.y, -1);
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, sizeDestination, Mathf.Sqrt(Time.deltaTime));
        }
        transform.position = originPos;
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
            else if(MapManager.isRoomFalling == true)
            {
                transform.position = player.transform.position + new Vector3(0, 0.2f, -1);
            }
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
}