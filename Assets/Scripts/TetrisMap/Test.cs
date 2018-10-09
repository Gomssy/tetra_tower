using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {


    public void ChangeTetrimino()
    {
        var MM = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>();
        var TS = GameObject.FindGameObjectWithTag("TetriminoSpawner").GetComponent<TetriminoSpawner>();
        Destroy(MM.currentTetrimino.gameObject);
        TS.MakeTetrimino();
    }
    public void SpawnBossTetrimino()
    {
        var MM = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>();
        MM.spawnBossTetrimino = true;
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
	}
}
