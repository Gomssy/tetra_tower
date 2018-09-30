using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb;     // RigidBody2D of this game object

    // Speeds of player
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float doubleJumpSpeed;

    // Bool values for jump & doublejump
    private bool isGrounded = true;
    private bool isJumpable = true;     // Can player jump or doublejump?

    // Inputs
    private float horizontal = 0;
    private bool jump = false;

    // Variables for IsGrounded()
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float rayDistance;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        if (isGrounded)
            isJumpable = true;

        float vertical = rb.velocity.y;
        if (jump)
        {
            if (isGrounded)
            {
                vertical = jumpSpeed;
            }
            else if (isJumpable)
            {
                vertical = doubleJumpSpeed;
                isJumpable = false;
            }
        }
        rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, vertical);
        jump = false;
    }
    
    bool IsGrounded()   // Is player grounded?
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);
        Debug.DrawRay(transform.position, rayDistance* Vector2.down, Color.white);
        return hit.collider != null;
    }
}
