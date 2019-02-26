using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour {

    // data

    // debuff
    readonly float[] immunity_time = new float[(int)EnemyDebuffCase.END_POINTER] { 0.0f, 3.0f, 6.0f, 6.0f, 6.0f };
    [SerializeField]
    protected DebuffState[] debuffState;
    float fireDuration = 0.0f;
    protected abstract IEnumerator OnIce(float duration);
    protected abstract IEnumerator OnStun(float duration);
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
    public float currHealth { get; protected set; }

    // manager
    protected InventoryManager inventoryManager;
    protected LifeStoneManager lifeStoneManager;
    protected EnemyManager enemyManager;

    // for movement
    protected Animator animator;
    protected float stunnedAnimLength;
    public bool MovementLock;
    public bool Invisible { get; protected set; }
    public bool KnockbackLock { get; protected set; }
    public float PlayerDistance { get; protected set; }
    protected abstract IEnumerator Knockback(float knockbackDist, float knockbackTime);

    // drop item
    private int[] dropTable;

    // for bumping attack
    public bool bumped = false;
    public bool bumpable = true;

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
        currHealth = maxHealth;
        Invisible = MovementLock = KnockbackLock = false;
        dropTable = enemyManager.DropTableByID[monsterID];
        PlayerDistance = Vector2.Distance(GameManager.Instance.player.transform.position, transform.parent.position);
    }

    protected virtual void FixedUpdate()
    {
        PlayerDistance = Vector2.Distance(GameManager.Instance.player.transform.position, transform.parent.position);
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
        animator.SetTrigger("TrackTrigger");
    }

    public void GetDamaged(float damage)
    {
        float prevHealth = currHealth;
        currHealth -= damage;
        if (currHealth <= 0)
        {
            Invisible = true;
            animator.SetTrigger("DeadTrigger");
            return;
        }

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
                    StartCoroutine(OnIce(duration));
                    break;
                case EnemyDebuffCase.Stun:
                    if(debuffState[debuff] != DebuffState.On) {
                        debuffState[debuff] = DebuffState.On;
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
        while(true)
        {
            yield return new WaitForSeconds(dotGap);
            fireDuration -= dotGap;
            if (fireDuration < 0.0f) {
                fireDuration = 0.0f;
                break;
            }
            GetDamaged(lifeStoneManager.lifeStoneRowNum * 0.3f);
        }
        debuffState[(int)EnemyDebuffCase.Fire] = DebuffState.Off;
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
