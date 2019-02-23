using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoomInGame : RoomInGame {

    GameObject portal;
    
	// Use this for initialization
	void Start () {
        EnemyManager.Instance.SpawnEnemyToMap();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
