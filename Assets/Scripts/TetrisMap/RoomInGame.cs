using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInGame : MonoBehaviour {

    /*
     * variables
     * */
    public bool[] leftDoorInfo = new bool[3];
    public bool[] rightDoorInfo = new bool[3];
    /// <summary>
    /// Information for stage.
    /// </summary>
    public bool[] concept = new bool[4];
    public int[,] tileInfo = new int[24, 24];

    /*
     * functions
     * */
     

    public virtual void RoomEnter()
    {

    }





}
