using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 감전봉
/// 번호: 52
/// </summary>
public class ShockStick : Item {

    GameObject player;
    bool hit;

    public override void Declare()
    {
        id = 52; name = "감전봉";
        quality = ItemQuality.Superior;
        skillNum = 2;
        combo = new string[3] { "ABA", "CBABB", "" };
        attachable = new bool[4] { false, false, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/shock stick");
        highlight = Resources.Load<Sprite>("Sprites/Items/shock stick_border");
        animation[0] = Resources.Load<AnimationClip>("Animations/shockStickAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/shockStickAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(140f, 140f);
        itemInfo = "적을 2초간 기절시킨다. 충전 5회 적중 시 방전을 사용할 수 있다.";
        comboName = new string[3] { "충전", "방전", "" };

        comboCool = new float[3] { 0, 5, 0 };
        comboCurrentCool = new float[3] { 0, 0, 0 };

        coolSprite[1] = Resources.Load<Sprite>("Sprites/Cools/shock stick_cool2");

        player = GameManager.Instance.player;
        hit = false;
    }

    protected override void PlaySkill1()
    {
        hit = true;
    }

    public override void OtherEffect(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        if(combo.Equals(this.combo[0]) && hit)
        {
            hit = false;
            comboCurrentCool[1]++;
        }
    }

    protected override void PlaySkill2()
    {
        GameManager.Instance.StartCoroutine(Emit());
    }

    IEnumerator Emit()
    {
        yield return new WaitForSeconds(0.75f);
        Enemy[] enemyArray = MapManager.currentRoom.GetComponentsInChildren<Enemy>();

        foreach (Enemy enemy in enemyArray)
        {
            if(Vector3.Distance(enemy.gameObject.transform.position,player.transform.position) <= 6f)
            {
                PlayerAttackInfo attack = new PlayerAttackInfo(30f, 0f, new float[(int)EnemyDebuffCase.END_POINTER] { 0, 0, 2, 0, 0 });
                AttackCalculation(attack, enemy, combo[1]);
                enemy.GetHit(attack);
                EffectManager.Instance.StartEffect(0, enemy.gameObject.transform.position);
                EffectManager.Instance.StartNumber(1, enemy.gameObject.transform.position, attack.damage);
            }
        }


    }
}
