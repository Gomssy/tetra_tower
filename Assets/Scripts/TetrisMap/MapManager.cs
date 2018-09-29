using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    /*
     * variables
     * */
    /// <summary>
    /// If camera is at tetris map, it is true.
    /// If camera is at player, it is false.
    /// </summary>
    public bool inTetris;
    /// <summary>
    /// Tetris map's size.
    /// </summary>
    public float tetrisMapSize = 1;
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
    public static Room[,] mapCoord = new Room[width, height];
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
    public GameObject mapLeftEnd, mapRightEnd;
    /// <summary>
    /// Current tetrimino waiting for falling.
    /// </summary>
    public Tetrimino currentTetrimino;


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
    /// <param name="TE">-1 for over left end, 1 for over right end, 0 for right place.</param>
    /// <returns></returns>
    public int IsRightTetrimino(Tetrimino TE)
    {
        for (int i = 0; i < TE.rooms.Length; i++)
        {
            if (TE.rooms[i].transform.position.x < mapLeftEnd.transform.position.x)
                return -1;
            else if (TE.rooms[i].transform.position.x > mapRightEnd.transform.position.x)
                return 1;
        }
        return 0;
    }
    /// <summary>
    /// Make Tetrimino in right X coordinate.
    /// </summary>
    /// <param name="TE">Tetrimino.</param>
    public void MakeTetriminoRightPlace(Tetrimino TE)
    {
        while (true)
        {
            if (IsRightTetrimino(TE) == 1)
            {
                TE.transform.position += new Vector3(-tetrisMapSize, 0, 0);
            }
            else if (IsRightTetrimino(TE) == -1)
            {
                TE.transform.position += new Vector3(tetrisMapSize, 0, 0);
            }
            else
                break;
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
            Destroy(mapCoord[x, row].gameObject);
            mapCoord[x, row] = null;
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
                mapCoord[x, y - 1] = mapCoord[x, y];
                mapCoord[x, y] = null;
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
            if (mapCoord[x, row] != null && mapCoord[x, row].specialRoomType == Room.SpecialRoomType.Boss)
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
    /// <param name="TE">Tetrimino you want to update on map.</param>
    public void UpdateMap(Tetrimino TE)
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



    public void TetriminoDown(Tetrimino TE)
    {

    }
    public void GhostDown()
    {

    }
    public void TetriminoMove(Tetrimino TE)
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) && inTetris)
        {
            TE.transform.position += new Vector3(-tetrisMapSize, 0, 0);
            if(IsRightTetrimino(TE) != 0)
                TE.transform.position += new Vector3(tetrisMapSize, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) && inTetris)
        {
            TE.transform.position += new Vector3(tetrisMapSize, 0, 0);
            if (IsRightTetrimino(TE) != 0)
                TE.transform.position += new Vector3(-tetrisMapSize, 0, 0);
        }
    }
    public void TetriminoRotate(Tetrimino TE)
    {

    }
    public void InitiateTetrimino()
    {

    }

    /*
     * Test
     * */
    public void SpawnBossTetrimino()
    {
        spawnBossTetrimino = true;
    }
    // Use this for initialization
    void Start () {
        inTetris = true;
    }
	
	// Update is called once per frame
	void Update () {
        TetriminoMove(currentTetrimino);
	}

}
