using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRoomInGame : RoomInGame {

    public override void RoomEnter()
    {
        base.RoomEnter();
        Room room = transform.parent.GetComponent<Room>();
        InventoryManager inventoryManager = InventoryManager.Instance;
        LifeStoneManager lifeStoneManager = LifeStoneManager.Instance;
        int probability = Random.Range(0, 100);
        Vector3 itemPosition = transform.Find("item spot").position;
        switch (room.itemRoomType)
        {
            case 1:
                if (probability < 25)
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                    lifeStoneManager.InstantiatePotion(itemPosition, 1);
                    lifeStoneManager.InstantiatePotion(itemPosition, 1);
                }
                else if (25 <= probability && probability < 50)
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                    lifeStoneManager.InstantiateDroppedLifeStone(4, 1, 0, itemPosition, 1);
                }
                else if (50 <= probability && probability < 67)
                {
                    inventoryManager.ItemInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                }
                else if (67 <= probability && probability < 92)
                {
                    inventoryManager.ItemInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Study, itemPosition, 1);
                }
                else
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition, 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Superior, itemPosition, 1);
                }
                break;
            case 2:
                if (probability % 5 == 0)
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition, 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Superior, itemPosition, 1);
                    lifeStoneManager.InstantiatePotion(itemPosition, 1);
                    lifeStoneManager.InstantiatePotion(itemPosition, 1);
                    lifeStoneManager.InstantiatePotion(itemPosition, 1);
                }
                else if (probability % 5 == 1)
                {
                    inventoryManager.AddonInstantiate(ItemQuality.Superior, itemPosition, 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Study, itemPosition, 1);
                }
                else if (probability % 5 == 2)
                {
                    inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition, 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Study, itemPosition, 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Study, itemPosition, 1);
                }
                else if (probability % 5 == 3)
                {
                    inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition, 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                    lifeStoneManager.InstantiateDroppedLifeStone(3, 0, 0, itemPosition, 1);
                }
                else if (probability % 5 == 4)
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition, 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Superior, itemPosition, 1);
                    lifeStoneManager.InstantiateDroppedLifeStone(3, 0, 0, itemPosition, 1);
                    lifeStoneManager.InstantiateDroppedLifeStone(3, 0, 0, itemPosition, 1);
                    lifeStoneManager.InstantiateDroppedLifeStone(3, 0, 0, itemPosition, 1);
                }
                break;
            case 3:
                if (probability < 67)
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Ordinary, itemPosition, 1);
                    inventoryManager.ItemInstantiate(ItemQuality.Superior, itemPosition, 1);
                    inventoryManager.AddonInstantiate(ItemQuality.Superior, itemPosition, 1);
                    lifeStoneManager.InstantiatePotion(itemPosition, 1);
                }
                else
                {
                    if (probability % 2 == 0)
                        inventoryManager.ItemInstantiate(ItemQuality.Masterpiece, itemPosition, 1);
                    else
                        inventoryManager.AddonInstantiate(ItemQuality.Masterpiece, itemPosition, 1);
                }
                break;
            case 4:
                if (probability % 2 == 0)
                    inventoryManager.ItemInstantiate(ItemQuality.Masterpiece, itemPosition, 1);
                else
                    inventoryManager.AddonInstantiate(ItemQuality.Masterpiece, itemPosition, 1);
                lifeStoneManager.InstantiatePotion(itemPosition, 1);
                lifeStoneManager.InstantiatePotion(itemPosition, 1);
                break;
            default:
                if (probability % 2 == 0)
                    inventoryManager.ItemInstantiate(ItemQuality.Masterpiece, itemPosition, 1);
                else
                    inventoryManager.AddonInstantiate(ItemQuality.Masterpiece, itemPosition, 1);
                lifeStoneManager.InstantiateDroppedLifeStone(3 * room.itemRoomType - 4, 0, 0, itemPosition, 1);
                lifeStoneManager.ExpandRow(room.itemRoomType - 4);
                break;
        }
    }
}
