using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 라거의 문양
/// 번호: 26
/// </summary>
public class DesignofRagur : Addon
{
    public override void Declare()
    {
        id = 26; name = "라거의 문양";
        quality = ItemQuality.Superior;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/Glowing Herb"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/Glowing Herb"); ;
        sizeInventory = new Vector2(80, 80);
        addonDescription = "자유롭고 , 긍지로우며 , 용맹하게.";
        addonInfo = "콤보에 포함된 A 키의 개수 만큼 피해량이 40% 증가합니다.";
    }

    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        int aNum = 0;
        for (int i = 0; i < combo.Length; i++)
            if (combo[i] == 'A')
                aNum++;

        return 1f + aNum * 0.4f;
    }
}
