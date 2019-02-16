  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    LifeStoneManager lifeStoneManager;
    public static int tx, ty;
    public static float X = 0.7f, Y = 1.6f;
    public int ttx;
    public int tty;

	// Use this for initialization
	void Start () {
        ttx = (int)(transform.position.x / 24f);
        tty = (int)(transform.position.y-0.9f / 24f);
        lifeStoneManager = GameObject.Find("LifeStoneUI").GetComponent<LifeStoneManager>();
    }
	
	// Update is called once per frame
	void Update () {
        tx = (int)(transform.position.x / 24f);
        ty = (int)((transform.position.y-0.9f) / 24f);
        if ((ttx != tx || tty != ty) && MapManager.isRoomFalling != true)
        {
            MapManager.tempRoom = MapManager.mapGrid[tx, ty];
            if (tx < ttx)
            {
                MapManager.currentRoom.CloseDoor("Left", true);
            }
            else if (tx > ttx)
            {
                MapManager.currentRoom.CloseDoor("Right", true);
            }
            else if (ty < tty)
            {
                MapManager.currentRoom.CloseDoor("Down", true);
            }
            else if (ty > tty)
            {
                MapManager.currentRoom.CloseDoor("Up", true);
            }
        }
        ttx = tx;
        tty = ty;
        if (lifeStoneManager.CountType(LifeStoneType.Normal) + lifeStoneManager.CountType(LifeStoneType.Gold) + lifeStoneManager.CountType(LifeStoneType.Amethyst) == 0)
            GameManager.gameState = GameState.GameOver;
	}
}
