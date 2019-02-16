﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    /// <summary>
    /// Which state this game is.
    /// change later
    /// </summary>
    public static GameState gameState;

    Vector3 spawnPosition = new Vector3(2, 1, 0);

    public GameObject minimap;
    public Canvas gameOverCanvas;
    public Canvas inventoryCanvas;

    // method
    // Constructor - protect calling raw constructor
    protected GameManager() { }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
        gameState = GameState.Ingame;
        Destroy(MapManager.currentRoom.gameObject);
        minimap.SetActive(true);
        TetriminoSpawner.Instance.MakeInitialTetrimino();
        GameObject.Find("Player").transform.position = MapManager.currentRoom.roomInGame.transform.Find("portal spot").position + spawnPosition;
        GameObject.Find("Main Camera").transform.position = GameObject.Find("Player").transform.position + new Vector3(0, 0, -1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        gameState = GameState.Tutorial;
        minimap = GameObject.Find("Minimap");
        minimap.SetActive(false);
        MapManager.currentRoom = GameObject.Find("Room Tutorial").GetComponent<Room>();
        GameObject.Find("Player").transform.position = MapManager.currentRoom.roomInGame.transform.Find("player spot").position + spawnPosition;
        GameObject.Find("Main Camera").transform.position = GameObject.Find("Player").transform.position + new Vector3(0, 0, -1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(CameraController.isSceneChanging != true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (gameState == GameState.Ingame)
                {
                    StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeScene(GameState.Tetris));
                }
                else if (gameState == GameState.Tetris)
                {
                    StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeScene(GameState.Ingame));
                }
            }
            else if(Input.GetKeyDown(KeyCode.I))
            {
                if(gameState == GameState.Ingame)
                {
                    inventoryCanvas.gameObject.SetActive(true);
                    gameState = GameState.Inventory;
                }
                else if(gameState == GameState.Inventory)
                {
                    inventoryCanvas.gameObject.SetActive(false);
                    gameState = GameState.Ingame;
                }
            }
            else if (Input.GetButtonDown("Interaction"))
            {
                if (gameState == GameState.Portal && MapManager.currentRoom != MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y])
                {
                    GameObject.Find("Player").transform.position = MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y].portal.transform.position + spawnPosition;
                    MapManager.Instance.ChangeRoom(MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y]);
                    MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y].portalSurface.GetComponent<SpriteRenderer>().sprite =
                        MapManager.Instance.portalExist;
                    StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeScene(GameState.Ingame));
                }
            }
            else if(Input.GetButtonDown("Cancel"))
            {
                if(gameState == GameState.Portal)
                {
                    MapManager.mapGrid[(int)MapManager.currentRoom.mapCoord.x, (int)MapManager.currentRoom.mapCoord.y].portalSurface.GetComponent<SpriteRenderer>().sprite =
                        MapManager.Instance.portalExist;
                    MapManager.mapGrid[(int)MapManager.currentRoom.mapCoord.x, (int)MapManager.currentRoom.mapCoord.y].portalSurface.GetComponent<SpriteRenderer>().sprite =
                        MapManager.Instance.portalExist;
                    StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeScene(GameState.Ingame));
                }
                else if(gameState == GameState.Inventory)
                {
                    inventoryCanvas.gameObject.SetActive(false);
                    gameState = GameState.Ingame;
                }
            }
        }
        if(gameState == GameState.GameOver)
        {
            if(gameOverCanvas.isActiveAndEnabled == false)
            {
                Debug.Log("Game Over");
                gameOverCanvas.gameObject.SetActive(true);
            }
        }
    }
}
