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
        addonDescription = "이 표식이 검게 될 쯤에야 정식 제자로 인정 받는다고";
        addonInfo = "피해량이 25% 증가합니다.";            
    }

    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 1.25f;
    }
}
