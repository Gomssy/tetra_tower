using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

    // data

    // debuff
    readonly float[] immunity_time = new float[(int)EnemyDebuffCase.END_POINTER] { 0.0f, 3.0f, 6.0f, 6.0f, 6.0f };
    [SerializeField]
    protected DebuffState[] debuffState;
    float fireDuration = 0.0f;
    protected virtual IEnumerator OnIce(float duration) { yield return 0; }
    protected virtual IEnumerator OnStun(float duration) { yield return 0; }
    // protected abstract IEnumerator OnBlind(float duration);
    // protected abstract IEnumerator OnCharm(float duration);

    // stat
    public int monsterID;
    public float maxHealth;
    public float weight;
    public float patrolRange;
    public float noticeRange;
    
    public float patrolSpeed;
    public float trackSpeed;
    public float[] knockbackPercentage;
    public float CurrHealth { get; protected set; }

    // manager
    protected InventoryManager inventoryManager;
    protected LifeStoneManager lifeStoneManager;
    protected EnemyManager enemyManager;

    // for movement
    protected Animator animator;
    protected float stunnedAnimLength;
    public EnemyMovementLock movementLock;
    public bool Invisible { get; protected set; }
    public float PlayerDistance { get; protected set; }
    protected virtual IEnumerator Knockback(float knockbackDist, float knockbackTime) { yield return 0; }

    // drop item
    private int[] dropTable;

    // method
    // Standard method
    protected virtual void Awake()
    {
        enemyManager = EnemyManager.Instance;
        inventoryManager = InventoryManager.Instance;
        lifeStoneManager = LifeStoneManager.Instance;
        animator = GetComponent<Animator>();

        DebuffState _temp = DebuffState.Off;
        debuffState = new DebuffState[(int)EnemyDebuffCase.END_POINTER] { _temp, _temp, _temp, _temp, _temp };

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips) { if (clip.name.Contains("Stunned")) { stunnedAnimLength = clip.length; } }

        Array.Sort(knockbackPercentage);
        Array.Reverse(knockbackPercentage);
    }

    protected virtual void Start()
    { 
        CurrHealth = maxHealth;
        Invisible = false;
        movementLock = EnemyMovementLock.Free;
        if (enemyManager.DropTableByID.ContainsKey(monsterID)) { dropTable = enemyManager.DropTableByID[monsterID]; }
        PlayerDistance = Vector2.Distance(GameManager.Instance.player.transform.position, transform.parent.position);
    }

    protected virtual void FixedUpdate()
    {
        PlayerDistance = Vector2.Distance(GameManager.Instance.player.transform.position, transform.parent.position);
    }

    // When damaged

    // - Calculate value & Arrange information
    public virtual void GetHit(PlayerAttackInfo attack)
    {
        TakeDamage(attack.damage);

        float knockbackDist = attack.damage * attack.knockBackMultiplier / weight;
        float knockbackTime = (knockbackDist >= 0.5f) ? 0.5f : knockbackDist;

        if (movementLock == EnemyMovementLock.Rigid) // 넉백이 진행 중
        {
            StopCoroutine("Knockback");
        }

        if (movementLock < EnemyMovementLock.Debuffed)
        {
            movementLock = EnemyMovementLock.Rigid;
            StartCoroutine(Knockback(knockbackDist, knockbackTime));
        }

        DebuffApply(attack.debuffTime);

        animator.SetTrigger("TrackTrigger");
    }

    public void TakeDamage(float damage)
    {
        if (Invisible) { return; }
        float prevHealth = CurrHealth;
        CurrHealth -= damage;
        if (CurrHealth <= 0)
        {
            MakeDead();
            return;
        }

        float currHealthPercentage = CurrHealth / maxHealth;
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

    public void MakeDead()
    {
        Invisible = true;
        animator.SetTrigger("DeadTrigger");
        StopCoroutine("OnFire");
        GetComponent<SpriteRenderer>().color = Color.white;
        return;
    }

    // - Apply debuff
    protected void DebuffApply(float[] debuffTime)
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
                    movementLock = EnemyMovementLock.Debuffed;
                    StartCoroutine(OnIce(duration));
                    break;
                case EnemyDebuffCase.Stun:
                    if(debuffState[debuff] != DebuffState.On) {
                        debuffState[debuff] = DebuffState.On;
                        movementLock = EnemyMovementLock.Debuffed;
                        StartCoroutine(OnStun(duration));
                    } 
                    break;
                case EnemyDebuffCase.Blind:
                    //StartCoroutine(OnBlind(duration));
                    break;
                case EnemyDebuffCase.Charm:
                    //StartCoroutine(OnCharm(duration));
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
        float damageMultiplier = 1f;

        while (true)
        {
            yield return new WaitForSeconds(dotGap);
            for (float timer = 0; timer < dotGap; timer += Time.deltaTime)
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f + 0.5f * timer / dotGap, 0.5f + 0.5f * timer / dotGap);
                yield return null;
            }
            fireDuration -= dotGap;
            if (fireDuration < 0.0f)
            {
                fireDuration = 0.0f;
                break;
            }

            damageMultiplier = 1f;
            foreach (Item item in inventoryManager.itemList)
                damageMultiplier *= item.GlobalFireDamageMultiplier();

            TakeDamage(lifeStoneManager.lifeStoneRowNum * 0.15f * damageMultiplier);
            EffectManager.Instance.StartNumber(0, gameObject.transform.parent.position, lifeStoneManager.lifeStoneRowNum * 0.15f);
        }
        debuffState[(int)EnemyDebuffCase.Fire] = DebuffState.Off;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator ImmuneTimer(EnemyDebuffCase Case, float duration)
    {
        debuffState[(int)Case] = DebuffState.Immune;
        yield return new WaitForSeconds(duration);
        debuffState[(int)Case] = DebuffState.Off;
    }

    protected void OffDebuff(EnemyDebuffCase Case)
    {
        StartCoroutine(ImmuneTimer(Case, immunity_time[(int)Case]));
        switch (Case)
        {
            case EnemyDebuffCase.Ice:
                GetComponent<SpriteRenderer>().color = Color.white;
                StopCoroutine("OnIce");
                movementLock = EnemyMovementLock.Free;
                animator.speed = 1.0f;
                animator.SetTrigger("DisableStunTrigger");
                break;
            case EnemyDebuffCase.Stun:
                StopCoroutine("OnStun");
                movementLock = EnemyMovementLock.Free;
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
        transform.parent.SetParent(null);
        StopAllCoroutines();
        enemyManager.EnemyDeadCount++;

        CurrHealth = maxHealth;
        Invisible = false;
        // Drop 아이템 결정. 인덱스 별 아이템은 맨 밑에 서술
        if (dropTable == null) { return; }

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
