using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    /*
     * variables
     * */
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
            int count = 0;
            foreach (Transform t in child.transform)
                count++;
            if (count == 0)
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
    public void TetriminoMove(Tetrimino TE)
    {

    }
    public void TetriminoRotate(Tetrimino TE)
    {

    }
    public void InitiateTetrimino()
    {

    }


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
