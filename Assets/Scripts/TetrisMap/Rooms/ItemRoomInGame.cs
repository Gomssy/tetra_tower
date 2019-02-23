using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRoomInGame : RoomInGame {
	
	public static RoomItemInfo[] itemRoomInformation = new RoomItemInfo[5];

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
		int itemRoomIndex = room.itemRoomType;
		if (itemRoomIndex > 5)
			itemRoomIndex = 5;
		for(int index = 0; index < itemRoomInformation[itemRoomIndex - 1].itemSpawnInfo.Count; index++)
		{
			ItemSpawnInfo child = itemRoomInformation[itemRoomIndex - 1].itemSpawnInfo[index];
			probability -= child.probability;
			Debug.Log(probability);
			if (probability <= 0)
			{
				Debug.Log("Item Spawn");
				int itemCount = 0;
				for(int i = 0; i < child.itemType.Length; i++)
				{
					if (child.itemType[i] == ItemSpawnType.Item) 
						for(int _amount = 0; _amount < child.amount[i]; _amount++)
						{
							Debug.Log("type" + child.itemType[i] + " quality" + child.itemQuality[i] + " amount" + child.amount[i]);
							inventoryManager.ItemInstantiate(child.itemQuality[i], itemPosition[itemCount++], 1);
						}
					else if (child.itemType[i] == ItemSpawnType.Addon)
						for (int _amount = 0; _amount < child.amount[i]; _amount++)
						{
							Debug.Log("type" + child.itemType[i] + " quality" + child.itemQuality[i] + " amount" + child.amount[i]);
							inventoryManager.AddonInstantiate(child.itemQuality[i], itemPosition[itemCount++], 1);
						}
					else if (child.itemType[i] == ItemSpawnType.GoldPotion)
						for (int _amount = 0; _amount < child.amount[i]; _amount++)
						{
							Debug.Log("type" + child.itemType[i] + " quality" + child.itemQuality[i] + " amount" + child.amount[i]);
							lifeStoneManager.InstantiatePotion(itemPosition[itemCount++], 1);
						}
					else if (child.itemType[i] == ItemSpawnType.LifeStone)
					{
						if(room.itemRoomType < 4)
							for (int _amount = 0; _amount < child.amount[i]; _amount++)
							{
								if(child.itemQuality[i] == ItemQuality.Gold)
								{
									Debug.Log("type" + child.itemType[i] + " quality" + child.itemQuality[i] + " amount" + child.amount[i]);
									lifeStoneManager.InstantiateDroppedLifeStone(4, 1, 0, itemPosition[itemCount++], 1);
								}
								else
								{
									Debug.Log("type" + child.itemType[i] + " quality" + child.itemQuality[i] + " amount" + child.amount[i]);
									lifeStoneManager.InstantiateDroppedLifeStone(3, 0, 0, itemPosition[itemCount++], 1);
								}
							}
						else
						{
							Debug.Log("type" + child.itemType[i] + " quality" + child.itemQuality[i] + " amount" + child.amount[i]);
							lifeStoneManager.InstantiateDroppedLifeStone(3 * (room.itemRoomType - 4), 1, 0, itemPosition[itemCount++], 1);
						}
					}
					else if (child.itemType[i] == ItemSpawnType.LifeStoneFrame)
					{
						Debug.Log("type" + child.itemType[i] + " quality" + child.itemQuality[i] + " amount" + child.amount[i]);
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
}
