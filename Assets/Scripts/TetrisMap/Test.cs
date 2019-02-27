using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    
    public static Vector3 tetrisCameraCoord = new Vector3(180, 0, -1);
    public static float tetrisMapSize = 300;
    public Text timer;
    public GameObject testUI;
    public GameObject lifeStoneTestUI;
    bool isTestUiActive = true;
    
    public void ChangeTetrimino()
    {
        Destroy(MapManager.currentTetrimino.gameObject);
        Destroy(MapManager.currentGhost.gameObject);
        TetriminoSpawner.Instance.MakeTetrimino();
    }
    public void SpawnBossTetrimino()
    {
        MapManager.Instance.clearedRoomCount = MapManager.Instance.stageClearCondition[MapManager.currentStage];
    }
    public void UpgradeStage()
    {
        if(MapManager.currentStage < 5)
            MapManager.currentStage += 1;
	}
	public void Item()
	{
		MapManager.Instance.UpgradeRoom(RoomType.Item);
	}
	public void Gold()
    {
        MapManager.Instance.UpgradeRoom(RoomType.Gold);
    }
    public void Amethyst()
    {
        MapManager.Instance.UpgradeRoom(RoomType.Amethyst);
    }
    public void BothSide()
    {
        MapManager.Instance.UpgradeRoom(RoomType.BothSide);
    }
    public void Boss()
    {
        SpawnBossTetrimino();
    }
    public void Timer()
    {
        timer.text = (GameManager.Instance.clock.timeToFallTetrimino - GameManager.Instance.clock.tetriminoWaitedTime).ToString();
    }
    public void ClearRoom()
    {
        MapManager.currentRoom.FinishRoom();
    }
    public void SummonEnemy()
    {
        EnemyManager.Instance.SpawnEnemyToMap();
    }


    private void Awake()
    {

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
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isTestUiActive)
            {
                testUI.SetActive(false);
                lifeStoneTestUI.SetActive(false);
                isTestUiActive = false;
            }
            else if (!isTestUiActive)
            {
                testUI.SetActive(true);
                lifeStoneTestUI.SetActive(true);
                isTestUiActive = true;
            }
        }
        if (!MapManager.isTetriminoFalling)
            Timer();
    }
}
