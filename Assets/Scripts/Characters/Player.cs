using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public LifeStoneManager LCUI;
    public static int tx, ty;
    public int ttx;
    public int tty;
    Coroutine roomEnterFadeIn;
    Coroutine roomExitFadeOut;

	// Use this for initialization
	void Start () {
        ttx = (int)(transform.position.x / 24f);
        tty = (int)(transform.position.y / 24f);
    }
	
	// Update is called once per frame
	void Update () {
        tx = (int)(transform.position.x / 24f);
        ty = (int)((transform.position.y-0.9f) / 24f);
        if ((ttx != tx || tty != ty) && MapManager.isRoomFalling != true)
        {
            MapManager.currentRoom = MapManager.mapGrid[tx, ty];
            if (roomEnterFadeIn != null)
                StopCoroutine(roomEnterFadeIn);
            if (roomExitFadeOut != null)
                StopCoroutine(roomExitFadeOut);
            roomEnterFadeIn = StartCoroutine(GameObject.Find("MapManager").GetComponent<MapManager>().RoomFadeIn(MapManager.currentRoom));
            roomExitFadeOut = StartCoroutine(GameObject.Find("MapManager").GetComponent<MapManager>().RoomFadeOut(MapManager.mapGrid[ttx, tty]));
        }
        ttx = tx;
        tty = ty;
	}
}
