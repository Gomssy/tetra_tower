using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 비트 검
/// 번호: 43
/// </summary>
public class BitSword: Item
{

    public override void Declare()
    {
        id = 43; name = "비트 검";
        quality = ItemQuality.Masterpiece;
        skillNum = 1;
        combo = new string[3] { "A", "", "" };
        attachable = new bool[4] { false, false, false, false };
        sprite = Resources.Load<Sprite>("Sprites/Items/bit sword");
        highlight = Resources.Load<Sprite>("Sprites/Items/bit sword_border");
        animation[0] = Resources.Load<AnimationClip>("Animations/bitSwordAttack1");
        animation[1] = null;
        animation[2] = null;
        sizeInventory = new Vector2(135f, 135f);
        itemInfo = "";
        comboName = new string[3] { "I", "", "" };
        comboCool = new float[3] { 50, 0, 0 };
        comboCurrentCool = new float[3] { 50, 0, 0 };
    }

    public override void GlobalOtherEffect(string combo)
    {
        List<Item> itemList = InventoryManager.Instance.itemList;

        if (MapManager.currentRoom.isRoomCleared)
        {
            for (int i = combo.Length; i >= 1; i--)
            {
                if (combo[i - 1] == 'A') comboCurrentCool[0]++;

                foreach (Item item in itemList)
                {
                    for (int j = 0; j < item.skillNum; j++)
                        if (item.combo[j].Equals(combo.Substring(0, i - 1))) return;
                }
            }
        }
    }
}
