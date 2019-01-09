using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInGame : MonoBehaviour {

    /// <summary>
    /// Information for side doors.
    /// Use this as binary. (1 as lowest, 2 as middle, 4 for highest)
    /// </summary>
    public int[] sideDoorInfo = new int[2];

    /// <summary>
    /// The enum for door information.
    /// </summary>
    public enum DoorInfo { Up, Down, Left, Right };
}
