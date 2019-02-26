using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 폭탄마의 장갑
/// 번호: 21
/// </summary>
public class ExplosionGloves : Item
{
    GameObject markPrefab;
    GameObject mark;
    GameObject player;

    public override void Declare()
    {
        id = 21; name = "폭탄마의 장갑";
        quality = ItemQuality.Ordinary;
        skillNum = 2;
        combo = new string[3] { "CAC", "CA", "" };
        attachable = new bool[4] { true, false, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/explosion gloves");
        highlight = Resources.Load<Sprite>("Sprites/Items/explosion gloves_border");
        animation[0] = Resources.Load<AnimationClip>("Animations/explosionGlovesAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/explosionGlovesAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(115f, 117.5f);
        itemInfo = "보기보다 강력한 폭발을 일으킨다. 폭발을 일으킬때마다 장갑이 떨리는게 느껴진다.";
        comboName = new string[3] { "설치", "폭파", "" };
        markPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/ExplosionMark");
        InitMark();
        player = GameManager.Instance.player;
    }

    void InitMark()
    {
        mark = Object.Instantiate(markPrefab);
        mark.SetActive(false);
        mark.GetComponent<AttackProperty>().Init("CA");
    }
    
    protected override void PlaySkill1()
    {
        if (mark == null)
            InitMark();
        GameManager.Instance.StartCoroutine(Plant());
    }
    IEnumerator Plant()
    {
        yield return new WaitForSeconds(0.25f);
        if (!mark.activeSelf)
        {
            mark.transform.position = player.transform.position - new Vector3(0.7f * Mathf.Sign(player.transform.localScale.x), -0.2f);
            mark.transform.SetParent(MapManager.currentRoom.transform);
            mark.GetComponent<BoxCollider2D>().enabled = false;
            mark.SetActive(true);
            mark.GetComponent<Animator>().SetTrigger("plant");
        }
    }

    protected override void PlaySkill2()
    {
        if (mark == null)
            InitMark();
        GameManager.Instance.StartCoroutine(Explode());
    }
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.1f);
        if (mark.activeSelf)
        {
            mark.GetComponent<Animator>().SetTrigger("explode");
        }
    }
}
