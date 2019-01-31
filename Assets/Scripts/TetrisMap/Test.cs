﻿using System.Collections;
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

    public GameObject upDoor;
    public GameObject downDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public void UpOpenTest()
    {
        Animator animator = upDoor.GetComponent<Animator>();
        animator.SetBool("doorOpen", true);
        animator.SetBool("doorClose", false);
    }
    public void UpCloseTest()
    {
        Animator animator = upDoor.GetComponent<Animator>();
        animator.SetBool("doorOpen", false);
        animator.SetBool("doorClose", true);
    }
    public void DownOpenTest()
    {
        Animator animator = downDoor.GetComponent<Animator>();
        animator.SetBool("doorOpen", true);
        animator.SetBool("doorClose", false);
    }
    public void DownCloseTest()
    {
        Animator animator = downDoor.GetComponent<Animator>();
        animator.SetBool("doorOpen", false);
        animator.SetBool("doorClose", true);
    }
    public void LeftOpenTest()
    {
        Animator animator = leftDoor.GetComponent<Animator>();
        animator.SetBool("doorOpen", true);
        animator.SetBool("doorClose", false);
    }
    public void LeftCloseTest()
    {
        Animator animator = leftDoor.GetComponent<Animator>();
        animator.SetBool("doorOpen", false);
        animator.SetBool("doorClose", true);
    }
    public void RightOpenTest()
    {
        Animator animator = rightDoor.GetComponent<Animator>();
        animator.SetBool("doorOpen", true);
        animator.SetBool("doorClose", false);
    }
    public void RightCloseTest()
    {
        Animator animator = rightDoor.GetComponent<Animator>();
        animator.SetBool("doorOpen", false);
        animator.SetBool("doorClose", true);
    }


    private void Awake()
    {
        //leftDoor.GetComponent<Animator>().SetInteger("doorPosition", 3);

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
