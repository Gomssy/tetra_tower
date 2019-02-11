using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRoomInGame : RoomInGame {

    public override void RoomEnter()
    {
        base.RoomEnter();
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().SpawnEnemy();
    }
}
