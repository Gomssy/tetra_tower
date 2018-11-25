﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    /*
     * variables
     * */

    TetriminoSpawner TS;
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
    public float fallSpeed = -0.1f;
    public float initialFallSpeed = -0.1f;
    /// <summary>
    /// Tetrimino falling gravity.
    /// </summary>
    public float gravity = 0.98f;
    public float collapseTime;
    public float fallTime;
    float initialFallTime;

    public static int height = 24, width = 10, realHeight = height - 5;

    /// <summary>
    /// Absolute coordinates on tetris map.
    /// </summary>
    public static Room[,] mapGrid = new Room[width, height];
    /// <summary>
    /// Current state of game.
    /// </summary>
    public bool gameOver = false;
    /// <summary>
    /// Check if tetrimino is falling.
    /// </summary>
    public bool isTetriminoFalling = false;
    /// <summary>
    /// Tetris Y axis coordinates on Unity.
    /// </summary>
    public static float[] tetrisYCoord = new float[height];
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
    /// Current ghost following currentTetrimino.
    /// </summary>
    public Tetrimino currentGhost;
    /// <summary>
    /// List for the normal Room candidates.
    /// </summary>
    public RoomInGame[] normalRoomList;
    /*/// <summary>
    /// List for the item Room candidates.
    /// </summary>
    public RoomInGame[] itemRoomList;*/
    /// <summary>
    /// List for the special Room candidates.
    /// </summary>
    public RoomInGame[] specialRoomList;
    public bool controlCurrentTetrimino = false;
    public Room startRoom;

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
    /// Delete one row.
    /// </summary>
    /// <param name="row">Rows wanted to be deleted.</param>
    public void DeleteRow(int row)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(mapGrid[x, row].gameObject);
            mapGrid[x, row] = null;
        }
    }
    public void DeleteFullRows()
    {
        for(int y = realHeight; y >= 0; y--)
        {
            if (IsRowFull(y))
            {
                for (int x = 0; x < width; x++)
                {
                    Destroy(mapGrid[x, y].gameObject);
                    mapGrid[x, y] = null;
                }
                DecreaseRowsAbove(y);
            }
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
                if (mapGrid[x, y] != null && mapGrid[x, y].transform.parent)
                {
                    mapGrid[x, y - 1] = mapGrid[x, y];
                    mapGrid[x, y] = null;
                    mapGrid[x, y - 1].mapCoord += new Vector3(0, -1, 0);
                    mapGrid[x, y - 1].transform.position += new Vector3(0, -tetrisMapSize, 0);
                }

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
            if (mapGrid[x, row] == null || mapGrid[x, row].specialRoomType == Room.SpecialRoomType.Boss)
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
    public IEnumerator DisplayTime()
    {
        yield return null;
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
        /*for(int i = 0; i < te.rooms.Length; i++)
        {
            if(te.rooms[i].mapCoord.x > 9)
            {
                for (int j = 0; j < te.rooms.Length; j++)
                    te.rooms[i].mapCoord += new Vector3(0, -1, 0);
                i = 0;
            }
        }*/
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
        isTetriminoFalling = true;
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
        UpdateMap(te);
        CreateRoom(te);
        DeleteFullRows();
        Destroy(currentGhost.gameObject);
        TS.MakeTetrimino();
        isTetriminoFalling = false;
    }
    /// <summary>
    /// Get tetrimino down.
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
        EndTetrimino(currentTetrimino);
    }
    /// <summary>
    /// Get ghost down.
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
        if(Input.GetKeyDown(KeyCode.Space) && GameManager.gameState == GameManager.GameState.Tetris)
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
            UpdateMap(currentTetrimino);
            te.rooms[i].transform.parent = grid;
            te.rooms[i].transform.position += new Vector3(0, 0, -2);
            if (te.rooms[i].specialRoomType != Room.SpecialRoomType.Normal)
            {
                Instantiate(specialRoomList[(int)te.rooms[i].specialRoomType], te.rooms[i].transform.position + new Vector3(0, 0, 2), Quaternion.identity, te.rooms[i].transform);
            }
            else
                Instantiate(normalRoomList[Random.Range(0, normalRoomList.Length)], te.rooms[i].transform.position + new Vector3(0, 0, 2), Quaternion.identity, te.rooms[i].transform);
        }
        Destroy(te.gameObject);
    }
    /*public void UpgradeRoom(Tetrimino te, Room.SpecialRoomType)
    {

    }*/

    void Awake ()
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
            tetrisYCoord[i] = i * 24;
        }
        TS = GameObject.Find("TetriminoSpawner").GetComponent<TetriminoSpawner>();
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
            }
           if(currentGhost != null)
            {
                GhostControl(currentGhost, currentTetrimino);
                currentGhost.transform.position = new Vector3(currentGhost.mapCoord.x * tetrisMapSize, tetrisYCoord[(int)currentGhost.mapCoord.y], currentGhost.mapCoord.z * tetrisMapSize);
            }
            //currentTetrimino.transform.position = currentTetrimino.mapCoord * tetrisMapSize + tetrisMapCoord;
        }

    }
}
