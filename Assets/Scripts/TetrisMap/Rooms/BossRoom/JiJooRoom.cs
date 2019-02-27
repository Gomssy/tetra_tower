using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiJooRoom : BossRoomInGame {
    

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        transitionUpdate[0] += Phase1Transition;
        //transitionUpdate[1] += Phase2Transition;
        phaseUpdate[0] += Phase1;
        //phaseUpdate[1] += Phase2;
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
    }

    protected void Phase1Transition()
    {
        
    }
    protected void Phase2Transition()
    {
        
    }
    protected void Phase1()
    {
        if (bosses[0].animator.GetBool("PhaseEnd") && bosses[1].animator.GetBool("PhaseEnd"))
        {
            CurPhase++;
        }
    }
    protected void Phase2()
    {
        if (bosses[0].animator.GetBool("PhaseEnd") && bosses[1].animator.GetBool("PhaseEnd"))
        {
            CurPhase++;
        }
    }
}
