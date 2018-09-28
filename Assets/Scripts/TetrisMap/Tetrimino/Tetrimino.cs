using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino : MonoBehaviour {

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
    /// Tetrimino room's concept.
    /// </summary>
    public int roomConcept;
    /// <summary>
    /// Indicates if it is boss tetrimino.
    /// </summary>
    public bool isBossTetrimino = false;
    /// <summary>
    /// Enum for tetriminno types.
    /// </summary>
    public enum TetriminoType { I, O, T, J, L, S, Z };
    /// <summary>
    /// Tetrimino types.
    /// </summary>
    public TetriminoType tetriminoType;










}
