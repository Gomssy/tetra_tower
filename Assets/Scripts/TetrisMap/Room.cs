using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public int leftDoorLocation;
    public int rightDoorLocation;
    /// <summary>
    /// Select which doors would be opened.
    /// </summary>
    public void SetDoors()
    {
        if (mapCoord.x < MapManager.width - 1 && MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y] != null)
            rightDoorLocation = MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].leftDoorLocation;
        else
            rightDoorLocation = Random.Range(0, 3);
        if (mapCoord.x > 0 && MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y] != null)
            leftDoorLocation = MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].rightDoorLocation;
        else
            leftDoorLocation = Random.Range(0, 3);
    }
    /// <summary>
    /// Create doors.
    /// </summary>
    public void CreateDoors(GameObject leftDoor, GameObject rightDoor)
    {
        Instantiate(leftDoor, transform.position + new Vector3(1, leftDoorLocation * 8 + 1, 0), Quaternion.identity, transform);
        Instantiate(rightDoor, transform.position + new Vector3(23, rightDoorLocation * 8 + 1, 0), Quaternion.identity, transform);
    }
}
