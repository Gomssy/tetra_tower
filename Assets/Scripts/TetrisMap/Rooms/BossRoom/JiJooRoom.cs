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
        phaseAction[0] += Phase1;
        phaseAction[1] += Phase2;
        transitionAction[0] += Phase1Transition;
        transitionAction[1] += Phase2Transition;
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

    }
    protected void Phase2()
    {

    }
}
