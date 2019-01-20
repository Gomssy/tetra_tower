using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    /*
     * variables
     * */
    TetriminoSpawner tetriminoSpawner;
    public GameObject player;
    /// <summary>
    /// Grid showing tiles.
    /// </summary>
    public Transform grid;
    /// <summary>
    /// Tetris map's size.
    /// </summary>
    public static float tetrisMapSize = 24;
    /// <summary>
    /// Tetris map's coordinates.
    /// </summary>
    public static Vector3 tetrisMapCoord = new Vector3(0, 0, 0);
    /// <summary>
    /// Tetrimino falling speed.
    /// </summary>
    private float fallSpeed = -0.1f;
    /// <summary>
    /// Tetrimino's initial falling speed.
    /// </summary>
    public float initialFallSpeed = -0.1f;
    /// <summary>
    /// Tetrimino falling gravity.
    /// </summary>
    public float gravity = 0.98f;
    /// <summary>
    /// Time tetrimino would wait until it falls.
    /// </summary>
    public float timeToFallTetrimino = 100.0f;
    /// <summary>
    /// Time tetris waits to fall.
    /// </summary>
    public float tetriminoWaitedTime;
    /// <summary>
    /// Time tetris has created.
    /// </summary>
    public float tetriminoCreatedTime;
    /// <summary>
    /// Time Tetrimino has fallen.
    /// </summary>
    private float fallTime;
    /// <summary>
    /// Time tetrimino has started to fall.
    /// </summary>
    private float initialFallTime;
    /// <summary>
    /// The time taken for a press to be collapsed.
    /// </summary>
    public float collapseTime;
    /// <summary>
    /// Sizes of tetris map.
    /// </summary>
    public const int height = 24, width = 10, realHeight = 19;
    /// <summary>
    /// Absolute coordinates on tetris map.
    /// </summary>
    public static Room[,] mapGrid = new Room[width, height];
    /// <summary>
    /// Check if tetrimino is falling.
    /// </summary>
    public static bool isTetriminoFalling = false;
    /// <summary>
    /// Check if room is falling after room collapsed.
    /// </summary>
    public static bool isRoomFalling = false;
    /// <summary>
    /// Check if this row is being deleted.
    /// </summary>
    public static bool[] isRowDeleting = new bool[20];
    /// <summary>
    /// Tetris Y axis coordinates on Unity.
    /// </summary>
    public static float[] tetrisYCoord = new float[height];
    /// <summary>
    /// Array that saves presses.
    /// </summary>
    private Press[] presses = new Press[realHeight];
    /// <summary>
    /// Choose to make a boss tetrimino or not.
    /// </summary>
    public bool spawnBossTetrimino = false;
    /// <summary>
    /// Queue that saves rooms waiting for upgrade tetrimino.
    /// </summary>
    public Queue<RoomType> roomsWaiting = new Queue<RoomType>();
    /// <summary>
    /// Current tetrimino waiting for falling.
    /// </summary>
    public static Tetrimino currentTetrimino;
    /// <summary>
    /// Current ghost following currentTetrimino.
    /// </summary>
    public static Tetrimino currentGhost;
    /// <summary>
    /// Room player exists.
    /// Not related to player's real position, it also consider if player enter the room ordinarily.
    /// </summary>
    public static Room currentRoom;
    /// <summary>
    /// Temporary room player exists according to only position.
    /// </summary>
    public static Room tempRoom;
    /// <summary>
    /// Current stage of game.
    /// </summary>
    public static int currentStage;
    /// <summary>
    /// Fog of the rooms.
    /// </summary>
    public GameObject fog;
    /// <summary>
    /// Fog of the cleared rooms.
    /// </summary>
    public GameObject clearedFog;
    /// <summary>
    /// Press.
    /// </summary>
    public Press press;
    /// <summary>
    /// Left door in tetris.
    /// </summary>
    public GameObject leftDoor;
    /// <summary>
    /// Right door in tetris.
    /// </summary>
    public GameObject rightDoor;
    /// <summary>
    /// Up door in ingame.
    /// </summary>
    public GameObject inGameDoorUp;
    /// <summary>
    /// Down door in ingame.
    /// </summary>
    public GameObject inGameDoorDown;
    /// <summary>
    /// Left door in ingame.
    /// </summary>
    public GameObject inGameDoorLeft;
    /// <summary>
    /// Right door in ingame.
    /// </summary>
    public GameObject inGameDoorRight;

    public RoomInGame[] normalRoomList1;
    public RoomInGame[] normalRoomList2;
    /*public RoomInGame[] normalRoomList3;
    public RoomInGame[] normalRoomList4;
    public RoomInGame[] normalRoomList5;*/
    public RoomInGame[] specialRoomList1;
    public RoomInGame[] specialRoomList2;
    /*public RoomInGame[] specialRoomList3;
    public RoomInGame[] specialRoomList4;
    public RoomInGame[] specialRoomList5;*/
    /// <summary>
    /// Array sorted normal rooms by location of side doors.
    /// Each dimension for stage, concept, left door, right door.
    /// </summary>
    public List<RoomInGame>[,,,] normalRoomsDistributed = new List<RoomInGame>[5, 4, 3, 3];
    /// <summary>
    /// Array sorted normal rooms by location of side doors.
    /// Each dimension for stage, concept, left door, right door.
    /// </summary>
    public List<RoomInGame>[,,,] specialRoomsDistributed = new List<RoomInGame>[5, 4, 3, 3];

    public List<Sprite>[] roomsSpritesDistributed = new List<Sprite>[5];

    public Sprite[] roomsSprite1;
    public Sprite[] roomsSprite2;
    /*public Sprite[] roomsSprite3;
    public Sprite[] roomsSprite4;
    public Sprite[] roomsSprite5;*/

    /*
     * functions
     * */
    /// <summary>
    /// Check if tetrimino is in right x coordinate.
    /// </summary>
    /// <param name="te">-1 for over left end, 1 for over right end, 2 for over bottom end, 3 for already existing, 0 for right place.</param>
    /// <returns></returns>
    public int IsRightTetrimino(Tetrimino te)
    {
        for (int i = 0; i < te.rooms.Length; i++)
        {
            if (te.rooms[i].mapCoord.x < 0)
                return -1;
            else if (te.rooms[i].mapCoord.x > 9)
                return 1;
            else if (te.rooms[i].mapCoord.y < 0)
                return 2;
            else if (mapGrid[(int)te.rooms[i].mapCoord.x, (int)te.rooms[i].mapCoord.y] != null && mapGrid[(int)te.rooms[i].mapCoord.x, (int)te.rooms[i].mapCoord.y].transform.parent != te)
                return 3;
        }
        return 0;
    }   
    /// <summary>
    /// Check if ghost is at right place.
    /// </summary>
    /// <param name="te">Ghost to check</param>
    /// <returns>True for right place, false for wrong place</returns>
    public bool IsRightGhost(Tetrimino te)
    {
        for (int i = 0; i < te.rooms.Length; i++)
        {
            if (te.rooms[i].mapCoord.y < 0)
                return false;
            else if (MapManager.mapGrid[(int)te.rooms[i].mapCoord.x, (int)te.rooms[i].mapCoord.y] != null)
                return false;
        }
        return true;
    }
    /// <summary>
    /// Make Tetrimino in right X coordinate.
    /// </summary>
    /// <param name="te">Tetrimino.</param>
    public void MakeTetriminoRightPlace(Tetrimino te)
    {
        while (true)
        {
            if (IsRightTetrimino(te) == 1)
                MoveTetriminoMapCoord(te, new Vector3(-1, 0, 0));
            else if (IsRightTetrimino(te) == -1)
                MoveTetriminoMapCoord(te, new Vector3(1, 0, 0));
            else
                return;
        }
    }
    /// <summary>
    /// Find full rows and create presses.
    /// </summary>
    public void DeleteFullRows()
    {
        int order = 0;
        ArrayList simultaneousPress = new ArrayList(); 
        for (int y = 0; y < realHeight; y++)
        {
            if (IsRowFull(y) && !isRowDeleting[y])
            {
                isRowDeleting[y] = true;
                Press leftPress = Instantiate(press, new Vector3(0, y * tetrisMapSize, 2), Quaternion.identity);
                Press rightPress = Instantiate(press, new Vector3(10 * tetrisMapSize, y * tetrisMapSize, 2), Quaternion.identity);
                leftPress.initialCollapseTime = Time.time;
                rightPress.initialCollapseTime = Time.time;
                leftPress.isLeft = true;
                rightPress.isLeft = false;
                leftPress.row = y;
                leftPress.bottomRow = y;
                leftPress.createdOrder = order;
                presses[y] = leftPress;
                simultaneousPress.Add(leftPress);
                StartCoroutine(TetrisPress(leftPress.initialCollapseTime, leftPress, rightPress));
                order++;
            }
        }
        foreach(Press child in simultaneousPress)
        {
            child.simultaneouslyCreatedPressNumber = order - 1;
        }
    }
    /// <summary>
    /// Extend and collapse press.
    /// When other presses are remain, than reduce their bottom rows.
    /// </summary>
    /// <param name="initialCollapseTime">Initial time that collapse has started.</param>
    /// <param name="leftPress">Left press.</param>
    /// <param name="rightPress">Right press.</param>
    /// <returns></returns>
    public IEnumerator TetrisPress(float initialCollapseTime, Press leftPress, Press rightPress)
    {
        int doorCloseCounter = 0;
        int roomDestroyCounter = 0;
        int row = leftPress.row;
        while (Time.time - initialCollapseTime < collapseTime)
        {
            yield return new WaitForSeconds(0.01f);
            float collapseRate = (Time.time - initialCollapseTime) / collapseTime;
            leftPress.transform.localScale = new Vector3(collapseRate * 20, 1, 1);
            rightPress.transform.localScale = new Vector3(-collapseRate * 20, 1, 1);
            if(collapseRate - doorCloseCounter * 0.2f > (float)1 / 12)
            {
                mapGrid[doorCloseCounter, row].CloseDoor("Up", false);
                mapGrid[doorCloseCounter, row].CloseDoor("Down", false);
                mapGrid[width - doorCloseCounter - 1, row].CloseDoor("Up", false);
                mapGrid[width - doorCloseCounter - 1, row].CloseDoor("Down", false);
                mapGrid[doorCloseCounter, row].isRoomDestroyed = true;
                mapGrid[width - doorCloseCounter - 1, row].isRoomDestroyed = true;
                doorCloseCounter++;
            }
            if(collapseRate - roomDestroyCounter * 0.2f > 0.2f)
            {
                if(mapGrid[roomDestroyCounter, row] == currentRoom || mapGrid[width - roomDestroyCounter - 1, row] == currentRoom)
                {
                    GameManager.gameState = GameState.GameOver;
                }
                //Destroy(mapGrid[roomDestroyCounter, row].gameObject);
                //Destroy(mapGrid[width - roomDestroyCounter - 1, row].gameObject);
                roomDestroyCounter++;
            }
        }
        for(int i = row + 1; i < realHeight; i++)
        {
            if(isRowDeleting[i])
            {
                presses[i].bottomRow = leftPress.bottomRow + i - row - 1;
                break;
            }
        }
        for(int x = 0; x < width; x++)
        {
            Destroy(mapGrid[x, row].gameObject);
            mapGrid[x, row] = null;
        }
        while (leftPress.transform.localScale.x > 1)
        {
            yield return new WaitForSeconds(0.01f);
            leftPress.transform.localScale -= new Vector3(3, 0, 0);
            rightPress.transform.localScale -= new Vector3(-3, 0, 0);
        }
        isRowDeleting[row] = false;
        if (leftPress.createdOrder == leftPress.simultaneouslyCreatedPressNumber)
            StartCoroutine(DecreaseYCoord(row, leftPress.bottomRow));
        presses[row] = null;
        UpgradeRoom(RoomType.Item);
        Destroy(leftPress.gameObject);
        Destroy(rightPress.gameObject);
    }
    /// <summary>
    /// Decrease all empty rows between top row and bottom row.
    /// Changes mapCoord.
    /// </summary>
    /// <param name="top">Top end of the empty rows.</param>
    /// <param name="bottom">Bottom end of the empty rows.</param>
    public void DecreaseRowsAbove(int top, int bottom)
    {
        for(int i = bottom; i <= top; i++)
        {
            for(int y = bottom; !isRowDeleting[y] && y < realHeight; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    if (y > 0 && mapGrid[x, y] != null && mapGrid[x, y].transform.parent)
                    {
                        mapGrid[x, y - 1] = mapGrid[x, y];
                        mapGrid[x, y] = null;
                        mapGrid[x, y - 1].mapCoord += new Vector3(0, -1, 0);
                    }
                }
            }
        }
        SetRoomsYCoord();
    }
    /// <summary>
    /// Set all rooms' mapCoord to tetrisYCoord.
    /// </summary>
    public void SetRoomsYCoord()
    {
        for (int y = 0; y < realHeight; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (mapGrid[x, y] != null && mapGrid[x, y].transform.parent)
                {
                    Vector3 coord = mapGrid[x, y].mapCoord;
                    mapGrid[x, y].transform.position = new Vector3(coord.x * tetrisMapSize, tetrisYCoord[(int)coord.y], mapGrid[x, y].transform.position.z);
                }
            }
        }
    }
    /// <summary>
    /// Decrease tetris' y coord.
    /// Changes real position.
    /// </summary>
    /// <param name="top">Top end of the empty row.</param>
    /// <param name="bottom">Bottom end of the empty row.</param>
    /// <returns></returns>
    public IEnumerator DecreaseYCoord(int top, int bottom)
    {
        float yInitialTime = Time.time;
        float yFallTime = 0, yFallSpeed = 0;
        int row = 0;
        bool shakeCamera = true;
        for(int i = 0; i < realHeight; i++)
        {
            if (IsRowEmpty(i))
            {
                row = i + 1;
                break;
            }
        }
        isRoomFalling = true;
        Vector3 previousPlayerRelativePosition = player.transform.position - currentRoom.transform.position;
        while (tetrisYCoord[top + 1] > bottom * tetrisMapSize)
        {
            yield return new WaitForSeconds(0.01f);
            if (isRowDeleting[top + 1])
            {
                shakeCamera = false;
                break;
            }
            yFallTime = Time.time - yInitialTime;
            yFallSpeed += gravity * yFallTime * yFallTime;
            for(int i = row; !isRowDeleting[i] && i < realHeight; i++)
            {
                if(tetrisYCoord[i] > 0 && tetrisYCoord[i] > tetrisYCoord[i - 1])
                    tetrisYCoord[i] -= yFallSpeed;
            }
            SetRoomsYCoord();
            if(currentRoom.mapCoord.y >= bottom)
                player.transform.position += new Vector3(0, - yFallSpeed, 0);
            previousPlayerRelativePosition = player.transform.position - currentRoom.transform.position;
        }
        if (shakeCamera)
        {
            Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            StartCoroutine(camera.GetComponent<CameraController>().CameraShake(5 * (top - bottom + 1) / CameraController.tetrisCameraSize));
        }
        for (int i = 0; i < height; i++)
        {
            tetrisYCoord[i] = i * tetrisMapSize;
        }
        DecreaseRowsAbove(top, bottom);
        player.transform.position = currentRoom.transform.position + previousPlayerRelativePosition;
        isRoomFalling = false;
        for (int i = 0; i < width; i++)
        {
            if (bottom > 0 && mapGrid[i, bottom] != null && mapGrid[i, bottom].isRoomCleared == true)
                mapGrid[i, bottom].OpenDoor("Down");
            else if (bottom > 0 && mapGrid[i, bottom - 1] != null && mapGrid[i, bottom - 1].isRoomCleared == true)
                mapGrid[i, bottom - 1].OpenDoor("Up");
        }
    }
    /// <summary>
    /// Check row if it is full.
    /// </summary>
    /// <param name="row">Row you want to check.</param>
    /// <returns></returns>
    public static bool IsRowFull(int row)
    {
        for (int x = 0; x < width; x++)
            if (mapGrid[x, row] == null || mapGrid[x, row].specialRoomType == RoomType.Boss)
                return false;
        return true;
    }
    /// <summary>
    /// Check row if it is empty.
    /// </summary>
    /// <param name="row">Row you want to check.</param>
    /// <returns></returns>
    public static bool IsRowEmpty(int row)
    {
        for (int x = 0; x < width; x++)
            if (mapGrid[x, row] != null)
                return false;
        return true;
    }
    /// <summary>
    /// Update rooms coordinates on mapCoord.
    /// </summary>
    /// <param name="te">Tetrimino you want to update on map.</param>
    public void UpdateMap(Tetrimino te)
    {
        for(int i = 0; i < te.rooms.Length; i++)
        {
            if ((int)te.rooms[i].mapCoord.y > 19)
            {
                GameManager.gameState = GameState.GameOver;
                return;
            }
            mapGrid[(int)te.rooms[i].mapCoord.x, (int)te.rooms[i].mapCoord.y] = te.rooms[i];
        }
    }
    /// <summary>
    /// Display how much time is it remain to fall current tetrimino.
    /// </summary>
    /// <returns></returns>
    public IEnumerator CountTetriminoWaitingTime()
    {
        while (!isTetriminoFalling)
        {
            yield return new WaitForSeconds(0.1f);
            tetriminoWaitedTime = Time.time - tetriminoCreatedTime;
        }
    }
    /// <summary>
    /// Move tetrimino horizontally.
    /// </summary>
    /// <param name="te"></param>
    /// <param name="coord"></param>
    public void MoveTetriminoHorizontal(Tetrimino te, Vector3 coord)
    {
        if (te.rotatedPosition[te.rotatedAngle] > 0 && coord.x < 0)
            for (int i = 0; i < te.rotatedPosition.Length; i++)
            {
                if (te.rotatedPosition[i] > 0)
                    te.rotatedPosition[i] += (int)coord.x;
            }
        else if (te.rotatedPosition[te.rotatedAngle] + Tetrimino.rotationInformation[(int)te.tetriminoType].horizontalLength[te.rotatedAngle] < width && coord.x > 0)
            for (int i = 0; i < te.rotatedPosition.Length; i++)
            {
                if (te.rotatedPosition[i] + Tetrimino.rotationInformation[(int)te.tetriminoType].horizontalLength[i] < width)
                    te.rotatedPosition[i] += (int)coord.x;
            }
        te.mapCoord = new Vector3(te.rotatedPosition[te.rotatedAngle], te.mapCoord.y, te.mapCoord.z);
        SetRoomMapCoord(te);
    }
    /// <summary>
    /// Rotate tetrimino.
    /// </summary>
    /// <param name="te"></param>
    /// <param name="i">1 for clockwise, -1 for counter clockwise.</param>
    public void TetriminoRotate(Tetrimino te, int direction)
    {
        if (direction == 1)
        {
            for (int i = 0; i < te.rooms.Length; i++)
            {
                Vector3 tempCoord = (te.rooms[i].mapCoord - te.mapCoord) - new Vector3(1.5f, 1.5f, 0);
                te.rooms[i].mapCoord = new Vector3(tempCoord.y, -tempCoord.x, tempCoord.z) + new Vector3(1.5f, 1.5f, 0) + te.mapCoord;
            }
            if (te.rotatedAngle != 3)
                te.rotatedAngle++;
            else
                te.rotatedAngle = 0;
        }
        else if (direction == -1)
        {
            for (int i = 0; i < te.rooms.Length; i++)
            {
                Vector3 tempCoord = (te.rooms[i].mapCoord - te.mapCoord) - new Vector3(1.5f, 1.5f, 0);
                te.rooms[i].mapCoord = new Vector3(-tempCoord.y, tempCoord.x, tempCoord.z) + new Vector3(1.5f, 1.5f, 0) + te.mapCoord;
            }
            if (te.rotatedAngle != 0)
                te.rotatedAngle--;
            else
                te.rotatedAngle = 3;
        }
        float minX = te.rooms[0].mapCoord.x, minY = te.rooms[0].mapCoord.y;
        for (int i = 0; i < te.rooms.Length; i++)
        {
            if (te.rooms[i].mapCoord.x < minX)
                minX = te.rooms[i].mapCoord.x;
            if (te.rooms[i].mapCoord.y < minY)
                minY = te.rooms[i].mapCoord.y;
        }
        te.mapCoord = new Vector3(te.rotatedPosition[te.rotatedAngle], te.mapCoord.y, te.mapCoord.z);
        for (int i = 0; i < te.rooms.Length; i++)
        {
            //te.rooms[i].mapCoord += new Vector3(-(minX - te.mapCoord.x), -(minY - te.mapCoord.y), 0);
            te.rooms[i].mapCoord += new Vector3(-(minX - te.mapCoord.x), -(minY - te.mapCoord.y), 0);
            te.rooms[i].transform.position = (te.rooms[i].mapCoord - te.mapCoord) * tetrisMapSize + te.transform.position;
        }
    }
    /// <summary>
    /// Move tetrimino as the amount of coord.
    /// </summary>
    /// <param name="te">Which tetrimino to move</param>
    /// <param name="coord">How much will tetrimino move.</param>
    public void MoveTetriminoMapCoord(Tetrimino te, Vector3 coord)
    {
        for (int i = 0; i < te.rooms.Length; i++)
        {
            te.rooms[i].mapCoord += coord;
        }
        te.mapCoord += coord;
    }
    /// <summary>
    /// Get tetrimino's mapCoord down.
    /// </summary>
    /// <param name="te">Which tetrimino to move.</param>
    public void TetriminoMapCoordDown(Tetrimino te)
    {
        while (IsRightTetrimino(te) == 0)
        {
            MoveTetriminoMapCoord(te, new Vector3(0, -1, 0));
        }
        MoveTetriminoMapCoord(te, new Vector3(0, 1, 0));
        initialFallTime = Time.time;
        StartCoroutine(TetriminoDown(te));
    }
    /// <summary>
    /// End tetrimino's falling and make rooms and new tetrimino.
    /// </summary>
    /// <param name="te">Tetrimino you want to end.</param>
    public void EndTetrimino(Tetrimino te)
    {
        te.transform.position = new Vector3(te.mapCoord.x * tetrisMapSize, tetrisYCoord[(int)te.mapCoord.y], te.mapCoord.z * tetrisMapSize);
        fallSpeed = initialFallSpeed;
        tetriminoWaitedTime = 0;
        UpdateMap(te);
        CreateRoom(te);
        DeleteFullRows();
        Destroy(currentGhost.gameObject);
        StartCoroutine(MakeNextTetrimino());
    }
    /// <summary>
    /// Get tetrimino's position down.
    /// </summary>
    /// <param name="te">Which tetrimino to move.</param>
    public IEnumerator TetriminoDown(Tetrimino te)
    {
        while(te.transform.position.y > tetrisYCoord[(int)te.mapCoord.y])
        {
            yield return new WaitForSeconds(0.01f);
            fallTime = Time.time - initialFallTime;
            fallSpeed += gravity * fallTime * fallTime;
            te.transform.position += new Vector3(0, -fallSpeed, 0);
        }
        StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().CameraShake(20 / CameraController.tetrisCameraSize));
        EndTetrimino(te);
    }
    /// <summary>
    /// Get ghost's position down.
    /// </summary>
    /// <param name="ghost">Which ghost to move.</param>
    /// <param name="te">Which tetrimino you'd like to sink with ghost.</param>
    public void GhostControl(Tetrimino ghost, Tetrimino te)
    {
        currentGhost.mapCoord = currentTetrimino.mapCoord;
        for (int i = 0; i < currentGhost.rooms.Length; i++)
        {
            currentGhost.rooms[i].mapCoord = currentTetrimino.rooms[i].mapCoord;
            currentGhost.rooms[i].transform.position = (currentGhost.rooms[i].mapCoord - currentGhost.mapCoord) * tetrisMapSize + currentGhost.transform.position;
        }
        while (IsRightGhost(ghost))
        {
            MoveTetriminoMapCoord(ghost, new Vector3(0, -1, 0));
        }
        MoveTetriminoMapCoord(ghost, new Vector3(0, 1, 0));
    }
    /// <summary>
    /// Press Left arrow/Right arrow to move left/right, Space to drop.
    /// </summary>
    /// <param name="te">Tetrimino you want to move.</param>
    public void TetriminoControl(Tetrimino te)
    {
        if((Input.GetKeyDown(KeyCode.Space) && GameManager.gameState == GameState.Tetris) || tetriminoWaitedTime > timeToFallTetrimino)
        {
            isTetriminoFalling = true;
            TetriminoMapCoordDown(currentTetrimino);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && GameManager.gameState == GameState.Tetris)
            MoveTetriminoHorizontal(currentTetrimino, new Vector3(-1, 0, 0));
        else if (Input.GetKeyDown(KeyCode.RightArrow) && GameManager.gameState == GameState.Tetris)
            MoveTetriminoHorizontal(currentTetrimino, new Vector3(1, 0, 0));
        else if (Input.GetKeyDown(KeyCode.UpArrow) && GameManager.gameState == GameState.Tetris)
            TetriminoRotate(currentTetrimino, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow) && GameManager.gameState == GameState.Tetris)
            TetriminoRotate(currentTetrimino, -1);
    }
    /// <summary>
    /// Set rooms' mapCoord on this tetrimino.
    /// </summary>
    /// <param name="te">Tetrimino you want to set rooms' mapCoord.</param>
    public void SetRoomMapCoord(Tetrimino te)
    {
        for (int i = 0; i < te.rooms.Length; i++)
        {
            te.rooms[i].mapCoord = te.mapCoord + (te.rooms[i].transform.localPosition / tetrisMapSize);
        }
    }
    /// <summary>
    /// Set rooms' sprite.
    /// </summary>
    /// <param name="room">Room to set sprite.</param>
    /// <param name="i">Index of the room in tetrimino. Use this to set ghost room's sprite.</param>
    public void SetRoomSprite(Room room, int i)
    {
        //When start and special rooms' sprites added, modify this.
        if (room.specialRoomType == RoomType.Normal)
            room.GetComponent<SpriteRenderer>().sprite = roomsSpritesDistributed[room.stage][(int)RoomSpriteType.Normal1 + room.roomConcept];
        else
            room.GetComponent<SpriteRenderer>().sprite = roomsSpritesDistributed[room.stage][(int)room.specialRoomType];



        if (currentGhost != null)
            currentGhost.rooms[i].GetComponent<SpriteRenderer>().sprite = room.GetComponent<SpriteRenderer>().sprite;
    }
    /// <summary>
    /// Create rooms player will move.
    /// </summary>
    /// <param name="te">Tetrimino you want to create rooms.</param>
    public void CreateRoom(Tetrimino te)
    {
        Room room;
        for (int i = 0; i < te.rooms.Length; i++)
        {
            room = te.rooms[i];
            room.transform.parent = grid;
            if(GameManager.gameState == GameState.Ingame)
                room.transform.localPosition += new Vector3(0, 0, -2);
            room.SetDoors();
            int left = room.leftDoorLocation;
            int right = room.rightDoorLocation;
            if (room.specialRoomType == RoomType.Normal)
                room.roomInGame = Instantiate(normalRoomsDistributed[room.stage, room.roomConcept, left, right]
                    [Random.Range(0, normalRoomsDistributed[room.stage, room.roomConcept, left, right].Count)], room.transform.position + new Vector3(0, 0, 2), Quaternion.identity, room.transform);
            else
                room.roomInGame = Instantiate(specialRoomsDistributed[room.stage, room.roomConcept, left, right]
                    [(int)room.specialRoomType], room.transform.position + new Vector3(0, 0, 2), Quaternion.identity, room.transform);
            room.CreateDoors(leftDoor, rightDoor, inGameDoorUp, inGameDoorDown, inGameDoorLeft, inGameDoorRight);
            room.fog = Instantiate(fog, room.transform.position + new Vector3(12, 12, 2), Quaternion.identity, room.transform);
            if (room.mapCoord.y > 0 && mapGrid[(int)room.mapCoord.x, (int)room.mapCoord.y - 1] != null && mapGrid[(int)room.mapCoord.x, (int)room.mapCoord.y - 1].isRoomCleared == true)
                room.OpenDoor("Down");
            if (room.mapCoord.x > 0 && mapGrid[(int)room.mapCoord.x - 1, (int)room.mapCoord.y] != null && mapGrid[(int)room.mapCoord.x - 1, (int)room.mapCoord.y].isRoomCleared == true)
                room.OpenDoor("Left");
            if (room.mapCoord.x < width - 1 && mapGrid[(int)room.mapCoord.x + 1, (int)room.mapCoord.y] != null && mapGrid[(int)room.mapCoord.x + 1, (int)room.mapCoord.y].isRoomCleared == true)
                room.OpenDoor("Right");
        }
        Destroy(te.gameObject);
    }
    /// <summary>
    /// Upgrade rooms.
    /// </summary>
    /// <param name="roomType">Rooms you want to upgrade.</param>
    public void UpgradeRoom(RoomType roomType)
    {
        if (!isTetriminoFalling)
        {
            if (roomType != RoomType.Item && currentTetrimino.notNormalRoomCount < 4)
            {
                int randomRoom = Random.Range(0, currentTetrimino.rooms.Length);
                if (currentTetrimino.rooms[randomRoom].specialRoomType == RoomType.Normal)
                {
                    currentTetrimino.notNormalRoomCount++;
                    currentTetrimino.rooms[randomRoom].specialRoomType = roomType;
                    SetRoomSprite(currentTetrimino.rooms[randomRoom], randomRoom);
                    return;
                }
                else
                {
                    UpgradeRoom(roomType);
                    return;
                }
            }
            else if (roomType == RoomType.Item)
            {
                if(currentTetrimino.itemRoomIndex != -1)
                {
                    currentTetrimino.rooms[currentTetrimino.itemRoomIndex].itemRoomType++;
                    SetRoomSprite(currentTetrimino.rooms[currentTetrimino.itemRoomIndex], currentTetrimino.itemRoomIndex);
                    return;
                }
                else
                {
                    int randomRoom = Random.Range(0, currentTetrimino.rooms.Length);
                    currentTetrimino.notNormalRoomCount++;
                    currentTetrimino.itemRoomIndex = randomRoom;
                    currentTetrimino.rooms[randomRoom].specialRoomType = roomType;
                    currentTetrimino.rooms[randomRoom].itemRoomType++;
                    SetRoomSprite(currentTetrimino.rooms[randomRoom], randomRoom);
                    return;
                }
            }
        }
        roomsWaiting.Enqueue(roomType);
    }
    /// <summary>
    /// Wait for one second and make a new tetrimino.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MakeNextTetrimino()
    {
        yield return new WaitForSeconds(1f);
        tetriminoSpawner.MakeTetrimino();
    }
    /// <summary>
    /// Make room fade in.
    /// </summary>
    /// <param name="room">Room you want to fade in.</param>
    /// <returns></returns>
    public IEnumerator RoomFadeIn(Room room)
    {
        float alpha = 1;
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.01f);
            room.leftTetrisDoor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            room.rightTetrisDoor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            room.fog.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            alpha -= 0.05f;
        }
        room.leftTetrisDoor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        room.rightTetrisDoor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        room.fog.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
    /// <summary>
    /// Make room fade out.
    /// </summary>
    /// <param name="room">Room you want to fade out.</param>
    /// <returns></returns>
    public IEnumerator RoomFadeOut(Room room)
    {
        float alpha = 0;
        for(int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.01f);
            room.leftTetrisDoor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            room.rightTetrisDoor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            room.fog.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            alpha += 0.05f;
        }
        room.leftTetrisDoor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        room.rightTetrisDoor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        room.fog.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    void Awake()
    {
        Tetrimino.rotationInformation[0].horizontalLength = new int[4] { 1, 4, 1, 4 };  //I
        Tetrimino.rotationInformation[1].horizontalLength = new int[4] { 2, 2, 2, 2 };  //O
        Tetrimino.rotationInformation[2].horizontalLength = new int[4] { 3, 2, 3, 2 };  //T
        Tetrimino.rotationInformation[3].horizontalLength = new int[4] { 2, 3, 2, 3 };  //J
        Tetrimino.rotationInformation[4].horizontalLength = new int[4] { 2, 3, 2, 3 };  //L
        Tetrimino.rotationInformation[5].horizontalLength = new int[4] { 3, 2, 3, 2 };  //S
        Tetrimino.rotationInformation[6].horizontalLength = new int[4] { 3, 2, 3, 2 };  //Z
        Tetrimino.rotationInformation[7].horizontalLength = new int[4] { 1, 1, 1, 1 };  //Boss
        for (int i = 0; i < tetrisYCoord.Length; i++)
            tetrisYCoord[i] = i * tetrisMapSize;
        for (int i = 0; i < isRowDeleting.Length; i++)
            isRowDeleting[i] = false;
        for (int stage = 0; stage < 5; stage++)
        {
            roomsSpritesDistributed[stage] = new List<Sprite>();
            for (int concept = 0; concept < 4; concept++)
                for (int leftDoor = 0; leftDoor < 3; leftDoor++)
                    for (int rightDoor = 0; rightDoor < 3; rightDoor++)
                    {
                        normalRoomsDistributed[stage, concept, leftDoor, rightDoor] = new List<RoomInGame>();
                        specialRoomsDistributed[stage, concept, leftDoor, rightDoor] = new List<RoomInGame>();
                    }
        }
        for (int concept = 0; concept < 4; concept++)
            for (int leftDoor = 0; leftDoor < 3; leftDoor++)
                for (int rightDoor = 0; rightDoor < 3; rightDoor++)
                {
                    for (int i = 0; i < normalRoomList1.Length; i++)
                        if (normalRoomList1[i].leftDoorInfo[leftDoor] == true && normalRoomList1[i].rightDoorInfo[rightDoor] == true && normalRoomList1[i].concept[concept] == true)
                            normalRoomsDistributed[0, concept, 2 - leftDoor, 2 - rightDoor].Add(normalRoomList1[i]);
                    for (int i = 0; i < normalRoomList2.Length; i++)
                        if (normalRoomList2[i].leftDoorInfo[leftDoor] == true && normalRoomList2[i].rightDoorInfo[rightDoor] == true && normalRoomList2[i].concept[concept] == true)
                            normalRoomsDistributed[1, concept, 2 - leftDoor, 2 - rightDoor].Add(normalRoomList2[i]);
                    /*for (int i = 0; i < normalRoomList3.Length; i++)
                        if (normalRoomList3[i].leftDoorInfo[leftDoor] == true && normalRoomList3[i].rightDoorInfo[rightDoor] == true && normalRoomList3[i].concept[concept] == true)
                            normalRoomsDistributed[2, concept, 2 - leftDoor, 2 - rightDoor].Add(normalRoomList1[i]);
                    for (int i = 0; i < normalRoomList4.Length; i++)
                        if (normalRoomList4[i].leftDoorInfo[leftDoor] == true && normalRoomList4[i].rightDoorInfo[rightDoor] == true && normalRoomList4[i].concept[concept] == true)
                            normalRoomsDistributed[3, concept, 2 - leftDoor, 2 - rightDoor].Add(normalRoomList1[i]);
                    for (int i = 0; i < normalRoomList5.Length; i++)
                        if (normalRoomList5[i].leftDoorInfo[leftDoor] == true && normalRoomList5[i].rightDoorInfo[rightDoor] == true && normalRoomList5[i].concept[concept] == true)
                            normalRoomsDistributed[4, concept, 2 - leftDoor, 2 - rightDoor].Add(normalRoomList1[i]);*/
                    for (int i = 0; i < specialRoomList1.Length; i++)
                        if (specialRoomList1[i].leftDoorInfo[leftDoor] == true && specialRoomList1[i].rightDoorInfo[rightDoor] == true && specialRoomList1[i].concept[concept] == true)
                            specialRoomsDistributed[0, concept, 2 - leftDoor, 2 - rightDoor].Add(specialRoomList1[i]);
                    for (int i = 0; i < specialRoomList2.Length; i++)
                        if (specialRoomList2[i].leftDoorInfo[leftDoor] == true && specialRoomList2[i].rightDoorInfo[rightDoor] == true && specialRoomList2[i].concept[concept] == true)
                            specialRoomsDistributed[1, concept, 2 - leftDoor, 2 - rightDoor].Add(specialRoomList2[i]);
                    /*for (int i = 0; i < specialRoomList3.Length; i++)
                        if (specialRoomList3[i].leftDoorInfo[leftDoor] == true && specialRoomList3[i].rightDoorInfo[rightDoor] == true && specialRoomList3[i].concept[concept] == true)
                            specialRoomsDistributed[2, concept, 2 - leftDoor, 2 - rightDoor].Add(specialRoomList3[i]);
                    for (int i = 0; i < specialRoomList4.Length; i++)
                        if (specialRoomList4[i].leftDoorInfo[leftDoor] == true && specialRoomList4[i].rightDoorInfo[rightDoor] == true && specialRoomList4[i].concept[concept] == true)
                            specialRoomsDistributed[3, concept, 2 - leftDoor, 2 - rightDoor].Add(specialRoomList4[i]);
                    for (int i = 0; i < specialRoomList5.Length; i++)
                        if (specialRoomList5[i].leftDoorInfo[leftDoor] == true && specialRoomList5[i].rightDoorInfo[rightDoor] == true && specialRoomList5[i].concept[concept] == true)
                            specialRoomsDistributed[4, concept, 2 - leftDoor, 2 - rightDoor].Add(specialRoomList5[i]);*/
                }
        roomsSpritesDistributed[0].Add(roomsSprite1[0]);
        for (RoomSpriteType spriteType = 0; (int)spriteType < 10; spriteType++)
        {
            roomsSpritesDistributed[0].Add(roomsSprite1[(int)spriteType + 1]);
            roomsSpritesDistributed[1].Add(roomsSprite2[(int)spriteType]);
            /*roomsSpritesDistributed[2].Add(roomsSprite3[(int)spriteType]);
            roomsSpritesDistributed[3].Add(roomsSprite4[(int)spriteType]);
            roomsSpritesDistributed[4].Add(roomsSprite5[(int)spriteType]);*/
        }
        tetriminoSpawner = GameObject.Find("TetriminoSpawner").GetComponent<TetriminoSpawner>();
        currentStage = 0;
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update() {
        if (GameManager.gameState != GameState.GameOver)
        {
            if (!isTetriminoFalling)
            {
                TetriminoControl(currentTetrimino);
                if (!isTetriminoFalling)
                    currentTetrimino.transform.position = new Vector3(currentTetrimino.mapCoord.x * tetrisMapSize, tetrisYCoord[(int)currentTetrimino.mapCoord.y], currentTetrimino.mapCoord.z);
                if (currentGhost != null)
                {
                    GhostControl(currentGhost, currentTetrimino);
                    currentGhost.transform.position = new Vector3(currentGhost.mapCoord.x * tetrisMapSize, tetrisYCoord[(int)currentGhost.mapCoord.y], currentGhost.mapCoord.z);
                }
            }
        }
        else
            Debug.Log("Game Over");
    }
}
