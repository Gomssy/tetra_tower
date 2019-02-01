using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAttack : MonoBehaviour {
    public float[] attackRaw = new float[3];
    public int[] attackKeyState = new int[3]; //0: released 1: push 2: pushing
    public float cancelRaw;
    public int cancelKeyState;
    public bool playingSkill;
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
            attackRaw[i] = Input.GetAxisRaw("Fire" + (i+1));
        cancelRaw = Input.GetAxisRaw("Stop");

        for (int i = 0; i < 3; i++)
        {
            if (attackRaw[i] > 0 && attackKeyState[i] < 2)
                attackKeyState[i]++;
            else if (attackRaw[i] == 0)
                attackKeyState[i] = 0;
        }
        if (cancelRaw > 0 && cancelKeyState < 2)
            cancelKeyState++;
        else if (cancelRaw == 0)
            cancelKeyState = 0;

        if (cancelKeyState == 1)
        {
            comboTimeOn = false;
        }

        if (!playingSkill)
        {
            for (int i = 0; i < 3; i++)
                if (attackKeyState[i] == 1)
                {
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

    public void SkillEnd()
    {
        if (CheckLongerCombo()) StartCoroutine(SkillEndCoroutine());
        else
        {
            comboArray = "";
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
                    playerController.playerState = PlayerState.Attack;
                    aoc["PlayerAttackAnim"] = item.animation[i];
                    anim.SetTrigger("attack");
                    item.ComboAction(i);
                    playingSkill = true;
                    GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x,-3f,3f), 0);
                    return;
                }
            }
        }

        playerController.playerState = PlayerState.Attack;
        aoc["PlayerAttackAnim"] = normalAttack[comboArray[comboArray.Length - 1] - 'A'];
        anim.SetTrigger("attack");
        playingSkill = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x, -3f, 3f), 0);
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
