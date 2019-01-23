using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public ComboState state;
    public float attackA,attackB,attackC;
    public float cancel;
    public string comboArray;
    public float StartTime;
    public bool keyCoolDown=true;
    public AttackCombo[] AttackArr= { new AttackCombo("화염발사", "ABC", 1.5f),
    new AttackCombo("공격A", "A", 0.5f),
    new AttackCombo("공격B", "B", 0.5f),
    new AttackCombo("공격C", "C", 0.5f),
    new AttackCombo("콩", "AC", 1f),
    new AttackCombo("콩콩콩", "ACB", 2f),
};
    public Queue comboQueue = new Queue();

    // Use this for initialization
    void Start () {
        StartTime = Time.time;
        state = ComboState.Idle;
    }
	
	// Update is called once per frame
	
    void Update()
    {

        attackA = Input.GetAxisRaw("Fire1");
        attackB = Input.GetAxisRaw("Fire2");
        attackC = Input.GetAxisRaw("Fire3");
        cancel = Input.GetAxisRaw("stop");
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
