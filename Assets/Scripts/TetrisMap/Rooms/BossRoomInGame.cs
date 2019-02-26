using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomInGame : RoomInGame {
    public Boss[] bosses;
    public int totalPhase;
    private int curPhase = -1;
    public int CurPhase
    {
        get { return curPhase; }
        set
        {
            // 모든 페이즈가 끝났을때
            if (value > totalPhase)
            {
                curPhase = -1;
                RoomClear();
            }
            // 페이즈 이동
            else if (value != curPhase)
            {
                if (phaseCoroutine != null)
                    StopCoroutine(phaseCoroutine);
                curPhase = value;
                phaseCoroutine = Phase(curPhase);
                StartCoroutine(phaseCoroutine);
            }
        }
    }
    public bool isTransitionFinished;
    // phaseUpdate 전 매 프레임 호출됨; Update 대용
    public Action[] transitionUpdate;
    // 진행중인 phase coroutine에서 매 프레임 호출됨; Update 대용
    public Action[] phaseUpdate;
    // 현재 진행중인 phase coroutine
    private IEnumerator phaseCoroutine;

    protected bool attackStart = false;
    protected bool attackTrigger = false;

    protected virtual void Awake()
    {
        transitionUpdate = new Action[totalPhase];
        phaseUpdate = new Action[totalPhase];
    }

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Debug.Log(CurPhase);
        if (!attackStart && attackTrigger)
        {
            StartCoroutine(BeforeBossFight());
            attackStart = true;
        }
    }

    protected virtual IEnumerator BeforeBossFight()
    {
        CurPhase++;
        yield return null;
    }
    protected virtual IEnumerator AfterBossFight()
    {
        yield return null;
    }
    IEnumerator Phase(int phase)
    {
        isTransitionFinished = false;
        Debug.Log(transitionUpdate[phase].GetInvocationList().GetLength(0));
        while (!isTransitionFinished)
        {
            if (transitionUpdate[phase] != null)
                transitionUpdate[phase]();
            yield return null;
        }
        while (CurPhase == phase)
        {
            if (phaseUpdate[phase] != null)
                phaseUpdate[phase]();
            yield return null;
        }
    }

    public override void RoomEnter()
    {
        base.RoomEnter();
        //EnemyManager.Instance.SpawnEnemyToMap();
        attackTrigger = true;
        //보스 만들어지면 구현할 것
    }

    public override void RoomClear()
    {
        base.RoomClear();
        MapManager.currentStage += 1;
        StartCoroutine(MapManager.Instance.MakeNextTetrimino());
    }
}
