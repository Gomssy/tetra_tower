using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

    // data

    // debuff
    readonly float[] immunity_time = new float[(int)EnemyDebuffCase.END_POINTER] { 0.0f, 3.0f, 6.0f, 6.0f, 6.0f };
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
    public float[] knockbackPercentage;
    public float currHealth { get; private set; }

    // manager
    private InventoryManager inventoryManager;
    private LifeStoneManager lifeStoneManager;
    private EnemyManager enemyManager;

    // for movement
    private Animator animator;
    private float damagedAnimLength;
    public bool Invisible { get; private set; }
    public bool MovementLock { get; private set; }
    public bool KnockbackLock { get; private set; }
    public float PlayerDistance { get; private set; }

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
        lifeStoneManager = LifeStoneManager.Instance;
        animator = GetComponent<Animator>();

        WallTest = new bool[] { false, false };
        CliffTest = new bool[] { false, false };

        DebuffState _temp = DebuffState.Off;
        debuffState = new DebuffState[(int)EnemyDebuffCase.END_POINTER] { _temp, _temp, _temp, _temp, _temp };

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips) { if (clip.name.Contains("Damaged")) { damagedAnimLength = clip.length; } }

        Array.Sort(knockbackPercentage);
        Array.Reverse(knockbackPercentage);
    }

    private void Start()
    {
        MoveDir = (int)NumeratedDir.Left;
        currHealth = maxHealth;
        Invisible = MovementLock = false;
        dropTable = enemyManager.DropTableByID[monsterID];
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
            float distance = 0.5f;
            LayerMask layerMask = LayerMask.GetMask("Wall", "OuterWall");
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);

            WallTest[(Dir + 1) / 2] = (hit.collider != null);
        }
    }

    // - Change direction, and speed of rigidbody of enemy
    public void ChangeVelocityX(float val)
    {
        ChangeVelocityX_lock(val, new bool[] { MovementLock, KnockbackLock });
    }

    private void ChangeVelocityX_lock(float val, bool[] lockArray)
    {
        foreach(var Lock in lockArray) { if (Lock) return; }
        Vector2 tempVelocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
        tempVelocity.x = val;
        transform.parent.GetComponent<Rigidbody2D>().velocity = tempVelocity;
    }

    public void ChangeDir(object dir)
    {
        ChangeDir_lock(dir, new bool[] { MovementLock, KnockbackLock });
    }

    private void ChangeDir_lock(object dir, bool[] lockArray)
    {
        foreach (var Lock in lockArray) { if (Lock) return; }
        MoveDir = (int)dir;
        transform.parent.eulerAngles = ((NumeratedDir)dir == NumeratedDir.Left) ? new Vector2(0, 0) : new Vector2(0, 180);
    }

    // - Knockback coroutine
    IEnumerator Knockback(float knockbackDist, float knockbackTime)
    {
        MovementLock = true;
        bool[] lockArray = new bool[] { false, KnockbackLock };
        int knockbackDir = (enemyManager.Player.transform.position.x - transform.parent.position.x >= 0) ? -1 : 1;
        float knockbackVelocity = knockbackDir * knockbackDist / knockbackTime;
        ChangeDir_lock(knockbackDir * -1, new bool[] { MovementLock, KnockbackLock });
        ChangeVelocityX_lock(knockbackVelocity, lockArray);

        for (float timer = 0; timer <= knockbackTime; timer += Time.deltaTime)
        {
            if (CliffTest[(knockbackDir + 1) / 2])
            {
                ChangeVelocityX_lock(0.0f, lockArray);
                yield return new WaitForSeconds(knockbackTime - timer);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        MovementLock = false;
        ChangeVelocityX(0.0f);
    }

    // When damaged

    // - Calculate value & Arrange information
    public void GetDamaged(PlayerAttackInfo attack)
    {
        float prevHealth = currHealth;
        currHealth -= attack.damage;
        if (currHealth <= 0)
        {
            Invisible = true;
            animator.SetTrigger("DeadTrigger");
            return;
        }

        DebuffApply(attack.debuffTime);

        float knockbackDist = attack.damage * attack.knockBackMultiplier / weight;
        float knockbackTime = (knockbackDist >= 0.5f) ? 0.5f : knockbackDist;

        if (MovementLock) // 넉백이 진행 중
        {
            StopCoroutine("Knockback");
        }
        StartCoroutine(Knockback(knockbackDist, knockbackTime));

        float currHealthPercentage = currHealth / maxHealth;
        float prevHealthPercentage = prevHealth / maxHealth;

        foreach (float percentage in knockbackPercentage)
        {
            if (currHealthPercentage > percentage) { break; }
            if (prevHealthPercentage > percentage)
            {
                animator.SetTrigger("DamagedTrigger");
                break;
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
    private void DebuffApply(float[] debuffTime)
    {
        if(debuffState[(int)EnemyDebuffCase.Ice] == DebuffState.On){
            OffDebuff(EnemyDebuffCase.Ice);
        }

        foreach (int debuff in Enum.GetValues(typeof(EnemyDebuffCase)))
        {
            if(debuff == (int)EnemyDebuffCase.END_POINTER || debuffTime[debuff] == 0.0f || debuffState[debuff] == DebuffState.Immune)
            {
                continue;
            }
            
            float duration = debuffTime[debuff];
            switch ((EnemyDebuffCase)debuff)
            {
                case EnemyDebuffCase.Fire:
                    if (fireDuration != 0.0f) { fireDuration += duration; }
                    else {
                        debuffState[debuff] = DebuffState.On;
                        StartCoroutine(OnFire(duration));
                    }
                    break;
                case EnemyDebuffCase.Ice:
                    debuffState[debuff] = DebuffState.On;
                    StartCoroutine(OnIce(duration));
                    break;
                case EnemyDebuffCase.Stun:
                    if(debuffState[debuff] != DebuffState.On) {
                        debuffState[debuff] = DebuffState.On;
                        StartCoroutine(OnStun(duration));
                    } 
                    break;
                case EnemyDebuffCase.Blind:
                    StartCoroutine(OnBlind(duration));
                    break;
                case EnemyDebuffCase.Charm:
                    StartCoroutine(OnCharm(duration));
                    break;
                default:
                    break;
            }
        }
    }

    // - Debuff coroutine
    IEnumerator OnFire(float duration)
    {
        fireDuration = duration;
        float dotGap = 1.0f;
        while(true)
        {
            yield return new WaitForSeconds(dotGap);
            fireDuration -= dotGap;
            if (fireDuration < 0.0f) {
                fireDuration = 0.0f;
                break;
            }
            GetDamaged(lifeStoneManager.lifeStoneRowNum * 3);
        }
        debuffState[(int)EnemyDebuffCase.Fire] = DebuffState.Off;
    }

    IEnumerator OnIce(float duration)
    {
        ChangeVelocityX_lock(0.0f, new bool[] { });
        KnockbackLock = true;
        animator.SetTrigger("StunnedTrigger");
        animator.speed = damagedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Ice);
    }

    IEnumerator OnStun(float duration)
    {
        ChangeVelocityX_lock(0.0f, new bool[] { });
        animator.SetTrigger("StunnedTrigger");
        animator.speed = damagedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Stun);
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
        StartCoroutine(ImmuneTimer(Case, immunity_time[(int)Case]));
        switch (Case)
        {
            case EnemyDebuffCase.Ice:
                StopCoroutine("OnIce");
                KnockbackLock = false;
                animator.speed = 1.0f;
                animator.SetTrigger("DisableStunTrigger");
                break;
            case EnemyDebuffCase.Stun:
                StopCoroutine("OnStun");
                animator.speed = 1.0f;
                animator.SetTrigger("DisableStunTrigger");
                break;
            default:
                break;
        }
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

    public void aaa()
    {
        Debug.Log("aaa");
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
