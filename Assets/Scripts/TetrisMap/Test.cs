using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    MapManager mapManager;
    EnemyManager enemyManager;
    TetriminoSpawner tetriminoSpawner;
    public static Vector3 tetrisCameraCoord = new Vector3(180, 0, -1);
    public static float tetrisMapSize = 300;
    public Text timer;

    public void ChangeTetrimino()
    {
        Destroy(MapManager.currentTetrimino.gameObject);
        Destroy(MapManager.currentGhost.gameObject);
        tetriminoSpawner.MakeTetrimino();
    }
    public void SpawnBossTetrimino()
    {
        mapManager.spawnBossTetrimino = true;
    }
    public void UpgradeStage()
    {
        if(MapManager.currentStage < 5)
            MapManager.currentStage += 1;
    }
    public void Gold()
    {
        mapManager.UpgradeRoom(RoomType.Gold);
    }
    public void Amethyst()
    {
        mapManager.UpgradeRoom(RoomType.Amethyst);
    }
    public void BothSide()
    {
        mapManager.UpgradeRoom(RoomType.BothSide);
    }
    public void Boss()
    {
        SpawnBossTetrimino();
    }
    public void Timer()
    {
        timer.text = (mapManager.timeToFallTetrimino - mapManager.tetriminoWaitedTime).ToString();
    }
    public void ClearRoom()
    {
        MapManager.currentRoom.ClearRoom();
    }
    public void SummonEnemy()
    {
        enemyManager.SpawnEnemy();
    }
    public void ChangeTile()
    {
        GameObject.Find("MapManager").GetComponent<TileManager>().SetAllTiles(MapManager.currentRoom.roomInGame);
    }


    private void Awake()
    {
        //leftDoor.GetComponent<Animator>().SetInteger("doorPosition", 3);
        enemyManager = EnemyManager.Instance;
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
        if(!MapManager.isTetriminoFalling)
            Timer();
    }
}
