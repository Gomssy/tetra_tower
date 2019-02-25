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

    /// <summary>
    /// The information of gold room's item spawn data.
    /// Each index means stage.
    /// </summary>
    public static RoomItemInfo<GoldRoomItemInfo>[] goldRoomInformation = new RoomItemInfo<GoldRoomItemInfo>[5];
    GameObject[] itemGoods;

    /// <summary>
    /// Loads data from gold room's item spawn data.
    /// </summary>
    /// <param name="dataFile">The data file of gold room..</param>
    public static void LoadGoldRoomData(TextAsset dataFile)
    {
        for (int i = 0; i < goldRoomInformation.Length; i++)
            goldRoomInformation[i] = new RoomItemInfo<GoldRoomItemInfo>();
        string[] linesFromText = dataFile.text.Split('\n');
        string[] cellValue = null;
        int stageIndex = 0;
        int skipDistance = 1;
        for (int i = 1; i < linesFromText.Length; i++)
        {
            cellValue = linesFromText[i].Split(',');
            int itemCase = (cellValue.Length - skipDistance) / 3;
            ItemSpawnType[] itemType = new ItemSpawnType[itemCase];
            ItemQuality[] itemQuality = new ItemQuality[itemCase];
            int[] itemPrice = new int[itemCase];
            for (int j = 0; j < itemCase; j++)
            {
                itemType[j] = (ItemSpawnType)System.Enum.Parse(typeof(ItemSpawnType), cellValue[skipDistance + j * 3]);
                itemQuality[j] = (ItemQuality)System.Enum.Parse(typeof(ItemQuality), cellValue[skipDistance + j * 3 + 1]);
                itemPrice[j] = int.Parse(cellValue[skipDistance + j * 3 + 2]);
            }
            goldRoomInformation[int.Parse(cellValue[stageIndex]) - 1].AddItemInfo(new GoldRoomItemInfo(itemType, itemQuality, itemPrice));
        }
    }
    /// <summary>
    /// Spawn items according to the probability and item stage of this room.
    /// </summary>
    public void SpawnItem()
    {
        Room room = transform.parent.GetComponent<Room>();
        InventoryManager inventoryManager = InventoryManager.Instance;
        LifeStoneManager lifeStoneManager = LifeStoneManager.Instance;
        Vector3[] itemPosition = new Vector3[6];
        int j = 0;
        foreach (Transform child in transform.Find("item spot"))
            itemPosition[j++] = child.position;
        int goldRoomIndex = room.stage;
        int random = Random.Range(0, goldRoomInformation[goldRoomIndex].itemSpawnInfo.Count);
        GoldRoomItemInfo itemInfo = goldRoomInformation[goldRoomIndex].itemSpawnInfo[random];
        itemGoods = new GameObject[itemInfo.itemType.Length];
        int itemCount = 0;
        for (int i = 0; i < itemInfo.itemType.Length; i++)
        {
            if (itemInfo.itemType[i] == ItemSpawnType.Item)
                itemGoods[i] = inventoryManager.ItemInstantiate(itemInfo.itemQuality[i], itemPosition[itemCount++], itemInfo.price[i], 0);
            else if (itemInfo.itemType[i] == ItemSpawnType.Addon)
                itemGoods[i] = inventoryManager.AddonInstantiate(itemInfo.itemQuality[i], itemPosition[itemCount++], itemInfo.price[i], 0);
            else if (itemInfo.itemType[i] == ItemSpawnType.GoldPotion)
                itemGoods[i] = lifeStoneManager.InstantiatePotion(itemPosition[itemCount++], itemInfo.price[i], 0);
            else if (itemInfo.itemType[i] == ItemSpawnType.LifeStone)
            {
                if (itemInfo.itemQuality[i] == ItemQuality.Gold)
                    itemGoods[i] = lifeStoneManager.InstantiateDroppedLifeStone(new Vector2Int(3, 2), 1, 0, itemPosition[itemCount++], itemInfo.price[i], 0);
                else
                    itemGoods[i] = lifeStoneManager.InstantiateDroppedLifeStone(new Vector2Int(3, 2), 0, 0, itemPosition[itemCount++], itemInfo.price[i], 0);
            }
            itemGoods[i].GetComponent<DroppedObject>().priceTag.transform.position = itemGoods[i].transform.position + new Vector3(0, 1, 0);
            itemGoods[i].GetComponent<DroppedObject>().priceTag.text = itemInfo.price[i].ToString();
        }
    }

    public override void RoomEnter()
    {
        base.RoomEnter();
        SpawnItem();
    }

    public override void RoomClear()
    {
        base.RoomClear();

    }
}
