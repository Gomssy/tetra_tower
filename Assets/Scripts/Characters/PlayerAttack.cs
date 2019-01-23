using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAttack : MonoBehaviour {
    public ComboState state;
    public float attackA,attackB,attackC;
    public float cancel;
    public Text time, combo;
    public static string comboArray;
    public static float StartTime;
    public bool keyCoolDown=true;
    public Animator anim;
    public AttackCombo[] AttackArr= { new AttackCombo("화염발사", "ABC", 1.5f,"PlayerRunAnim"),
    new AttackCombo("공격A", "A", 0.5f,"PlayerGoingDownAnim"),
    new AttackCombo("공격B", "B", 0.5f,"PlayerWalkAnim"),
    new AttackCombo("공격C", "C", 0.5f,"PlayerGoingUpAnim"),
    new AttackCombo("콩", "AC", 1f,"PlayerIdleAnim"),
    new AttackCombo("콩콩콩", "ACB", 2f,"PlayerIdleAnim"),
};
    public Queue comboQueue = new Queue();

    // Use this for initialization
    void Start () {
        StartTime = Time.time;
        state = ComboState.Idle;
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	
    void Update()
    {

        attackA = Input.GetAxisRaw("Fire1");
        attackB = Input.GetAxisRaw("Fire2");
        attackC = Input.GetAxisRaw("Fire3");
        cancel = Input.GetAxisRaw("stop");
        combo.text = comboArray;
        float tempTime = Mathf.Clamp(StartTime - Time.time+1f , 0f, 9999f) ;
        foreach (AttackCombo c in comboQueue)
        {
            tempTime += c.getTime();
        }
        
        time.text=Mathf.Round(tempTime*10f)/10f+"";
           

        if (attackA + attackB + attackC == 0)
        {
            keyCoolDown = true;
        }
        
        if (state == ComboState.Idle)
        {
            
            if (attackA + attackB + attackC > 0 && keyCoolDown)
            {
               
                state = ComboState.Attack;
                StartTime = Time.time;

                ComboCheck();

            }
            
        }
        else if (state == ComboState.Attack)
        {
            ComboCheck();
            //공격중일때

            if (Time.time > StartTime)
            {
                state = ComboState.Combo;
            }
            
          
        }

       else if (state == ComboState.Combo)
        {
            ComboCheck();
            if (comboQueue.Count > 0) //콤보가 남아있다면
            {

                AttackCombo cur = (AttackCombo)comboQueue.Dequeue();
                print(cur);
                anim.Play(cur.getComboAnim());
                //실제로는 애니메이션 가져옴
                state = ComboState.Attack;
                StartTime = Time.time + cur.getTime();
            }
            else if (Time.time > StartTime + 1f || cancel==1)
            {
                //현재 시간이 마지막 콤보 끝나는 시점보다 1초 지났다면
                state = ComboState.Idle;
                comboArray = "";
            }
            
            

        }

        if (attackA + attackB + attackC > 0)
        {
            keyCoolDown = false;
        }
    }


    void ComboCheck()
    {
        //들어갈 콤보가 있는지 확인함
        if (attackA + attackB + attackC > 0 && keyCoolDown)
        {
            if (attackA == 1)
            {
                comboArray += "A";
            }
            else if (attackB == 1)
            {
                comboArray += "B";
            }
            else if (attackC == 1)
            {
                comboArray += "C";
            }
            bool success = false;
            foreach (AttackCombo com in AttackArr)
            {
                if (com.Equals(comboArray))
                {
                    
                    comboQueue.Enqueue(com);
                    success = true;
                }
                
            }
            if (success==false) //콤보 실행에 실패했다면
            {
                string temp = comboArray[comboArray.Length - 1]+"";
                foreach (AttackCombo com2 in AttackArr) //기본 글자 콤보 실행
                {
                    if (com2.Equals(temp))
                    {
                        comboQueue.Enqueue(com2);
                    }
                   
                }
            }
        }
    }
}
