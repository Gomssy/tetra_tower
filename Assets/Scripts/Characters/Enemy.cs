using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour {

    private readonly float maxHealth;
    private readonly float weight;
    public float playerMaxHealth; //다른 스크립트에 있는 플레이어 최대체력 가져와야함
    private float currHealth;
    float[] immunity_time = new float[5] { 0.0f, 3.0f, 6.0f, 6.0f, 6.0f };//면역 시간
    bool[] immunity = new bool[] { false, }; //현재 에너미가 디버프 상태에 대해서 면역인지를 체크하는 변수
    enum debuffCase { fire, ice, stun, blind, charm};
    private EnemyManager.State currState;
    private List<int[]> dropTable; // [item ID, numerator]
    private Dictionary<EnemyManager.State, EnemyManager.Action<int>> actionByState;
    public Enemy(uint id, float maxHealth, float weight) {
        this.maxHealth = maxHealth;
        this.weight = weight;
        this.currHealth = maxHealth;
        this.dropTable = GetDropTable(id);
        this.actionByState = GetActionByState(id);
        this.currState = EnemyManager.State.Idle;
    }
    public void GetDamaged(float damage) {
        float unitDist = 3;
        currHealth -= damage;
        if(currHealth <= 0) {
            mamaWooWooWoo_I_Dont_Wanna_Die();
            return;
        }
        float knockback_dist = damage * unitDist / weight;
        // do something - knockback animation

    }
    private List<int[]> GetDropTable(uint id) {
        List<int[]> resultList = new List<int[]>();
        return resultList;
    }
    private Dictionary<EnemyManager.State, EnemyManager.Action<int>> GetActionByState(uint id) {
        var resultDictionary = new Dictionary<EnemyManager.State, EnemyManager.Action<int>>();
        return resultDictionary;
    }
    private void mamaWooWooWoo_I_Dont_Wanna_Die() {
        int dropItemId = -1;
        if (dropTable != null) {
            float denominator = dropTable[dropTable.Count - 1][1];
            float numerator = UnityEngine.Random.Range(0f, denominator);

            foreach (var drop in dropTable) {
                if (numerator <= drop[1]) {
                    dropItemId = drop[0];
                    break;
                }
            }
        }
        // spawn a item that has ID

        // destroy itself (or, deactivate for pooling)
        Destroy(gameObject);
    }

    struct EnemyDebuffed
    {
        public debuffCase Case;
        public float debuffTime;
    }
    IEnumerator DebuffCase(EnemyDebuffed sCase)
    {   
      
            if(sCase.Case == debuffCase.fire)
            {
                StartCoroutine(OnFire(sCase));
            }

            else if(sCase.Case == debuffCase.ice && !immunity[(int)debuffCase.ice])
            {
                //Enemy 정지하는 코드 필요
                immunity[(int)debuffCase.ice] = true;
                yield return StartCoroutine(DebuffDoing(sCase));
            }

            else if(sCase.Case == debuffCase.stun && !immunity[(int)debuffCase.stun])
            {
                //Enemy 정지하는 코드 필요
                immunity[(int)debuffCase.stun] = true;
                yield return StartCoroutine(DebuffDoing(sCase));
            }

            else if(sCase.Case == debuffCase.blind && !immunity[(int)debuffCase.blind])
            {
                //Enemy의 공격이 적중하지 않는 코드 필요 
                immunity[(int)debuffCase.stun] = true;
                yield return StartCoroutine(DebuffDoing(sCase));
            }

            else if(sCase.Case == debuffCase.charm && !immunity[(int)debuffCase.charm])
            {
                //Enemy 공격이 플레이어 회복하는 코드 필요
                immunity[(int)debuffCase.stun] = true;
                yield return StartCoroutine(DebuffDoing(sCase));
            }
        
    }

    IEnumerator OnFire(EnemyDebuffed sCase)
    {
        for(int i=0;i<sCase.debuffTime/1;i++)
        {
            yield return new WaitForSeconds(1.0f);
            currHealth = currHealth - playerMaxHealth / 10;           
        }
    }

    IEnumerator DebuffDoing(EnemyDebuffed sCase)
    {
        yield return new WaitForSeconds(sCase.debuffTime);

        yield return StartCoroutine(DebuffEnd(sCase));

    }

    IEnumerator DebuffEnd(EnemyDebuffed sCase)
    {
        //다시 동작하는 코드 필요

        yield return StartCoroutine(ImmunityTimer(sCase));
    }

    IEnumerator ImmunityTimer(EnemyDebuffed sCase)
    {
        yield return new WaitForSeconds(immunity_time[(int)sCase.Case]);
        immunity[(int)sCase.Case] = false;
    }
}

//얼음일때 깨어나기
//공격이 적중했을 때 매혹에서 깨어나기