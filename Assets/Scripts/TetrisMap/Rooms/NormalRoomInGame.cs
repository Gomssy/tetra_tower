using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRoomInGame : RoomInGame {

    public override void RoomEnter()
    {
        base.RoomEnter();
        EnemyManager.Instance.SpawnEnemyToMap();
    }
}
