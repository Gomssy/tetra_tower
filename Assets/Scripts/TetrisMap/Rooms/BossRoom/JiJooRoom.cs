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
        Debug.Log("come");
    }
    protected void Phase2Transition()
    {
        
    }
    protected void Phase1()
    {
        if (bosses[0].currHealth <= 0 && bosses[1].currHealth <= 0)
        {
            CurPhase++;
        }
    }
    protected void Phase2()
    {
        if (bosses[0].currHealth <= 0 && bosses[1].currHealth <= 0)
        {
            CurPhase++;
        }
    }
}
