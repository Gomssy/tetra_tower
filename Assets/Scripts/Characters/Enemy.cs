using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour {

// data
    // static
    static readonly float unitDist = 3;

    // debuff
    float[] immunity_time = new float[5] { 0.0f, 3.0f, 6.0f, 6.0f, 6.0f };//면역 시간
    bool[] immunity = new bool[] { false, }; //현재 에너미가 디버프 상태에 대해서 면역인지를 체크하는 변수
    struct EnemyDebuffed
    {
        public EnemyDebuffCase Case;
        public float debuffTime;
    }

    // stat
    public int monsterID;
    public float maxHealth;
    public float weight;
    public float patrolRange;
    public float noticeRange;
    public float attackRange;
    public float patrolSpeed;
    public float trackSpeed;
    
    private float playerMaxHealth; //다른 스크립트에 있는 플레이어 최대체력 가져와야함
    private float currHealth;

    // [HideInInspector]

    // enemy manager
    private EnemyManager enemyManager;

    // for animation
    [HideInInspector]
    public float playerDistance;
    public bool gotKnockback;
    RuntimeAnimatorController ac;

    // drop item


    // method
    // Standard Method
    private void Awake()
    {
        enemyManager = EnemyManager.Instance;
        ac = GetComponent<Animator>().runtimeAnimatorController;
    }

    private void Start()
    {
        this.currHealth = maxHealth;
        playerDistance = Vector2.Distance(enemyManager.player.transform.position, transform.parent.position);
    }

    private void Update()
    {
        playerDistance = Vector2.Distance(enemyManager.player.transform.position, transform.parent.position);
        if (gotKnockback)
        {

        }
    }

    // hit by player or debuff
    public void GetDamaged(float damage) { 
        currHealth -= damage;
        if(currHealth <= 0) {
            gameObject.SetActive(false);
            return;
        }
        gameObject.GetComponent<Animator>().SetTrigger("DamagedTrigger");
    }

    IEnumerator DebuffCase(EnemyDebuffed sCase)
    {

        if (sCase.Case == EnemyDebuffCase.fire)
        {
            StartCoroutine(OnFire(sCase));
        }

        else if (sCase.Case == EnemyDebuffCase.ice && !immunity[(int)EnemyDebuffCase.ice])
        {
            //Enemy 정지하는 코드 필요
            immunity[(int)EnemyDebuffCase.ice] = true;
            yield return StartCoroutine(DebuffDoing(sCase));
        }

        else if (sCase.Case == EnemyDebuffCase.stun && !immunity[(int)EnemyDebuffCase.stun])
        {
            //Enemy 정지하는 코드 필요
            immunity[(int)EnemyDebuffCase.stun] = true;
            yield return StartCoroutine(DebuffDoing(sCase));
        }

        else if (sCase.Case == EnemyDebuffCase.blind && !immunity[(int)EnemyDebuffCase.blind])
        {
            //Enemy의 공격이 적중하지 않는 코드 필요 
            immunity[(int)EnemyDebuffCase.stun] = true;
            yield return StartCoroutine(DebuffDoing(sCase));
        }

        else if (sCase.Case == EnemyDebuffCase.charm && !immunity[(int)EnemyDebuffCase.charm])
        {
            //Enemy 공격이 플레이어 회복하는 코드 필요
            immunity[(int)EnemyDebuffCase.stun] = true;
            yield return StartCoroutine(DebuffDoing(sCase));
        }

    }

    IEnumerator OnFire(EnemyDebuffed sCase)
    {
        for (int i = 0; i < sCase.debuffTime / 1; i++)
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