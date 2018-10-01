using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


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
    }


}
