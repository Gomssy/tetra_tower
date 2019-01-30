using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    /// <summary>
    /// Which state this game is.
    /// change later
    /// </summary>
    public static GameState gameState;

    public Canvas gameOverScreen;

    public void RestartGame()
    {
        gameOverScreen.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // Use this for initialization
    void Start () {
        gameState = GameState.Ingame;
        GameObject.Find("TetriminoSpawner").GetComponent<TetriminoSpawner>().MakeInitialTetrimino();
        Vector2 coord = MapManager.currentRoom.transform.position;
        GameObject.Find("Player").transform.position = new Vector2(coord.x, coord.y) + new Vector2(3, 3);
        GameObject.Find("Main Camera").transform.position = GameObject.Find("Player").transform.position;
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
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (gameState == GameState.Portal && MapManager.currentRoom != MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y])
                {
                    GameObject.Find("Player").transform.position = MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y].portal.transform.position + new Vector3(2, 1, 0);
                    GameObject.Find("MapManager").GetComponent<MapManager>().ChangeRoom(MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y]);
                    MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y].portalSurface.GetComponent<SpriteRenderer>().sprite =
                        GameObject.Find("MapManager").GetComponent<MapManager>().portalExist;
                    StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeScene(GameState.Ingame));
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MapManager.mapGrid[(int)MapManager.currentRoom.mapCoord.x, (int)MapManager.currentRoom.mapCoord.y].portalSurface.GetComponent<SpriteRenderer>().sprite =
                    GameObject.Find("MapManager").GetComponent<MapManager>().portalExist;
                StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeScene(GameState.Ingame));
            }
        }
        if(gameState == GameState.GameOver)
        {
            if(gameOverScreen.isActiveAndEnabled == false)
                Debug.Log("Game Over");
            gameOverScreen.gameObject.SetActive(true);
        }
    }
}
