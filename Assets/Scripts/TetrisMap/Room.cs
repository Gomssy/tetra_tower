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
    /// Door info per rooms.
    /// 0 for up, 1 for down.
    /// doorInfo[0] for left door, doorInfo[1] for right door.
    /// </summary>
    public int[] doorInfo;
    /// <summary>
    /// Stage per rooms.
    /// Range from 0 to 4.
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
    /// Enum for special room types.
    /// </summary>
    public enum SpecialRoomType { Start, Item, BothSide, Gold, Amethyst, Boss, Normal };
    /// <summary>
    /// Special room types.
    /// </summary>
    public SpecialRoomType specialRoomType;

    /// <summary>
    /// Select which doors would be opened.
    /// </summary>
    public void SetDoors()
    {

    }
}
