using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public LayerMask roomLayer;
    public GameObject player;
    /*
     * If camera is in Tetris view, ideal position is (108, 240, -1)
     * size 300
     * */
    readonly float camX = 9.5f;
    readonly float camY = 4f;
    private Vector3 tetrisCameraCoord = new Vector3(108, 240, -1);
    private float tetrisCameraSize = 300f;
    private float inGameCameraSize = 4.5f;


    GameManager.GameState lastGameState;

    Vector3 destination;


    // Use this for initialization
    void Start()
    {
        lastGameState = GameManager.GameState.Ingame;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (lastGameState != GameManager.gameState)
        {
            StartCoroutine("ChangeScene");
            lastGameState = GameManager.gameState;
        }
        else if (lastGameState == GameManager.GameState.Ingame)
        {
            SetDestination();
        }*/
        ChangeState();
        GotoDestination();
    }


    IEnumerator ChangeScene()
    {


        yield return null;

    }
    void SetDestination()
    {

    }

    void GotoDestination()
    {
        // TODO: Change this.
        /*Vector3 pos = GameObject.Find("Player").transform.position;
        pos.z = -1;
        transform.position = pos;*/

        if (GameManager.gameState == GameManager.GameState.Ingame)
        {
            float posx = player.transform.position.x;
        float posy = player.transform.position.y;

            Room curRoom = MapManager.mapGrid[Player.tx, Player.ty];
            if (!curRoom.GetComponentInChildren<RoomInGame>().isRoomClear)
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
            

                transform.position = Vector3.Lerp(transform.position, new Vector3(posx, posy, 0), 2f * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, transform.position.y, -1); //카메라를 원래 z축으로 이동
            
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
        GameObject grid = GameObject.Find("Grid");
        if (Input.GetKeyDown(KeyCode.Tab) && GameManager.gameState == GameManager.GameState.Ingame)
        {
            GameManager.gameState = GameManager.GameState.Tetris;
            transform.position = tetrisCameraCoord;
            GetComponent<Camera>().orthographicSize = tetrisCameraSize;
            //grid.transform.position = new Vector3(0, 0, 2);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && GameManager.gameState == GameManager.GameState.Tetris)
        {
            GameManager.gameState = GameManager.GameState.Ingame;
            GetComponent<Camera>().orthographicSize = inGameCameraSize;
            //grid.transform.position = new Vector3(0, 0, 0);
            GotoDestination();
        }
    }
}