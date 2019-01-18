using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    /// <summary>
    /// Position of this door.
    /// 0 for up, 1 for right, 2 for down, 3 for left.
    /// </summary>
    public int position;
    /// <summary>
    /// Entered position of the player. Use this for check if player has entered through this door or not.
    /// 0 for up, 1 for right, 2 for down, 3 for left.
    /// </summary>
    public int enteredPosition;
    /// <summary>
    /// Animator of this door.
    /// </summary>
    public Animator animatorThisRoom;
    /// <summary>
    /// Animator of adjacent door with this door.
    /// </summary>
    public Animator animatorNextRoom;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enteredPosition == position && collision.tag.Equals("Player"))
        {
            switch (position)
            {
                case 0:
                    if (collision.transform.position.y < GetComponent<PolygonCollider2D>().transform.position.y)
                        return;
                    enteredPosition = 2;
                    break;
                case 1:
                    if (collision.transform.position.x < GetComponent<PolygonCollider2D>().transform.position.x)
                        return;
                    enteredPosition = 3;
                    break;
                case 2:
                    if (collision.transform.position.y > GetComponent<PolygonCollider2D>().transform.position.y)
                        return;
                    enteredPosition = 0;
                    break;
                case 3:
                    if (collision.transform.position.x > GetComponent<PolygonCollider2D>().transform.position.x)
                        return;
                    enteredPosition = 1;
                    break;
            }
            MapManager mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
            Room room = MapManager.currentRoom;
            StartCoroutine(mapManager.GetComponent<MapManager>().RoomFadeOut(room));
            if(room.specialRoomType == RoomType.Normal)
                room.GetComponent<SpriteRenderer>().sprite = mapManager.roomsSpritesDistributed[room.stage][(int)RoomSpriteType.Normal1 + room.roomConcept];
            else
                room.GetComponent<SpriteRenderer>().sprite = mapManager.roomsSpritesDistributed[room.stage][(int)room.specialRoomType];
            MapManager.currentRoom = MapManager.tempRoom;
            room = MapManager.currentRoom;
            StartCoroutine(mapManager.RoomFadeIn(room));
            room.GetComponent<SpriteRenderer>().sprite = mapManager.roomsSpritesDistributed[room.stage][(int)RoomSpriteType.Current];
            if (MapManager.currentRoom.isRoomCleared != true)
            {
                animatorThisRoom.SetBool("isPlayerTouchEnded", true);
                animatorNextRoom.SetBool("isPlayerTouchEnded", true);
                animatorThisRoom.SetBool("doorOpen", false);
                animatorNextRoom.SetBool("doorOpen", false);
                animatorThisRoom.SetBool("doorClose", true);
                animatorNextRoom.SetBool("doorClose", true);
            }
        }
    }
}
