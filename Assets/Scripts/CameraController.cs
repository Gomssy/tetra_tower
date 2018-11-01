using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    /*
     * If camera is in Tetris view, ideal position is (108, 240, -1)
     * size 300
     * */



    GameManager.GameState lastGameState;

    Vector3 destination;


    // Use this for initialization
    void Start()
    {
        lastGameState = GameManager.GameState.Ingame;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastGameState != GameManager.gameState)
        {
            StartCoroutine("ChangeScene");
            lastGameState = GameManager.gameState;
        }
        else if (lastGameState == GameManager.GameState.Ingame)
        {
            SetDestination();
        }

        GotoDestination();
    }


    IEnumerator ChangeScene()
    {


        yield return null;

    }
    void SetDestination()
    {

    }

    void GotoDestination()
    {
        // TODO: Change this.
        Vector3 pos = GameObject.Find("Player").transform.position;
        pos.z = -1;
        transform.position = pos;
    }


}
