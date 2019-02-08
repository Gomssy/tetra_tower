using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public MapManager mapManager;
    public GameObject player;
    /// <summary>
    /// Check if scene is changing now.
    /// </summary>
    public static bool isSceneChanging = false;
    const float cameraXLimit = 4.5f;
    const float cameraYLimit = 3f;
    public Vector3 tetrisCameraCoord = new Vector3(108, 240, -1);
    public const float tetrisCameraSize = 300f;
    public const float inGameCameraSize = 4.5f;

    public Vector3 originPos;
    

    private void Awake()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameManager.gameState == GameState.Ingame)
        {
            FollowPlayer();
            originPos = transform.position;
        }
        else if (GameManager.gameState == GameState.Tetris || GameManager.gameState == GameState.Portal)
        {
            originPos = tetrisCameraCoord;
        }
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
            transform.position = new Vector3(0.2f * Random.insideUnitCircle.x * amount * GetComponent<Camera>().orthographicSize, 
                Random.insideUnitCircle.y * amount * GetComponent<Camera>().orthographicSize, 0) + originPos;
            amount -= _amount / 40;
            yield return null;
        }
        transform.localPosition = originPos;
    }

    /// <summary>
    /// Change scene between tetris and ingame.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ChangeScene(GameState gameState)
    {
        if(isSceneChanging != true)
        {
            GameObject grid = GameObject.Find("Grid");
            float sizeDestination = 0;
            isSceneChanging = true;
            if (gameState == GameState.Ingame)
            {
                GameManager.gameState = GameState.Ingame;
                StartCoroutine(mapManager.RoomEnter(MapManager.currentRoom));
                grid.transform.position = new Vector3(0, 0, 0);
                sizeDestination = inGameCameraSize;
                while (GetComponent<Camera>().orthographicSize > sizeDestination + 0.01)
                {
                    yield return null;
                    FollowPlayer();
                    GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, sizeDestination, Mathf.Sqrt(Time.deltaTime));
                }
            }
            else if (gameState == GameState.Tetris || gameState == GameState.Portal)
            {
                if(gameState == GameState.Tetris)
                    GameManager.gameState = GameState.Tetris;
                else if(gameState == GameState.Portal)
                {
                    GameManager.gameState = GameState.Portal;
                    MapManager.portalDestination = MapManager.currentRoom.mapCoord;
                    MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y].portalSurface.GetComponent<SpriteRenderer>().sprite =
                        GameObject.Find("MapManager").GetComponent<MapManager>().portalSelected;
                }
                StartCoroutine(mapManager.RoomExit(MapManager.currentRoom));
                grid.transform.position = new Vector3(0, 0, 2);
                sizeDestination = tetrisCameraSize;
                while (GetComponent<Camera>().orthographicSize < sizeDestination - 2)
                {
                    yield return null;
                    Vector2 coord = Vector2.Lerp(transform.position, tetrisCameraCoord, Mathf.Sqrt(Time.deltaTime));
                    transform.position = new Vector3(coord.x, coord.y, -1);
                    GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, sizeDestination, Mathf.Sqrt(Time.deltaTime));
                }
                transform.position = tetrisCameraCoord;
            }
            GetComponent<Camera>().orthographicSize = sizeDestination;
            isSceneChanging = false;
        }
    }

    void FollowPlayer()
    {
        float posx = player.transform.position.x;
        float posy = player.transform.position.y;
        if (RoomCol("Up") != -1)
            posy = RoomCol("Up") - cameraYLimit;
        if (RoomCol("Down") != -1)
            posy = RoomCol("Down") + cameraYLimit;
        if (RoomCol("Left") != -1)
            posx = RoomCol("Left") + cameraXLimit;
        if (RoomCol("Right") != -1)
            posx = RoomCol("Right") - cameraXLimit;
        if (RoomCol("Left") != -1 && RoomCol("Right") != -1)
        {
            float middle = Player.tx * 24f + 12f;
            if (middle - RoomCol("Left") > 20f)
                posx = RoomCol("Left") + cameraXLimit;
            else if (RoomCol("Right") - middle > 20f)
                posx = RoomCol("Right") - cameraXLimit;
            else
                posx = player.transform.position.x;
            //방의 중심과 비교하여 어느게 더 가까운가
        }
        if (MapManager.isRoomFalling != true)
            transform.position = Vector3.Lerp(transform.position, new Vector3(posx, posy, -1), 4f * Time.deltaTime);
        else if (MapManager.isRoomFalling == true)
            transform.position = Vector3.Lerp(transform.position, new Vector3(posx, posy, -1), 0.9f);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    float RoomCol(string direction)
    {
        Vector2 position = player.transform.position;
        switch (direction)
        {
            case "Up":
                if (position.y + cameraYLimit >= MapManager.tetrisYCoord[(int)MapManager.currentRoom.mapCoord.y] + 23f)
                    return MapManager.tetrisYCoord[(int)MapManager.currentRoom.mapCoord.y] + 23f;
                break;
            case "Down":
                if (position.y - cameraYLimit <= MapManager.tetrisYCoord[(int)MapManager.currentRoom.mapCoord.y] + 1f)
                    return MapManager.tetrisYCoord[(int)MapManager.currentRoom.mapCoord.y] + 1f;
                break;
            case "Left":
                if(position.x - cameraXLimit <= Player.tx * 24f + 1)
                    return Player.tx * 24f + 1;
                break;
            case "Right":
                if(position.x + cameraXLimit >= (Player.tx + 1) * 24f - 1)
                    return (Player.tx + 1) * 24f - 1;
                break;
        }
        return -1;
    }
}