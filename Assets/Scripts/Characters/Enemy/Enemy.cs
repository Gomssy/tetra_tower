using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

// data

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
    public float currHealth;

    // manager
    private InventoryManager inventoryManager;
    private LifeStoneManager lifeStoneManager;
    private EnemyManager enemyManager;

    // for movement
    private Animator animator;
    public bool Invisible { get; private set; }
    public bool DuringKnockback { get; private set; }
    public float PlayerDistance { get; private set; }
    private readonly float knockbackCritPoint = 0.25f;

    public bool[] WallTest { get; private set; }
    public bool[] CliffTest { get; private set; }

    /*
    public bool[] WallTest { get { return wallTest; } private set { wallTest = value; } } // {left, right}
    public bool[] CliffTest { get { return cliffTest; } private set { cliffTest = value; } } // {left, right}

    [SerializeField]
    private bool[] wallTest;
    [SerializeField]
    private bool[] cliffTest;
    */

    public int MoveDir { get; private set; }

    // drop item
    private int[] dropTable;

    // method
    // Standard Method
    private void Awake()
    {
        enemyManager = EnemyManager.Instance;
        inventoryManager = InventoryManager.Instance;
        lifeStoneManager = GameObject.Find("UI Canvas").transform.GetChild(0).GetComponent<LifeStoneManager>();
        animator = GetComponent<Animator>();

        WallTest = new bool[] { false, false };
        CliffTest = new bool[] { false, false };
    }

    private void Start()
    {
        MoveDir = (int)NumeratedDir.Left;
        currHealth = maxHealth;
        Invisible = DuringKnockback = false;
        dropTable = enemyManager.DropTableByID[monsterID];
        //Physics2D.IgnoreCollision(enemyManager.Player.gameObject.GetComponent<Collider2D>(), transform.parent.GetComponent<Collider2D>());
        PlayerDistance = Vector2.Distance(enemyManager.Player.transform.position, transform.parent.position);
    }

    private void FixedUpdate()
    {
        PlayerDistance = Vector2.Distance(enemyManager.Player.transform.position, transform.parent.position);
        CheckCliff(); CheckWall();
    }

    // check whether enemy is near to cliff
    private void CheckCliff()
    {
        Vector2 velocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
        Vector2 colliderSize = transform.parent.GetComponent<BoxCollider2D>().size;

        foreach (int Dir in Enum.GetValues(typeof(NumeratedDir)))
        {
            Vector2 origin = (Vector2)transform.parent.position + Dir * new Vector2(colliderSize.x / 2.0f, 0);
            Vector2 direction = Vector2.down;
            float distance = colliderSize.y / 4.0f;
            int layerMask = LayerMask.NameToLayer("platform");
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);

            CliffTest[(Dir + 1) / 2] = (hit.collider == null);
        }
    }
    // check whether enemy is touching wall
    private void CheckWall()
    {
        Vector2 colliderSize = transform.parent.GetComponent<BoxCollider2D>().size;

        foreach (int Dir in Enum.GetValues(typeof(NumeratedDir)))
        {
            Vector2 origin = (Vector2)transform.parent.position + new Vector2(Dir * colliderSize.x / 2.0f, colliderSize.y);
            Vector2 direction = Vector2.right * Dir;
            float distance = 0.02f;
            int layerMask = LayerMask.GetMask("Wall", "OuterWall");
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);

            WallTest[(Dir + 1) / 2] = (hit.collider != null);
        }
    }

    // hit by player or debuff
    public void GetDamaged(PlayerAttackInfo attack)
    { 
        currHealth -= attack.damage;
        if (currHealth <= 0)
        {
            Invisible = true;
            animator.SetTrigger("DeadTrigger");
            return;
        }
        float knockbackDist = attack.damage * attack.knockBackMultiplier / weight;
        float knockbackTime = (knockbackDist >= 0.5f) ? 0.5f : knockbackDist;

        if (DuringKnockback)
        {
            StopCoroutine("Knockback");
        }
        StartCoroutine(Knockback(knockbackDist, knockbackTime));

        if (knockbackDist >= knockbackCritPoint)
        {
            animator.SetFloat("knockbackTime", knockbackTime);
            animator.SetTrigger("DamagedTrigger");
        }
    }

    // change direction, and speed of rigidbody of enemy
    public void ChangeVelocityX(float val)
    {
        if (!DuringKnockback)
        {
            Vector2 tempVelocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
            tempVelocity.x = val;
            transform.parent.GetComponent<Rigidbody2D>().velocity = tempVelocity;
        }
    }

    private void SudoChangeVelocityX(float val)
    {
        Vector2 tempVelocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
        tempVelocity.x = val;
        transform.parent.GetComponent<Rigidbody2D>().velocity = tempVelocity;
    }

    public void ChangeDir(object dir)
    {
        if (!DuringKnockback)
        {
            MoveDir = (int)dir;
            transform.parent.eulerAngles = ((NumeratedDir)dir == NumeratedDir.Left) ? new Vector2(0, 0) : new Vector2(0, 180);
        }
    }

    private void SudoChangeDir(object dir)
    {
        MoveDir = (int)dir;
        transform.parent.eulerAngles = ((NumeratedDir)dir == NumeratedDir.Left) ? new Vector2(0, 0) : new Vector2(0, 180);
    }

    // Animation Event
    // Dead
    public void DeadEvent()
    {
        if(transform.parent.GetComponentInChildren<HPBar>())
            transform.parent.GetComponentInChildren<HPBar>().Inactivate();
        transform.parent.gameObject.SetActive(false);
        enemyManager.EnemyDeadCount++; // 다른 enemy로 인해 소환되는 enemy가 추가될 경우 여기를 건드려야 함

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

        // if indexOfItem == 0 then don't drop anything

        if (indexOfItem >= 1 && indexOfItem <= 5) // Lifestone
        {
            lifeStoneManager.InstantiateDroppedLifeStone(
                indexOfItem, EnemyManager.goldPer, EnemyManager.ameNum, transform.parent.position, EnemyManager.dropObjStrength);
        }
        if (indexOfItem == 6) // Gold Potion
        {
            lifeStoneManager.InstantiatePotion(transform.parent.position, EnemyManager.dropObjStrength);
        }
        if (indexOfItem == 7) // Amethyst Potion
        {
            // insert!
        }
        if (indexOfItem >= 8 && indexOfItem <= 11) // Item
        {
            inventoryManager.ItemInstantiate((ItemQuality)(indexOfItem - 8), transform.parent.position, EnemyManager.dropObjStrength);
        }
        if (indexOfItem >= 12 && indexOfItem <= 15) // Addon
        {
            inventoryManager.AddonInstantiate((ItemQuality)(indexOfItem - 12), transform.parent.position, EnemyManager.dropObjStrength);
        }

        currHealth = maxHealth;
        Invisible = false;
        return;
    }

    // Coroutine
    // Knockback
    IEnumerator Knockback(float knockbackDist, float knockbackTime)
    {
        DuringKnockback = true;
        int knockbackDir = (enemyManager.Player.transform.position.x - transform.parent.position.x >= 0) ? -1 : 1;
        float knockbackVelocity = knockbackDir * knockbackDist / knockbackTime;
        SudoChangeDir(knockbackDir * -1);
        SudoChangeVelocityX(knockbackVelocity);
        
        for (float timer = 0; timer <= knockbackTime; timer += Time.deltaTime)
        {
            if (CliffTest[(knockbackDir + 1) / 2])
            {
                SudoChangeVelocityX(0.0f);
                yield return new WaitForSeconds(knockbackTime - timer);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        DuringKnockback = false;
        ChangeVelocityX(0.0f);
    }

    // Debuff
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
