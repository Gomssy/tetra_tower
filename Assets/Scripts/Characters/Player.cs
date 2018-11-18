using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public LifeCrystalUI LCUI;
    public static int tx, ty;
    public int ttx;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        tx = (int)(transform.position.x / 24f);
        ttx = tx;
	}
}
