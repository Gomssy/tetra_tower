using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnInfo
{
    ItemType itemType;
    int amount;
    bool isGold;
    public ItemSpawnInfo(ItemType _itemType, int _amount)
    {
        itemType = _itemType;
        amount = _amount;
        isGold = false;
    }
    public ItemSpawnInfo(ItemType _itemType, int _amount, bool _isGold)
    {
        itemType = _itemType;
        amount = _amount;
        isGold = _isGold;
    }
}

public class RoomInGame : MonoBehaviour {

    /*
     * variables
     * */
    /// <summary>
    /// Size of the room counted by 1 unit.
    /// </summary>
    public Vector2 roomSize;
    public bool[] leftDoorInfo = new bool[3];
    public bool[] rightDoorInfo = new bool[3];
    /// <summary>
    /// Information for concept.
    /// </summary>
    public bool[] concept = new bool[4];
    public bool[,] wallTileInfo = new bool[24, 24];
    public bool[,] platformTileInfo = new bool[24, 24];
    public bool[,] ropeTileInfo = new bool[24, 24];
    public bool[,] spikeTileInfo = new bool[24, 24];

    /*
     * functions
     * */
    /// <summary>
    /// This function is called when player enters the room.
    /// </summary>
    public virtual void RoomEnter()
    {

    }
}
