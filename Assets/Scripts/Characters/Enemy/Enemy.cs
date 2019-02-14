using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

// data

    // debuff
    float[] immunity_time = new float[5] { 0.0f, 3.0f, 6.0f, 6.0f, 6.0f };//면역 시간
    bool[] immunity = new bool[] { false, }; //현재 에너미가 디버프 상태에 대해서 면역인지를 체크하는 변수
    struct EnemyDebuff
    {
        public EnemyDebuffCase Case;
        public float Duration;
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

    public int MoveDir { get; private set; }
    public bool[] WallTest { get; private set; }
    public bool[] CliffTest { get; private set; }

    /* Inspector에서 WallTest와 CliffTest를 확인해보고 싶으면 주석을 풀으시오
    public bool[] WallTest { get { return wallTest; } private set { wallTest = value; } } // {left, right}
    public bool[] CliffTest { get { return cliffTest; } private set { cliffTest = value; } } // {left, right}

    [SerializeField]
    private bool[] wallTest;
    [SerializeField]
    private bool[] cliffTest;
    */

    // drop item
    private int[] dropTable;

// method
    // Standard method
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

    // Movement & Physics

    // - Check whether enemy is near to cliff
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

    // - Check whether enemy is touching wall
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

    // - Change direction, and speed of rigidbody of enemy
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

    // - Knockback coroutine
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

    // When damaged

    // - Calculate value & Arrange information
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

    public void GetDamaged(float damage)
    {
        currHealth -= damage;
        if (currHealth <= 0)
        {
            Invisible = true;
            animator.SetTrigger("DeadTrigger");
            return;
        }
    }

    // - Apply debuff
    private void DebuffApply(EnemyDebuff debuff)
    {
        IEnumerator debuffFunc = null;
        switch (debuff.Case)
        {
            case EnemyDebuffCase.fire:
                debuffFunc = OnFire(debuff.Duration);
                break;
            case EnemyDebuffCase.ice:
                debuffFunc = OnIce(debuff.Duration);
                break;
            case EnemyDebuffCase.stun:
                debuffFunc = OnStun(debuff.Duration);
                break;
            case EnemyDebuffCase.blind:
                debuffFunc = OnBlind(debuff.Duration);
                break;
            case EnemyDebuffCase.charm:
                debuffFunc = OnCharm(debuff.Duration);
                break;
            default:
                break;
        }
        StartCoroutine(debuffFunc);
    }

    // - Debuff coroutine
    IEnumerator OnFire(float duration)
    {
        int dotCount = 0;

        while(true)
        {
            dotCount += 1;
            if (duration < dotCount) { break; }
            yield return new WaitForSeconds(1.0f);
            GetDamaged(lifeStoneManager.lifeStoneRowNum * 3);
        }
    }

    IEnumerator OnIce(float duration)
    {
        yield return null;
    }

    IEnumerator OnStun(float duration)
    {
        yield return null;
    }

    IEnumerator OnBlind(float duration)
    {
        yield return null;
    }

    IEnumerator OnCharm(float duration)
    {
        yield return null;
    }

    IEnumerator ImmunityTimer(EnemyDebuff sCase)
	{
		yield return new WaitForSeconds(immunity_time[(int)sCase.Case]);
		immunity[(int)sCase.Case] = false;
	}

    // Animation Event

    // - When dead
    public void DeadEvent()
    {
        if (transform.parent.GetComponentInChildren<HPBar>())
            transform.parent.GetComponentInChildren<HPBar>().Inactivate();
        transform.parent.gameObject.SetActive(false);
        StopAllCoroutines();
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
}

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
