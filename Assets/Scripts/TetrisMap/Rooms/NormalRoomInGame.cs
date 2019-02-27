using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRoomInGame : RoomInGame {


    bool roomEntered = false;
    public override void RoomEnter()
    {
        base.RoomEnter();
        EnemyManager.Instance.SpawnEnemyToMap();
        roomEntered = true;
    }

    public override void RoomClear()
    {
        base.RoomClear();
        MapManager.Instance.clearedRoomCount++;
    }

    private void Update()
    {
        if (roomEntered && GameManager.Instance.statueEvent == 1)
        {
            Debug.Log(MapManager.currentRoom.transform.name);
            foreach (Transform obj in MapManager.currentRoom.roomInGame.transform)
            {
                if (obj.CompareTag("Enemy"))
                {
                    GameObject body = obj.GetChild(0).gameObject;
                    if (body.GetComponent<EnemyGround>() != null && body.GetComponent<SpriteRenderer>().isVisible)
                    {
                        Vector3 pos = obj.position;
                        body.gameObject.GetComponent<EnemyGround>().ResetForPool();
                        GameObject tempStatue = Instantiate(GameManager.Instance.statue, pos, Quaternion.identity);
                        tempStatue.name = "aaa";
                        GameManager.Instance.statueEvent = 2;
                    }
                }
            }
        }
    }
}
