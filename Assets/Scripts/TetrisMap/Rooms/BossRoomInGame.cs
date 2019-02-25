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
    // phaseAction 전(isTransitionFinished == false 일때) 매 프레임 호출됨; Update 대응
    public Action[] transitionAction;
    // 진행중인 phase coroutine에서 매 프레임 호출됨; Update 대용
    public Action[] phaseAction;
    // 현재 진행중인 phase coroutine
    private IEnumerator phaseCoroutine;

    protected bool attackStart;

    protected virtual void Awake()
    {
        transitionAction = new Action[totalPhase];
        phaseAction = new Action[totalPhase];
    }

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (attackStart)
        {
            StartCoroutine(BeforeBossFight());
            attackStart = false;
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
        while (!isTransitionFinished)
        {
            transitionAction[phase]();
            yield return null;
        }
        while (CurPhase == phase)
        {
            phaseAction[phase]();
            yield return null;
        }
    }

    public override void RoomEnter()
    {
        base.RoomEnter();

        //보스 만들어지면 구현할 것
    }

    public override void RoomClear()
    {
        base.RoomClear();
        MapManager.currentStage += 1;
        StartCoroutine(MapManager.Instance.MakeNextTetrimino());
    }
}
