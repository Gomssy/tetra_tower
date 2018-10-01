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
        gameState = GameState.Ingame;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
