using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    /*
     * variables
     * */

    TetriminoSpawner tetriminoSpawner;
    /// <summary>
    /// Grid showing tiles.
    /// </summary>
    public Transform grid;
    /// <summary>
    /// Tetris map's size.
    /// </summary>
    public float tetrisMapSize;
    /// <summary>
    /// Tetris map's coordinates.
    /// </summary>
    public Vector3 tetrisMapCoord;
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
    public const int height = 24, width = 10, realHeight = 19;
    /// <summary>
    /// Absolute coordinates on tetris map.
    /// </summary>
    public static Room[,] mapGrid = new Room[width, height];
    /// <summary>
    /// Current state of game.
    /// </summary>
    public bool gameOver;
    /// <summary>
    /// Check if tetrimino is falling.
    /// </summary>
    public bool isTetriminoFalling = false;
    /// <summary>
    /// Check if this row is being deleted.
    /// </summary>
    private static bool[] isRowDeleting = new bool[20];
    /// <summary>
    /// Tetris Y axis coordinates on Unity.
    /// </summary>
    private static float[] tetrisYCoord = new float[height];
    /// <summary>
    /// Array that saves presses.
    /// </summary>
    private Press[] presses = new Press[realHeight];
    /// <summary>
    /// Choose to make a boss tetrimino or not.
    /// </summary>
    public bool spawnBossTetrimino = false;
    /// <summary>
    /// Press.
    /// </summary>
    public Press press;
    /// <summary>
    /// Left door.
    /// </summary>
    public GameObject leftDoor;
    /// <summary>
    /// Right door.
    /// </summary>
    public GameObject rightDoor;
    /// <summary>
    /// Current tetrimino waiting for falling.
    /// </summary>
    public Tetrimino currentTetrimino;
    /// <summary>
    /// Current ghost following currentTetrimino.
    /// </summary>
    public Tetrimino currentGhost;
    /// <summary>
    /// Enum for special room types.
    /// </summary>
    public enum SpecialRoomType { Start, Item, BothSide, Gold, Amethyst, Boss, Normal };
    /// <summary>
    /// Array for the normal Room candidates.
    /// </summary>
    public RoomInGame[] normalRoomList;
    /// <summary>
    /// Array for the special Room candidates.
    /// </summary>
    public RoomInGame[] specialRoomList;
    /// <summary>
    /// Array sorted normal rooms by location of side doors.
    /// </summary>
    List<RoomInGame>[,] normalRoomsDistributed = new List<RoomInGame>[3, 3];
    /// <summary>
    /// First room player exists.
    /// </summary>
    public Room startRoom;
    /// <summary>
    /// Queue that saves rooms waiting for upgrade tetrimino.
    /// </summary>
    public Queue<SpecialRoomType> roomsWaiting = new Queue<SpecialRoomType>();

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
            {
                MoveTetriminoMapCoord(te, new Vector3(-1, 0, 0));
            }
            else if (IsRightTetrimino(te) == -1)
            {
                MoveTetriminoMapCoord(te, new Vector3(1, 0, 0));
            }
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
                Press leftPress = Instantiate(press, new Vector3(0, y * tetrisMapSize, 0), Quaternion.identity);
                Press rightPress = Instantiate(press, new Vector3(10 * tetrisMapSize, y * tetrisMapSize, 0), Quaternion.identity);
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
        while (Time.time - initialCollapseTime < collapseTime)
        {
            yield return new WaitForSeconds(0.01f);
            float collapseRate = (Time.time - initialCollapseTime) / collapseTime;
            leftPress.transform.localScale = new Vector3(collapseRate * 20, 1, 1);
            rightPress.transform.localScale = new Vector3(-collapseRate * 20, 1, 1);
        }
        int row = leftPress.row;
        for(int i = row + 1; i < realHeight; i++)
        {
            if(isRowDeleting[i])
            {
                presses[i].bottomRow = leftPress.bottomRow + i - row - 1;
                break;
            }
        }
        for (int x = 0; x < width; x++)
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
        {
            StartCoroutine(DecreaseYCoord(row, leftPress.bottomRow));
        }
        presses[row] = null;
        UpgradeRoom(SpecialRoomType.Item);
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
        }
        if (shakeCamera)
        {
            GameObject camera = GameObject.Find("Tetris Camera");
            StartCoroutine(CameraShake(5 * (top - bottom + 1), camera.transform.position, camera));
        }
        for (int i = 0; i < height; i++)
        {
            tetrisYCoord[i] = i * tetrisMapSize;
        }
        DecreaseRowsAbove(top, bottom);
    }
    /// <summary>
    /// Check row if it is full.
    /// </summary>
    /// <param name="row">Row you want to check.</param>
    /// <returns></returns>
    public static bool IsRowFull(int row)
    {
        for (int x = 0; x < width; x++)
            if (mapGrid[x, row] == null || mapGrid[x, row].specialRoomType == SpecialRoomType.Boss)
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
                gameOver = true;
                Debug.Log("Game Over");
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
        {
            for (int i = 0; i < te.rotatedPosition.Length; i++)
            {
                if (te.rotatedPosition[i] > 0)
                    te.rotatedPosition[i] += (int)coord.x;
            }
        }
        else if (te.rotatedPosition[te.rotatedAngle] + Tetrimino.rotationInformation[(int)te.tetriminoType].horizontalLength[te.rotatedAngle] < width && coord.x > 0)
        {
            for (int i = 0; i < te.rotatedPosition.Length; i++)
            {
                if (te.rotatedPosition[i] + Tetrimino.rotationInformation[(int)te.tetriminoType].horizontalLength[i] < width)
                    te.rotatedPosition[i] += (int)coord.x;
            }
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
        //controlCurrentTetrimino = false;
        while (IsRightTetrimino(te) == 0)
        {
            MoveTetriminoMapCoord(te, new Vector3(0, -1, 0));
        }
        MoveTetriminoMapCoord(te, new Vector3(0, 1, 0));
        initialFallTime = Time.time;
        StartCoroutine(TetriminoDown(te));
        //EndTetrimino(currentTetrimino);
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
        GameObject camera = GameObject.Find("Tetris Camera");
        StartCoroutine(CameraShake(20, camera.transform.position, camera));
        EndTetrimino(currentTetrimino);
    }
    /// <summary>
    /// Get ghost's position down.
    /// </summary>
    /// <param name="ghost">Which ghost to move.</param>
    /// <param name="te">Which tetrimino you'd like to sink with ghost.</param>
    public void GhostControl(Tetrimino ghost, Tetrimino te)
    {
        /*if(ghost.rotatedAngle != te.rotatedAngle)
            TetriminoRotate(ghost, te.rotatedAngle - ghost.rotatedAngle);*/
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
        if((Input.GetKeyDown(KeyCode.Space) && GameManager.gameState == GameManager.GameState.Tetris) || tetriminoWaitedTime > timeToFallTetrimino)
        {
            isTetriminoFalling = true;
            TetriminoMapCoordDown(currentTetrimino);
            //StartCoroutine(TetriminoDown(currentTetrimino));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && GameManager.gameState == GameManager.GameState.Tetris)
        {
            MoveTetriminoHorizontal(currentTetrimino, new Vector3(-1, 0, 0));
            /*MoveTetriminoMapCoord(te, new Vector3(-1, 0, 0));
            if (IsRightTetrimino(te) != 0)
                MoveTetriminoMapCoord(te, new Vector3(1, 0, 0));*/
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && GameManager.gameState == GameManager.GameState.Tetris)
        {
            MoveTetriminoHorizontal(currentTetrimino, new Vector3(1, 0, 0));
            /*MoveTetriminoMapCoord(te, new Vector3(1, 0, 0));
            if (IsRightTetrimino(te) != 0)
                MoveTetriminoMapCoord(te, new Vector3(-1, 0, 0));*/
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && GameManager.gameState == GameManager.GameState.Tetris)
        {
            TetriminoRotate(currentTetrimino, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && GameManager.gameState == GameManager.GameState.Tetris)
        {
            TetriminoRotate(currentTetrimino, -1);
        }
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
    /// Create rooms player will move.
    /// </summary>
    /// <param name="te">Tetrimino you want to create rooms.</param>
    public void CreateRoom(Tetrimino te)
    {
        for (int i = 0; i < te.rooms.Length; i++)
        {
            te.rooms[i].transform.parent = grid;
            te.rooms[i].transform.position += new Vector3(0, 0, -2);
            te.rooms[i].SetDoors();

            if (te.rooms[i].specialRoomType != SpecialRoomType.Normal)
            {
                Instantiate(specialRoomList[(int)te.rooms[i].specialRoomType], te.rooms[i].transform.position + new Vector3(0, 0, 2), Quaternion.identity, te.rooms[i].transform);
            }
            else
            {
                Instantiate(normalRoomsDistributed[te.rooms[i].leftDoorLocation, te.rooms[i].rightDoorLocation][Random.Range(0, normalRoomsDistributed[te.rooms[i].leftDoorLocation, te.rooms[i].rightDoorLocation].Count)],
                    te.rooms[i].transform.position + new Vector3(0, 0, 2), Quaternion.identity, te.rooms[i].transform);
                te.rooms[i].CreateDoors(leftDoor, rightDoor);
            }

        }
        Destroy(te.gameObject);
    }
    /// <summary>
    /// Upgrade rooms.
    /// </summary>
    /// <param name="roomType">Rooms you want to upgrade.</param>
    public void UpgradeRoom(SpecialRoomType roomType)
    {
        if (!isTetriminoFalling)
        {
            if (roomType != SpecialRoomType.Item && currentTetrimino.notNormalRoomCount < 4)
            {
                int randomRoom = Random.Range(0, currentTetrimino.rooms.Length);
                if (currentTetrimino.rooms[randomRoom].specialRoomType == SpecialRoomType.Normal)
                {
                    currentTetrimino.notNormalRoomCount++;
                    currentTetrimino.rooms[randomRoom].specialRoomType = roomType;
                    return;
                }
                else
                {
                    UpgradeRoom(roomType);
                    return;
                }
            }
            else if (roomType == SpecialRoomType.Item)
            {
                if(currentTetrimino.itemRoomIndex != -1)
                {
                    currentTetrimino.rooms[currentTetrimino.itemRoomIndex].itemRoomType++;
                    return;
                }
                else
                {
                    int randomRoom = Random.Range(0, currentTetrimino.rooms.Length);
                    currentTetrimino.notNormalRoomCount++;
                    currentTetrimino.itemRoomIndex = randomRoom;
                    currentTetrimino.rooms[randomRoom].specialRoomType = roomType;
                    currentTetrimino.rooms[randomRoom].itemRoomType++;
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
    /// Shake the camera when tetrimino has fallen.
    /// </summary>
    /// <param name="_amount">Amount you want to shake the camera.</param>
    /// <param name="originPos">Original position of the camera.</param>
    /// <param name="camera">Camera you want to shake.</param>
    /// <returns></returns>
    public IEnumerator CameraShake(float _amount, Vector3 originPos, GameObject camera)
    {
        float amount = _amount;
        while (amount > 0)
        {
            //transform.localPosition = (Vector3)Random.insideUnitCircle * amount + originPos;
            camera.transform.localPosition = new Vector3(0.2f * Random.insideUnitCircle.x * amount + originPos.x, Random.insideUnitCircle.y * amount + originPos.y, originPos.z);
            //transform.localPosition = new Vector3(Random.insideUnitCircle.x * amount + originPos.x, originPos.y, originPos.z);
            //transform.localPosition = new Vector3(originPos.x, Random.insideUnitCircle.y * amount + originPos.y, originPos.z);
            amount -= _amount / 40;
            yield return null;
        }
        camera.transform.localPosition = originPos;
        Debug.Log("sd");
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
        {
            tetrisYCoord[i] = i * tetrisMapSize;
        }
        for (int i = 0; i < isRowDeleting.Length; i++)
        {
            isRowDeleting[i] = false;
        }
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                normalRoomsDistributed[i, j] = new List<RoomInGame>();
        for(int i = 0; i < normalRoomList.Length; i++)
        {
            int leftDoor = normalRoomList[i].sideDoorInfo[0];
            int rightDoor = normalRoomList[i].sideDoorInfo[1];
            int leftCount = 0;
            int rightCount = 0;
            for(int left = leftDoor; left != 0; left /= 10)
            {
                for (int right = rightDoor; right != 0; right /= 10)
                {
                    if(left % 10 == 1 && right % 10 == 1)
                    {
                        Debug.Log(i + " : left " + leftCount + " right " + rightCount);
                        normalRoomsDistributed[leftCount, rightCount].Add(normalRoomList[i]);

                        //normalRoomsDistributed[leftCount, rightCount] = normalRoomList[i];
                    }
                    rightCount++;
                }
                rightCount = 0;
                leftCount++;
            }
            
        }

        tetriminoSpawner = GameObject.Find("TetriminoSpawner").GetComponent<TetriminoSpawner>();
    }
    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update() {
        if (!gameOver)
        {
            if(!isTetriminoFalling)
            {
                TetriminoControl(currentTetrimino);
                if(!isTetriminoFalling)
                    currentTetrimino.transform.position = new Vector3(currentTetrimino.mapCoord.x * tetrisMapSize, tetrisYCoord[(int)currentTetrimino.mapCoord.y], currentTetrimino.mapCoord.z * tetrisMapSize);
                if(currentGhost != null)
                {
                   GhostControl(currentGhost, currentTetrimino);
                   currentGhost.transform.position = new Vector3(currentGhost.mapCoord.x * tetrisMapSize, tetrisYCoord[(int)currentGhost.mapCoord.y], currentGhost.mapCoord.z * tetrisMapSize);
                }
            }
        }

    }
}
