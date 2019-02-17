using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

// data

    // debuff
    float[] immunity_time = new float[(int)EnemyDebuffCase.END_POINTER] { 0.0f, 3.0f, 6.0f, 6.0f, 6.0f };
    DebuffState[] debuffState;
    float fireDuration = 0.0f;

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
    public bool KnockbackLock { get; private set; }
    public bool DebuffLock { get; private set; }
    public float PlayerDistance { get; private set; }
    private readonly float knockbackCritPoint = 0.25f;

    public int MoveDir { get; private set; }
    public bool[] WallTest { get; private set; }
    public bool[] CliffTest { get; private set; }

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

        DebuffState _temp = DebuffState.Off;
        debuffState = new DebuffState[(int)EnemyDebuffCase.END_POINTER] { _temp, _temp, _temp, _temp, _temp };
    }

    private void Start()
    {
        MoveDir = (int)NumeratedDir.Left;
        currHealth = maxHealth;
        Invisible = KnockbackLock = false;
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
        if (!KnockbackLock && !DebuffLock)
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
        if (!KnockbackLock && !DebuffLock)
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
        KnockbackLock = true;
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
        KnockbackLock = false;
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

        if (KnockbackLock)
        {
            StopCoroutine("Knockback");
        }
        StartCoroutine(Knockback(knockbackDist, knockbackTime));

        if (knockbackDist >= knockbackCritPoint)
        {
            animator.SetFloat("knockbackTime", knockbackTime);
            animator.SetTrigger("DamagedTrigger");
        }

        for(int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++)
        {
            if(attack.debuffTime[i] > 0.0f)
            {
                DebuffApply((EnemyDebuffCase)i, attack.debuffTime[i]);
            }
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
    private void DebuffApply(EnemyDebuffCase debuff, float duration)
    {
        IEnumerator debuffFunc = null;
        int intCase = (int)debuff;
        if (debuffState[intCase] == DebuffState.Immune) return;
        debuffState[intCase] = DebuffState.On;
        switch (debuff)
        {
            case EnemyDebuffCase.Fire:
                if (fireDuration != 0.0f) { fireDuration += duration; }
                else { debuffFunc = OnFire(duration); }
                break;
            case EnemyDebuffCase.Ice:
                debuffFunc = OnIce(duration);
                break;
            case EnemyDebuffCase.Stun:
                debuffFunc = OnStun(duration);
                break;
            case EnemyDebuffCase.Blind:
                debuffFunc = OnBlind(duration);
                break;
            case EnemyDebuffCase.Charm:
                debuffFunc = OnCharm(duration);
                break;
            default:
                break;
        }
        StartCoroutine(debuffFunc);
    }

    // - Debuff coroutine
    IEnumerator OnFire(float duration)
    {
        fireDuration = duration;
        float dotGap = 1.0f;

        while(true)
        {
            fireDuration -= dotGap;
            if (fireDuration <= 0.0f) {
                fireDuration = 0;
                break;
            }
            yield return new WaitForSeconds(1.0f);
            GetDamaged(lifeStoneManager.lifeStoneRowNum * 3);
        }
        debuffState[(int)EnemyDebuffCase.Fire] = DebuffState.Off;
    }

    IEnumerator OnIce(float duration)
    {
        ChangeVelocityX(0.0f);
        DebuffLock = true;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Ice);
    }

    IEnumerator OnStun(float duration)
    {
        ChangeVelocityX(0.0f);
        DebuffLock = true;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Ice);
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

    IEnumerator ImmuneTimer(EnemyDebuffCase Case, float duration)
    {
        debuffState[(int)Case] = DebuffState.Immune;
        yield return new WaitForSeconds(duration);
        debuffState[(int)Case] = DebuffState.Off;
    }

    private void OffDebuff(EnemyDebuffCase Case)
    {
        DebuffLock = false;
        ImmuneTimer(Case, immunity_time[(int)Case]);
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
