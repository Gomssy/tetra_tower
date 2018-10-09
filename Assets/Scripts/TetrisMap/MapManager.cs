using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    /*
     * variables
     * */

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
    public float speed;
    /// <summary>
    /// Tetrimino falling gravity.
    /// </summary>
    public float gravity;
    public float collapseTime;
    public float fallTime;

    public static int height = 23, width = 10, realHeight = height - 4;

    /// <summary>
    /// Absolute coordinates on tetris map.
    /// </summary>
    public static Room[,] mapGrid = new Room[width, height];
    /// <summary>
    /// Tetris Y axis coordinates on Unity.
    /// </summary>
    public static float[] tetrisYCoord = new float[height];
    /// <summary>
    /// Current state of game.
    /// </summary>
    public bool gameOver = false;
    /// <summary>
    /// Choose to make a boss tetrimino or not.
    /// </summary>
    public bool spawnBossTetrimino = false;
    /// <summary>
    /// Tetris map.
    /// </summary>
    public GameObject tetrisMap;
    /// <summary>
    /// Current tetrimino waiting for falling.
    /// </summary>
    public Tetrimino currentTetrimino;
    /// <summary>
    /// List for the normal Room candidates.
    /// </summary>
    public RoomInGame[] normalRoomList;
    /// <summary>
    /// List for the item Room candidates.
    /// </summary>
    public RoomInGame[] itemRoomList;
    /// <summary>
    /// List for the special Room candidates.
    /// </summary>
    public RoomInGame[] specialRoomList;

    /*
     * functions
     * */
    /*
    /// <summary>
    /// Set coordinates to integer.
    /// Only use it for mapCoord adjustment.
    /// </summary>
    /// <param name="coord">Room's map coordinates.</param>
    /// <returns></returns>
    public static Vector3 AdjustMapCoord(Vector3 coord)
    {
        return new Vector3(Mathf.Round(coord.x), Mathf.Round(coord.y), coord.z);
    }
    */
    public void MoveTetriminoMapCoord(Tetrimino te, Vector3 coord)
    {
        for(int i = 0; i < te.rooms.Length; i++)
        {
            te.rooms[i].mapCoord += coord;
        }
        te.mapCoord += coord;
    }
    /// <summary>
    /// Check if tetrimino is in right x coordinate.
    /// </summary>
    /// <param name="te">-1 for over left end, 1 for over right end, 0 for right place.</param>
    /// <returns></returns>
    public int IsRightTetrimino(Tetrimino te)
    {
        for (int i = 0; i < te.rooms.Length; i++)
        {
            if (te.rooms[i].mapCoord.x < 0)
                return -1;
            else if (te.rooms[i].mapCoord.x > 9)
                return 1;
        }
        return 0;
    }
    /// <summary>
    /// Make Tetrimino in right X coordinate.
    /// </summary>
    /// <param name="te">Tetrimino.</param>
    public void MakeTetriminoRightPlace(Tetrimino te)
    {
        while (true)
        {
            Debug.Log(te.rooms[3].mapCoord);
            if (IsRightTetrimino(te) == 1)
            {
                Debug.Log("Move Left");
                MoveTetriminoMapCoord(te, new Vector3(-1, 0, 0));
            }
            else if (IsRightTetrimino(te) == -1)
            {
                Debug.Log("Move Right");
                MoveTetriminoMapCoord(te, new Vector3(1, 0, 0));
            }
            else
            {
                Debug.Log("return");
                return;
            }
        }
    }
    /// <summary>
    /// Delete one row.
    /// </summary>
    /// <param name="row">Rows wanted to be deleted.</param>
    public void DeleteRow(int row)
    {
        for(int x = 0; x < width; x++)
        {
            Destroy(mapGrid[x, row].gameObject);
            mapGrid[x, row] = null;
        }
    }
    /// <summary>
    /// Decrease all rows above this row.
    /// </summary>
    /// <param name="row">Row that will be underneath.</param>
    public void DecreaseRowsAbove(int row)
    {
        for(int y = row; y < realHeight; y++)
        {
            for(int x = 0; x < width; x++)
            {
                mapGrid[x, y - 1] = mapGrid[x, y];
                mapGrid[x, y] = null;
            }
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
            if (mapGrid[x, row] != null && mapGrid[x, row].specialRoomType == Room.SpecialRoomType.Boss)
                return false;
        return true;
    }
    /// <summary>
    /// Destroy Tetrimino that has no rooms.
    /// </summary>
    public static void DestroyParent()
    {
        GameObject[] tetriminoes = GameObject.FindGameObjectsWithTag("Tetrimino");
        foreach(GameObject child in tetriminoes)
        {
            if(child.transform.childCount == 0)
                Destroy(child.gameObject);
        }
    }
    /// <summary>
    /// Update rooms coordinates on mapCoord.
    /// </summary>
    /// <param name="te">Tetrimino you want to update on map.</param>
    public void UpdateMap(Tetrimino te)
    {
        
    }
    /// <summary>
    /// Display how much time is it remain to fall current tetrimino.
    /// </summary>
    /// <returns></returns>
    public IEnumerator DisplayTime()
    {
        yield return null;
    }



    public void TetriminoDown(Tetrimino te)
    {

    }
    public void GhostDown()
    {

    }
    public void TetriminoMove(Tetrimino te)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && GameManager.gameState == GameManager.GameState.Tetris)
        {
            MoveTetriminoMapCoord(te, new Vector3(-1, 0, 0));
            if (IsRightTetrimino(te) != 0)
                MoveTetriminoMapCoord(te, new Vector3(1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && GameManager.gameState == GameManager.GameState.Tetris)
        {
            MoveTetriminoMapCoord(te, new Vector3(1, 0, 0));
            if (IsRightTetrimino(te) != 0)
                MoveTetriminoMapCoord(te, new Vector3(-1, 0, 0));
        }
    }
    public void TetriminoRotate(Tetrimino te)
    {

    }
    public void SetRoomMapCoord(Tetrimino te)
    {
        for (int i = 0; i < te.rooms.Length; i++)
        {
            te.rooms[i].mapCoord = te.mapCoord + (te.rooms[i].transform.localPosition / tetrisMapSize);
        }
    }
    public void CreateRoom(Tetrimino te)
    {
        for (int i = 0; i < te.rooms.Length; i++)
        {
            te.rooms[i].transform.parent = grid;
            if (te.rooms[i].itemRoomType != 0) ;
            else if (te.rooms[i].specialRoomType != Room.SpecialRoomType.Normal)
                Instantiate(specialRoomList[(int)te.rooms[i].specialRoomType], te.rooms[i].transform.position, Quaternion.identity, te.rooms[i].transform);
            else
                Instantiate(normalRoomList[Random.Range(0, normalRoomList.Length)], te.rooms[i].transform.position, Quaternion.identity, te.rooms[i].transform);
        }
        Destroy(te.gameObject);
    }

    /*
     * Test
     * */

    public void Test()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            CreateRoom(currentTetrimino);
        }
    }



    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update() {
        Test();
        TetriminoMove(currentTetrimino);
        currentTetrimino.transform.position = currentTetrimino.mapCoord * tetrisMapSize + tetrisMapCoord;
    }
}
