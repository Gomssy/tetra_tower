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
        transitionAction[0] += Phase0Transition;
        transitionAction[1] += Phase1Transition;
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
    }

    protected void Phase0Transition()
    {
        
    }
    protected void Phase1Transition()
    {
        
    }
    protected void Phase1()
    {

    }
    protected void Phase2()
    {

    }
}
