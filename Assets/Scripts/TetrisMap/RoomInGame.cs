using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnInfo
{
	public float probability;
	public ItemSpawnType[] itemType;
	public ItemQuality[] itemQuality;
	public int[] amount;
	public ItemSpawnInfo(float _probability, ItemSpawnType[] _itemType, ItemQuality[] _itemQuality, int[] _amount)
	{
		probability = _probability;
		itemType = new ItemSpawnType[4];
		itemQuality = new ItemQuality[4];
		amount = new int[4];
		for (int i = 0; i < _itemType.Length; i++)
		{
			itemType[i] = _itemType[i];
			itemQuality[i] = _itemQuality[i];
			amount[i] = _amount[i];
		}
	}
}

public class RoomItemInfo
{
	public List<ItemSpawnInfo> itemSpawnInfo = new List<ItemSpawnInfo>();
	public RoomItemInfo()
	{

	}
	public RoomItemInfo(ItemSpawnInfo[] _itemSpawnInfo)
	{
		for (int i = 0; i < _itemSpawnInfo.Length; i++)
		{
			itemSpawnInfo.Add(_itemSpawnInfo[i]);
		}
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
