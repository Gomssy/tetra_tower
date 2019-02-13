using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAttack : MonoBehaviour {
    public bool[] attack = new bool[3];
    public bool cancel;
    public bool playingSkill;
    private bool comboEndDelay = true;
    public float comboTime;
    public Text time, combo;
    public string comboArray;
    public float StartTime;
    public Animator anim;
    public AnimatorOverrideController aoc;
    public AnimationClip[] normalAttack = new AnimationClip[3];
    public InventoryManager inventoryManager;
    public LifeStoneManager lifeStoneManager;

    float comboEndTime;
    bool comboTimeOn;
    PlayerController playerController;

    
    void Awake ()
    {
        playerController = GetComponent<PlayerController>();
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
                    CheckCombo();
                    SetComboText();
                    break;
                }
        }
    }

    public void SetComboText()
    {
        string conString = "";
        if (comboArray.Equals(""))
        {
            combo.text = "";
            return;
        }
        conString += comboArray[0];
        for (int i = 1; i < comboArray.Length; i++)
            conString += " " + comboArray[i];
        combo.text = conString;
    }

    public void SetTimeText(float fullTime, float currentTime)
    {
        if (comboTimeOn)
        {
            for (int i = 0; i < 20; i++)
            {
                if (currentTime / fullTime < (i + 1) * 0.05f)
                {
                    string str = "";
                    for (int j = 0; j < i + 1; j++) str += "-";
                    time.text = str;
                    break;
                }
            }
        }
        else
        {
            time.text = "";
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
        yield return new WaitForSeconds(1.5f);
        SetComboText();
    }
    IEnumerator SkillEndCoroutine()
    {
        comboEndTime = Time.time + comboTime;
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
        foreach(Item item in itemList)
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
        if (!CheckLongerCombo()) comboArray = comboArray[comboArray.Length - 1] + "";

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
