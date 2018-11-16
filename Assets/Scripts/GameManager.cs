using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum GameState { MainMenu, Ingame, Tetris, Pause, Inventory }

    /// <summary>
    /// Which state this game is.
    /// change later
    /// </summary>
    public static GameState gameState;

    // Use this for initialization
    void Start () {
        gameState = GameState.Tetris;
        var TS = GameObject.Find("TetriminoSpawner").GetComponent<TetriminoSpawner>();
        TS.MakeInitialTetrimino();
        Vector2 coord = GameObject.Find("MapManager").GetComponent<MapManager>().startRoom.transform.position;

        //GameObject.Find("Player").transform.position = new Vector2(coord.x, coord.y) + new Vector2(3, 3);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
