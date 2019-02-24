using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoldRoomItemInfo
{
    public ItemSpawnType[] itemType;
    public ItemQuality[] itemQuality;
    public int[] price;
    public GoldRoomItemInfo(ItemSpawnType[] _itemType, ItemQuality[] _itemQuality, int[] _price)
    {
        itemType = new ItemSpawnType[_itemType.Length];
        itemQuality = new ItemQuality[_itemQuality.Length];
        price = new int[_price.Length];
        for (int i = 0; i < _itemType.Length; i++)
        {
            itemType[i] = _itemType[i];
            itemQuality[i] = _itemQuality[i];
            price[i] = _price[i];
        }
    }
}

public class GoldRoomInGame : RoomInGame {

    public static RoomItemInfo<GoldRoomItemInfo>[] goldRoomInformation = new RoomItemInfo<GoldRoomItemInfo>()[];

    public override void RoomEnter()
    {
        base.RoomEnter();

    }

    public override void RoomClear()
    {
        base.RoomClear();

    }
}
