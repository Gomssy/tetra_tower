using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomInGame : RoomInGame {

    public override void RoomEnter()
    {
        base.RoomEnter();

        //보스 만들어지면 구현할 것
    }

    public override void RoomClear()
    {
        base.RoomClear();
        MapManager.currentStage += 1;
    }
}
