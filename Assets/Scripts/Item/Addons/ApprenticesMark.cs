using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 수습생의 표식
/// 번호: 1
/// </summary>
public class ApprenticesMark : Addon
{
    public override void Declare()
    {
        id = 1; name = "수습생의 표식";
        quality = ItemQuality.Study;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/apprentice's mark"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/apprentice's mark_border"); ;
        sizeInventory = new Vector2(65, 80);
    }

    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 1.25f;
    }
}
