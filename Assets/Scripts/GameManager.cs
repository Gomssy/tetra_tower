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
        var TS = GameObject.Find("TetriminoSpawner").GetComponent<TetriminoSpawner>();
        gameState = GameState.Ingame;
        TS.MakeInitialTetrimino();
        Vector2 coord = MapManager.currentRoom.transform.position;

        GameObject.Find("Player").transform.position = new Vector2(coord.x, coord.y) + new Vector2(3, 3);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
