using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRoomItemInfo
{
    public float probability;
    public ItemSpawnType[] itemType;
    public ItemQuality[] itemQuality;
    public int[] amount;
    public ItemRoomItemInfo(float _probability, ItemSpawnType[] _itemType, ItemQuality[] _itemQuality, int[] _amount)
    {
        probability = _probability;
        itemType = new ItemSpawnType[_itemType.Length];
        itemQuality = new ItemQuality[_itemQuality.Length];
        amount = new int[_amount.Length];
        for (int i = 0; i < _itemType.Length; i++)
        {
            itemType[i] = _itemType[i];
            itemQuality[i] = _itemQuality[i];
            amount[i] = _amount[i];
        }
    }
}

public class ItemRoomInGame : RoomInGame {

    /// <summary>
    /// The information of item room's item spawn data.
    /// Each index means item stage.
    /// </summary>
    public static RoomItemInfo<ItemRoomItemInfo>[] itemRoomInformation = new RoomItemInfo<ItemRoomItemInfo>[5];

    /// <summary>
    /// Loads data from item room's item spawn data.
    /// </summary>
    /// <param name="dataFile">The data file of item room..</param>
    public static void LoadItemRoomData(TextAsset dataFile)
    {
        for (int i = 0; i < itemRoomInformation.Length; i++)
            itemRoomInformation[i] = new RoomItemInfo<ItemRoomItemInfo>();
        string[] linesFromText = dataFile.text.Split('\n');
        string[] cellValue = null;
        int stageIndex = 0;
        int probabilityIndex = 1;
        int skipDistance = 2;
        for (int i = 1; i < linesFromText.Length; i++)
        {
            cellValue = linesFromText[i].Split(',');
            int itemCase = (cellValue.Length - skipDistance) / 3;
            float probability = float.Parse(cellValue[probabilityIndex]);
            ItemSpawnType[] itemType = new ItemSpawnType[itemCase];
            ItemQuality[] itemQuality = new ItemQuality[itemCase];
            int[] itemAmount = new int[itemCase];
            for(int j = 0; j < itemCase; j++)
            {
                itemType[j] = (ItemSpawnType)System.Enum.Parse(typeof(ItemSpawnType), cellValue[skipDistance + j * 3]);
                itemQuality[j] = (ItemQuality)System.Enum.Parse(typeof(ItemQuality), cellValue[skipDistance + j * 3 + 1]);
                itemAmount[j] = int.Parse(cellValue[skipDistance + j * 3 + 2]);
            }
            itemRoomInformation[int.Parse(cellValue[stageIndex]) - 1].AddItemInfo(new ItemRoomItemInfo(probability, itemType, itemQuality, itemAmount));
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
		Vector3[] itemPosition = new Vector3[5];
		int j = 0;
		foreach (Transform child in transform.Find("item spot"))
			itemPosition[j++] = child.transform.position;
        int itemRoomIndex = room.itemRoomType > 5 ? 5 : room.itemRoomType;
        for (int index = 0; index < itemRoomInformation[itemRoomIndex - 1].itemSpawnInfo.Count; index++)
		{
			ItemRoomItemInfo itemInfo = itemRoomInformation[itemRoomIndex - 1].itemSpawnInfo[index];
			probability -= itemInfo.probability;
			Debug.Log(probability);
			if (probability <= 0)
			{
				Debug.Log("Item Spawn");
				int itemCount = 0;
				for(int i = 0; i < itemInfo.itemType.Length; i++)
				{
					if (itemInfo.itemType[i] == ItemSpawnType.Item) 
						for(int _amount = 0; _amount < itemInfo.amount[i]; _amount++)
						{
							Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " amount" + itemInfo.amount[i]);
							inventoryManager.ItemInstantiate(itemInfo.itemQuality[i], itemPosition[itemCount++], 0);
						}
					else if (itemInfo.itemType[i] == ItemSpawnType.Addon)
						for (int _amount = 0; _amount < itemInfo.amount[i]; _amount++)
						{
							Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " amount" + itemInfo.amount[i]);
							inventoryManager.AddonInstantiate(itemInfo.itemQuality[i], itemPosition[itemCount++], 0);
						}
					else if (itemInfo.itemType[i] == ItemSpawnType.GoldPotion)
						for (int _amount = 0; _amount < itemInfo.amount[i]; _amount++)
						{
							Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " amount" + itemInfo.amount[i]);
							lifeStoneManager.InstantiatePotion(itemPosition[itemCount++], 0);
						}
					else if (itemInfo.itemType[i] == ItemSpawnType.LifeStone)
					{
						if(room.itemRoomType < 4)
							for (int _amount = 0; _amount < itemInfo.amount[i]; _amount++)
							{
								if(itemInfo.itemQuality[i] == ItemQuality.Gold)
								{
									Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " amount" + itemInfo.amount[i]);
                                    lifeStoneManager.InstantiateDroppedLifeStone(new Vector2Int(3, 2), 4, 1, 0, itemPosition[itemCount++], 0);
								}
								else
								{
									Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " amount" + itemInfo.amount[i]);
                                    lifeStoneManager.InstantiateDroppedLifeStone(new Vector2Int(3, 2), 0, 0, itemPosition[itemCount++], 0);
								}
							}
						else
						{
							Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " amount" + itemInfo.amount[i]);
                            lifeStoneManager.InstantiateDroppedLifeStone(new Vector2Int(3, room.itemRoomType - 4), 1, 0, itemPosition[itemCount++], 0);
						}
					}
					else if (itemInfo.itemType[i] == ItemSpawnType.LifeStoneFrame)
					{
						Debug.Log("type" + itemInfo.itemType[i] + " quality" + itemInfo.itemQuality[i] + " amount" + itemInfo.amount[i]);
						lifeStoneManager.ExpandRow(room.itemRoomType - 4);
					}
				}
				return;
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
