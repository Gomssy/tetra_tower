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
        float probability = Random.Range(0f, 100f);
        Vector3[] itemPosition = new Vector3[6];
        int j = 0;
        foreach (Transform child in transform.Find("item spot"))
            itemPosition[j++] = child.position;
        int goldRoomIndex = room.stage;
        int random = Random.Range(0, goldRoomInformation[goldRoomIndex].itemSpawnInfo.Count);
        GoldRoomItemInfo itemInfo = goldRoomInformation[goldRoomIndex].itemSpawnInfo[random];
        int itemCount = 0;
        for (int i = 0; i < itemInfo.itemType.Length; i++)
        {
            if (itemInfo.itemType[i] == ItemSpawnType.Item)
            {
                Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " price" + itemInfo.price[i]);
                inventoryManager.ItemInstantiate(itemInfo.itemQuality[i], itemPosition[itemCount++], 0);
                Debug.Log("done");
            }
            else if (itemInfo.itemType[i] == ItemSpawnType.Addon)
            {
                Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " price" + itemInfo.price[i]);
                inventoryManager.AddonInstantiate(itemInfo.itemQuality[i], itemPosition[itemCount++], 0);
                Debug.Log("done");
            }
            else if (itemInfo.itemType[i] == ItemSpawnType.GoldPotion)
            {
                Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " price" + itemInfo.price[i]);
                lifeStoneManager.InstantiatePotion(itemPosition[itemCount++], 0);
                Debug.Log("done");
            }
            else if (itemInfo.itemType[i] == ItemSpawnType.LifeStone)
            {
                if (itemInfo.itemQuality[i] == ItemQuality.Gold)
                {
                    Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " price" + itemInfo.price[i]);
                    lifeStoneManager.InstantiateDroppedLifeStone(6, 1, 0, itemPosition[itemCount++], 0);
                    Debug.Log("done");
                }
                else
                {
                    Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " price" + itemInfo.price[i]);
                    lifeStoneManager.InstantiateDroppedLifeStone(6, 0, 0, itemPosition[itemCount++], 0);
                    Debug.Log("done");
                }
            }
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
