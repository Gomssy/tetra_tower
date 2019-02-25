using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino : MonoBehaviour {

    /*
     * variables
     * */
    /// <summary>
    /// Tetrimino's Location on tetris map.
    /// Not related to real location.
    /// </summary>
    public Vector3 mapCoord;
    /// <summary>
    /// Tetrimino's stage.
    /// </summary>
    public int stage;
    /// <summary>
    /// Tetrimino's room concept.
    /// </summary>
    public int roomConcept;
    /// <summary>
    /// Indicates if it is boss tetrimino.
    /// </summary>
    public bool isBossTetrimino = false;
    /// <summary>
    /// Tetrimino types.
    /// </summary>
    public TetriminoType tetriminoType;
    /// <summary>
    /// Each rooms for this tetrimino.
    /// </summary>
    public Room[] rooms;
    public int notNormalRoomCount = 0;
    public int itemRoomIndex = -1;
    /// <summary>
    /// Tetrimino rotated angle.
    /// </summary>
    public int rotatedAngle = 0;
    /// <summary>
    /// Tetrimino rotated position;
    /// </summary>
    public int[] rotatedPosition = { 0, 0, 0, 0 };
    public struct RotationInformation
    {
        public int[] horizontalLength;
    }
    public static RotationInformation[] rotationInformation = new RotationInformation[8];

    /*
     * functions
     * */

}
