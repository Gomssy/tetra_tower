using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 모래 주머니
/// 번호: 29
/// </summary>
public class Sandbag : Addon
{
    public override void Declare()
    {
        id = 29; name = "모래 주머니";
        quality = ItemQuality.Superior;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        sizeInventory = new Vector2(80, 80);
    }
    public override void OtherEffect(string combo)
    {
        GameManager.Instance.player.GetComponent<PlayerAttack>().comboTime *= 0.5f;
    }
    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 2f;
    }
}