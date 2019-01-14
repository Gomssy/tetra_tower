using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool close = false;
    public int position;
    public int enteredPosition;
    public Animator animatorThisRoom;
    public Animator animatorNextRoom;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enteredPosition == position && collision.tag.Equals("Player") && close == true)
        {
            switch (position)
            {
                case 0:
                    if (collision.transform.position.y < GetComponent<PolygonCollider2D>().transform.position.y)
                        return;
                    break;
                case 1:
                    if (collision.transform.position.x < GetComponent<PolygonCollider2D>().transform.position.x)
                        return;
                    break;
                case 2:
                    if (collision.transform.position.y > GetComponent<PolygonCollider2D>().transform.position.y)
                        return;
                    break;
                case 3:
                    if (collision.transform.position.x > GetComponent<PolygonCollider2D>().transform.position.x)
                        return;
                    break;
            }
            StartCoroutine(GameObject.Find("MapManager").GetComponent<MapManager>().RoomFadeOut(MapManager.currentRoom));
            MapManager.currentRoom = MapManager.tempRoom;
            StartCoroutine(GameObject.Find("MapManager").GetComponent<MapManager>().RoomFadeIn(MapManager.currentRoom));
            if (MapManager.currentRoom.isRoomCleared != true)
            {
                animatorThisRoom.SetBool("isPlayerTouchEnded", true);
                animatorNextRoom.SetBool("isPlayerTouchEnded", true);
                animatorThisRoom.SetBool("doorOpen", false);
                animatorNextRoom.SetBool("doorOpen", false);
                animatorThisRoom.SetBool("doorClose", true);
                animatorNextRoom.SetBool("doorClose", true);
            }
            close = false;
        }
    }
}
