using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAttack : MonoBehaviour {
    public bool[] attack = new bool[3];
    public bool cancel;
    public bool playingSkill;
    private bool comboEndDelay = true;
    public float originComboTime;
    public float comboTime;
    public string comboArray;
    public float StartTime;
    public Animator anim;
    public AnimatorOverrideController aoc;
    public AnimationClip[] normalAttack = new AnimationClip[3];
    public ComboUI comboUI;
    InventoryManager inventoryManager;
    LifeStoneManager lifeStoneManager;

    float comboEndTime;
    bool comboTimeOn;
    PlayerController playerController;
    AttackProperty attackProperty;

    
    void Awake ()
    {
        lifeStoneManager = LifeStoneManager.Instance;
        inventoryManager = InventoryManager.Instance;
        playerController = GetComponent<PlayerController>();
        attackProperty = GetComponentInChildren<AttackProperty>();
        anim = GetComponent<Animator>();
        aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = aoc;
    }
	
    void Update()
    {
        SetTimeText(comboTime, comboEndTime - Time.time);
        for (int i = 0; i < 3; i++)
            attack[i] = Input.GetButtonDown("Fire" + (i+1));
        cancel = Input.GetButtonDown("Stop");
        
        if (cancel)
        {
            comboTimeOn = false;
        }

        if (!playingSkill && comboEndDelay)
        {
            for (int i = 0; i < 3; i++)
                if (attack[i])
                {
                    if (playerController.playerState == PlayerState.GoingUp || playerController.playerState == PlayerState.GoingDown)
                        playerController.airAttack = false;
                    comboArray += (char)('A' + i);
                    attackProperty.Init(comboArray);
                    CheckCombo();
                    SetComboText();
                    break;
                }
        }
    }

    public void SetComboText()
    {
        bool comboFail = true;
        List<Item> itemList = inventoryManager.itemList;

        foreach (Item item in itemList)
            for (int i = 0; i < item.skillNum; i++)
                if (comboArray.Equals(item.combo[i]))
                    comboFail = false;

        if (CheckLongerCombo()) comboFail = false;

        if (comboArray.Length == 1) comboFail = false;

        comboUI.SetCombo(comboArray, comboFail);
    }

    public void SetTimeText(float fullTime, float currentTime)
    {
        if (comboTimeOn)
        {
            comboUI.SetTime(currentTime, fullTime);
        }
        else
        {
            comboUI.SetTime();
        }
    }
    IEnumerator ComboEndDelay()
    {
        comboEndDelay = false;
        yield return new WaitForSeconds(0.3f);
        comboEndDelay = true;
    }
    public void SkillEnd()
    {
        if (CheckLongerCombo()) StartCoroutine(SkillEndCoroutine());
        else
        {
            comboArray = "";
            StartCoroutine(ComboEndDelay());
            StartCoroutine(ComboTextReset());
        }
    }
    IEnumerator ComboTextReset()
    {
        for(float timer = 0f; timer < 1.5f; timer += Time.deltaTime)
        {
            yield return null;
            if (playingSkill) break;
        }
        if(comboArray.Equals("") && !playingSkill)
            SetComboText();
    }
    IEnumerator SkillEndCoroutine()
    {
        comboEndTime = Time.time + comboTime;
        comboTime = originComboTime;
        comboTimeOn = true;
        while (Time.time < comboEndTime && comboTimeOn && !playingSkill)
        {
            yield return null;
        }
        if (!playingSkill)
        {
            comboArray = "";
            StartCoroutine(ComboEndDelay());
            SetComboText();
        }
        comboTimeOn = false;
    }

    void CheckCombo()
    {
        List<Item> itemList = inventoryManager.itemList;

        
        foreach (Item item in itemList)
        {
            for(int i=0; i< item.skillNum; i++)
            {
                if(item.combo[i].Equals(comboArray))
                {
                    aoc["PlayerAttackAnim"] = item.animation[i];
                    anim.SetTrigger("attack");
                    item.ComboAction(i);
                    playingSkill = true;
                    if (playerController.playerState != PlayerState.GoingUp && playerController.playerState != PlayerState.GoingDown)
                        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x,-0.5f,0.5f), GetComponent<Rigidbody2D>().velocity.y);
                    playerController.playerState = PlayerState.Attack;
                    
                    return;
                }
            }
        }
        
        aoc["PlayerAttackAnim"] = normalAttack[comboArray[comboArray.Length - 1] - 'A'];
        anim.SetTrigger("attack");
        playingSkill = true;
        if (playerController.playerState != PlayerState.GoingUp && playerController.playerState != PlayerState.GoingDown)
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x, -0.5f, 0.5f),GetComponent<Rigidbody2D>().velocity.y);
        playerController.playerState = PlayerState.Attack;
        
    }

    bool CheckLongerCombo()
    {
        List<Item> itemList = inventoryManager.itemList;
        foreach(Item item in itemList)
            for (int i = 0; i < item.skillNum; i++)
                if (item.combo[i].Length > comboArray.Length && item.combo[i].Substring(0, comboArray.Length).Equals(comboArray))
                    return true;
        return false;
    }

    public void TakeDamage(EnemyAttackInfo attack)
    {
        lifeStoneManager.DestroyStone((int)Mathf.Ceil(attack.damage));
    }
}
