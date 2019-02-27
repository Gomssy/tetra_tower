using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomItemInfo<T>
{
    public List<T> itemSpawnInfo = new List<T>();
    public RoomItemInfo()
    {

    }
    public void AddItemInfo(T _itemSpawnInfo)
	{
        itemSpawnInfo.Add(_itemSpawnInfo);
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
    /// <summary>
    /// This function is called when player clears the room.
    /// </summary>
    public virtual void RoomClear()
    {
        Room room = transform.parent.GetComponent<Room>();

        Vector3 fogPosition = room.fog.transform.position;
        Destroy(room.fog.gameObject);
        room.fog = Instantiate(MapManager.Instance.clearedFog, fogPosition, Quaternion.identity, transform);
        room.fog.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        room.CreatePortal();
        room.isRoomCleared = true;
        GameManager.Instance.clock.clockSpeedStack -= 4;
        if (GameManager.Instance.clock.clockSpeedStack < 0)
            GameManager.Instance.clock.clockSpeedStack = 0;
        room.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
}
