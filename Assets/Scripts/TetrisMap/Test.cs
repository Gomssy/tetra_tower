using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    MapManager mapManager;
    TetriminoSpawner tetriminoSpawner;
    public static Vector3 tetrisCameraCoord = new Vector3(180, 0, -1);
    public static float tetrisMapSize = 300;
    public Text timer;

    public void ChangeTetrimino()
    {
        Destroy(mapManager.currentTetrimino.gameObject);
        Destroy(mapManager.currentGhost.gameObject);
        tetriminoSpawner.MakeTetrimino();
    }
    public void SpawnBossTetrimino()
    {
        mapManager.spawnBossTetrimino = true;
    }
    public void Gold()
    {
        mapManager.UpgradeRoom(MapManager.SpecialRoomType.Gold);
    }
    public void Amethyst()
    {
        mapManager.UpgradeRoom(MapManager.SpecialRoomType.Amethyst);
    }
    public void BothSide()
    {
        mapManager.UpgradeRoom(MapManager.SpecialRoomType.BothSide);
    }
    public void Boss()
    {
        SpawnBossTetrimino();
    }
    public void Timer()
    {
        timer.text = (mapManager.timeToFallTetrimino - mapManager.tetriminoWaitedTime).ToString();
    }

    private void Awake()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        tetriminoSpawner = GameObject.Find("TetriminoSpawner").GetComponent<TetriminoSpawner>();
    }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeTetrimino();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SpawnBossTetrimino();
        if(!mapManager.isTetriminoFalling)
            Timer();
        /*if (Input.GetKeyDown(KeyCode.Tab) && GameManager.gameState != GameManager.GameState.Tetris)
        {
            GameManager.gameState = GameManager.GameState.Tetris;
            GameObject.Find("Main Camera").transform.position = tetrisCameraCoord;
            GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = tetrisMapSize;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && GameManager.gameState == GameManager.GameState.Tetris)
            GameManager.gameState = GameManager.GameState.Ingame;*/
    }
}
