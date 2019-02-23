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
		int itemRoomType = room.itemRoomType;
		foreach(ItemSpawnInfo child in itemRoomInformation[itemRoomType - 1].itemSpawnInfo)
		{
			probability -= child.probability;
			if(probability <= 0)
			{
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
						if(room.itemRoomType <= 4)
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
							lifeStoneManager.InstantiateDroppedLifeStone(3 * room.itemRoomType - 4, 1, 0, itemPosition[itemCount++], 1);
						}
					}
					else if (child.itemType[i] == ItemSpawnType.LifeStoneFrame)
						lifeStoneManager.ExpandRow(room.itemRoomType - 4);
				}
				return;
			}
		}
	}

    public override void RoomEnter()
    {
        base.RoomEnter();
		SpawnItem();

        /*switch (room.itemRoomType)
        {
            case 1:
                if (probability < 25)
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Ordinary, itemPosition[0], 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition[0], 1);
                    lifeStoneManager.InstantiatePotion(itemPosition[1], 1);
                    lifeStoneManager.InstantiatePotion(itemPosition[2], 1);
                }
                else if (25 <= probability && probability < 50)
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Ordinary, itemPosition[0], 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition[0], 1);
                    lifeStoneManager.InstantiateDroppedLifeStone(4, 1, 0, itemPosition[1], 1);
                }
                else if (50 <= probability && probability < 67)
                {
                    inventoryManager.ItemInstantiate(ItemQuality.Ordinary, itemPosition[0], 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition[1], 1);
                }
                else if (67 <= probability && probability < 92)
                {
                    inventoryManager.ItemInstantiate(ItemQuality.Ordinary, itemPosition[0], 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Study, itemPosition[1], 1);
                }
                else
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition[0], 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Superior, itemPosition[0], 1);
                }
                break;
            case 2:
                if (probability % 5 == 0)
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition[0], 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Superior, itemPosition[0], 1);
                    lifeStoneManager.InstantiatePotion(itemPosition[1], 1);
                    lifeStoneManager.InstantiatePotion(itemPosition[2], 1);
                    lifeStoneManager.InstantiatePotion(itemPosition[3], 1);
                }
                else if (probability % 5 == 1)
                {
                    inventoryManager.AddonInstantiate(ItemQuality.Superior, itemPosition[0], 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition[1], 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Study, itemPosition[2], 1);
                }
                else if (probability % 5 == 2)
                {
                    inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition[0], 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Study, itemPosition[1], 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Study, itemPosition[2], 1);
                }
                else if (probability % 5 == 3)
                {
                    inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition[0], 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition[1], 1);
                    lifeStoneManager.InstantiateDroppedLifeStone(3, 0, 0, itemPosition[2], 1);
                }
                else if (probability % 5 == 4)
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition[0], 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Superior, itemPosition[0], 1);
                    lifeStoneManager.InstantiateDroppedLifeStone(3, 0, 0, itemPosition[1], 1);
                    lifeStoneManager.InstantiateDroppedLifeStone(3, 0, 0, itemPosition[2], 1);
                    lifeStoneManager.InstantiateDroppedLifeStone(3, 0, 0, itemPosition[3], 1);
                }
                break;
            case 3:
                if (probability < 67)
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Ordinary, itemPosition[0], 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition[0], 1);
                    inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition[1], 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Superior, itemPosition[2], 1);
                    lifeStoneManager.InstantiatePotion(itemPosition[3], 1);
                }
                else
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Masterpiece, itemPosition[0], 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Masterpiece, itemPosition[0], 1);
                }
                break;
            case 4:
                if (probability % 2 == 0)
                    inventoryManager.ItemInstantiate(ItemQuality.Masterpiece, itemPosition[0], 1);
                else
                    inventoryManager.AddonInstantiate(ItemQuality.Masterpiece, itemPosition[0], 1);
                lifeStoneManager.InstantiatePotion(itemPosition[1], 1);
                lifeStoneManager.InstantiatePotion(itemPosition[2], 1);
                break;
            default:
                if (probability % 2 == 0)
                    inventoryManager.ItemInstantiate(ItemQuality.Masterpiece, itemPosition[0], 1);
                else
                    inventoryManager.AddonInstantiate(ItemQuality.Masterpiece, itemPosition[0], 1);
                lifeStoneManager.InstantiateDroppedLifeStone(3 * room.itemRoomType - 4, 0, 0, itemPosition[1], 1);
                lifeStoneManager.ExpandRow(room.itemRoomType - 4);
                break;
        }*/
    }
}
