using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {
    public BossRoomInGame bossRoom;
    public RuntimeAnimatorController[] animators;

    protected override IEnumerator OnIce(float duration) { yield return null; }
    protected override IEnumerator OnStun(float duration) { yield return null; }
    protected override IEnumerator Knockback(float knockbackDist, float knockbackTime) { yield return null; }

    protected override void Awake()
    {
        base.Awake();
        debuffState[(int)EnemyDebuffCase.Ice] = DebuffState.Immune;
        debuffState[(int)EnemyDebuffCase.Stun] = DebuffState.Immune;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
