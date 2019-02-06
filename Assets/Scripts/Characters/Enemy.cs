using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    [SerializeField]
    private float currHealth;

    // [HideInInspector]

    // enemy manager
    private EnemyManager enemyManager;

    // for animation
    [HideInInspector]
    public float playerDistance;
    private Animator animator;

    // drop item
    [HideInInspector]
    public InventoryManager inventoryManager;
    [HideInInspector]
    public int[] dropTable;

    // method
    // Standard Method
    private void Awake()
    {
        enemyManager = EnemyManager.Instance;
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        this.currHealth = maxHealth;
        dropTable = enemyManager.dropTableByID[monsterID];
        Physics2D.IgnoreCollision(enemyManager.player.gameObject.GetComponent<Collider2D>(), transform.parent.GetComponent<Collider2D>());
    }

    private void Update()
    {
        playerDistance = Vector2.Distance(enemyManager.player.transform.position, transform.parent.position);
    }

    // hit by player or debuff
    public void GetDamaged(PlayerAttackInfo attack) { 
        currHealth -= attack.damage;
        if (currHealth <= 0)
        {
            animator.SetTrigger("DeadTrigger");
        }
        else
        {
            animator.SetFloat("knockbackDistance", attack.damage / this.weight * attack.knockBackMultiplier);
            animator.SetTrigger("DamagedTrigger");
        }
    }

    // Animation Event
    // Dead
    public void deadEvent()
    {
        transform.parent.gameObject.SetActive(false);

        // Drop 아이템 결정. 인덱스 별 아이템은 맨 밑에 서술
        float denominator = dropTable[dropTable.Length - 1];
        float numerator = Random.Range(0, denominator);

        int indexOfItem = 0;
        for (int i = 0; i < dropTable.Length; i++)
        {
            if (numerator <= dropTable[i])
            {
                indexOfItem = i;
                break;
            }
        }

        if (indexOfItem == 0) // None
        {
            Debug.Log("None");
        }
        if (indexOfItem >= 1 && indexOfItem <= 5) // Lifestone
        {
            Debug.Log("LifeStone " + indexOfItem);
        }
        if (indexOfItem == 6) // Gold Potion
        {
            Debug.Log("Gold Potion");
            // insert!
        }
        if (indexOfItem == 7) // Amethyst Potion
        {
            Debug.Log("Amethyst Potion");
            // insert!
        }
        if (indexOfItem >= 8 && indexOfItem <= 11) // Item
        {
            Debug.Log("Item" + (ItemQuality)(indexOfItem - 8));
            inventoryManager.ItemInstantiate((ItemQuality)(indexOfItem - 8), transform.parent.position);
        }
        if (indexOfItem >= 12 && indexOfItem <= 15) // Addon
        {
            Debug.Log("Addon" + (ItemQuality)(indexOfItem - 12));
            inventoryManager.AddonInstantiate((ItemQuality)(indexOfItem - 12), transform.parent.position);
        }

        // Pool로 돌아가기 전 Enemy의 상태를 초기화
        this.currHealth = this.maxHealth;

        return;
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

/* Item Drop Index
 * 0 - None
 * 1 - Lifestone 1
 * 2 - Lifestone 2
 * 3 - Lifestone 3
 * 4 - Lifestone 4
 * 5 - Lifestone 5
 * 6 - Gold Potion
 * 7 - Amethyst Potion
 * 8 - Item(Study)
 * 9 - Item(Ordinary)
 * 10 - Item(Superior)
 * 11 - Item(Masterpiece)
 * 12 - Addon(Study)
 * 13 - Addon(Ordinary)
 * 14 - Addon(Superior)
 * 15 - Addon(Masterpiece)
*/
