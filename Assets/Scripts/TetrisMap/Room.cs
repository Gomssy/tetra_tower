using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Door
{
    /// <summary>
    /// Door info of left door.
    /// 0 for lowest door, 1 for middle door, 2 for highest door.
    /// </summary>
    public int left;
    /// <summary>
    /// Door info of right door.
    /// 0 for lowest door, 1 for middle door, 2 for highest door.
    /// </summary>
    public int right;
}

/// <summary>
/// Room class
/// </summary>
public class Room : MonoBehaviour
{
    /// <summary>
    /// Room's Location on tetris map.
    /// Not related to real location.
    /// </summary>
    public Vector3 mapCoord;
    /// <summary>
    /// Door info per rooms.
    /// 0 for up, 1 for down.
    /// doorInfo[0] for left door, doorInfo[1] for right door.
    /// </summary>
    //public int[] doorInfo;

    Door doorInfo = new Door();
    /// <summary>
    /// Stage per rooms.
    /// </summary>
    public int stage;
    /// <summary>
    /// Room concept per rooms.
    /// Range from 0 to 3.
    /// </summary>
    public int roomConcept;
    /// <summary>
    /// Item room type per rooms.
    /// 0 for normal room, 1~ for item rooms.
    /// </summary>
    public int itemRoomType;
    /// <summary>
    /// Special room types.
    /// </summary>
    public MapManager.SpecialRoomType specialRoomType;

    public int testLeft;
    public int testRight;
    /// <summary>
    /// Select which doors would be opened.
    /// </summary>
    public void SetDoors()
    {
        if (mapCoord.x < MapManager.width - 1 && MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y] != null)
        {
            doorInfo.right = MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].doorInfo.left;
        }
        else
        {
            doorInfo.right = Random.Range(0, 3);
        }
        if (mapCoord.x > 0 && MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y] != null)
        {
            doorInfo.left = MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].doorInfo.right;
        }
        else
        {
            doorInfo.left = Random.Range(0, 3);
        }
        testLeft = doorInfo.left;
        testRight = doorInfo.right;
    }
}
