using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 비트 메모리
/// 번호: 36
/// </summary>
public class BitMemory : Addon
{
    float damageStack;
    public override void Declare()
    {
        id = 36; name = "비트 메모리";
        quality = ItemQuality.Masterpiece;
        type = AddonType.Component;
        sprite = Resources.Load<Sprite>("Sprites/Addons/bit memory");
        highlight = Resources.Load<Sprite>("Sprites/Addons/bit memory_border");
        sizeInventory = new Vector2(80, 80);
        addonInfo = "500 = item";
        damageStack = 0;
    }

    public override void OtherEffect(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        damageStack += attackInfo.damage;
        if(damageStack >= 500)
        {
            MapManager.Instance.UpgradeRoom(RoomType.Item);
            damageStack = 0;
        }
    }
}
