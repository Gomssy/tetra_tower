using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float dx;   // Variation of horizontal speed
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float doubleJumpSpeed;

    private bool isGround = true;       // Is the player on the ground?
    private bool isDoubleJumping = false;     // Is the player double jumping?
    
    private Rigidbody2D rb;     // RigidBody2D of this game object

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        // TODO: Make moving algorithm
    }
}
