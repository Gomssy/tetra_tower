using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour {
    
// data
    // health
    private readonly float maxHealth;
    private readonly float weight;
    public float playerMaxHealth; //다른 스크립트에 있는 플레이어 최대체력 가져와야함
    private float currHealth;

    // unit distance when get damaged
    static readonly float unitDist = 3;

    // debuff
    float[] immunity_time = new float[5] { 0.0f, 3.0f, 6.0f, 6.0f, 6.0f };//면역 시간
    bool[] immunity = new bool[] { false, }; //현재 에너미가 디버프 상태에 대해서 면역인지를 체크하는 변수
    enum debuffCase { fire, ice, stun, blind, charm };
    struct EnemyDebuffed
    {
        public debuffCase Case;
        public float debuffTime;
    }

    // enemy manager
    private readonly EnemyManager enemyManager = EnemyManager.Instance;

	// drop item
	// private readonly EnemyManager.DropItemInfo dropItem; // [item ID, probability]

	// data for ignoring collision
	Vector2 lastPosition;
	Vector2 lastVelocity;
	float lastAngularVelocity;


// method
// constructor
	public Enemy(int id, float maxHealth, float weight) {
        this.maxHealth = maxHealth;
        this.weight = weight;
        this.currHealth = maxHealth;

        // EnemyManager.DropItemInfo dropItem_temp;
        // this.dropItem = (enemyManager.dropTableByID.TryGetValue(id, out dropItem_temp)) ? 
        //                           dropItem_temp : new EnemyManager.DropItemInfo(-1, -1);
    }

	// ignore collision with player
	void FixedUpdate()
	{
		lastPosition = transform.position;
		lastVelocity = GetComponent<Rigidbody2D>().velocity;
		lastAngularVelocity = GetComponent<Rigidbody2D>().angularVelocity;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			transform.position = lastPosition;
			GetComponent<Rigidbody2D>().velocity = lastVelocity;
			GetComponent<Rigidbody2D>().angularVelocity = lastAngularVelocity;
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}
	}

	// hit by player or debuff
	public void GetDamaged(float damage) { 
        currHealth -= damage;
        if(currHealth <= 0) {

			/*
            if (dropItem.id != -1)
            {
                float dropProb = Random.Range(0.0f, 1.0f);
                if (dropProb < dropItem.prob)
                {
                    // spawn a item that has ID
                }
            }
			*/
            Destroy(gameObject);
            return;
        }
        float knockback_dist = damage * unitDist / weight;
        // gameObject.GetComponent<Animator>().SetTrigger("DamagedTrigger");
    }
    

    
    IEnumerator DebuffCase(EnemyDebuffed sCase)
    {

        if (sCase.Case == debuffCase.fire)
        {
            StartCoroutine(OnFire(sCase));
        }

        else if (sCase.Case == debuffCase.ice && !immunity[(int)debuffCase.ice])
        {
            //Enemy 정지하는 코드 필요
            immunity[(int)debuffCase.ice] = true;
            yield return StartCoroutine(DebuffDoing(sCase));
        }

        else if (sCase.Case == debuffCase.stun && !immunity[(int)debuffCase.stun])
        {
            //Enemy 정지하는 코드 필요
            immunity[(int)debuffCase.stun] = true;
            yield return StartCoroutine(DebuffDoing(sCase));
        }

        else if (sCase.Case == debuffCase.blind && !immunity[(int)debuffCase.blind])
        {
            //Enemy의 공격이 적중하지 않는 코드 필요 
            immunity[(int)debuffCase.stun] = true;
            yield return StartCoroutine(DebuffDoing(sCase));
        }

        else if (sCase.Case == debuffCase.charm && !immunity[(int)debuffCase.charm])
        {
            //Enemy 공격이 플레이어 회복하는 코드 필요
            immunity[(int)debuffCase.stun] = true;
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