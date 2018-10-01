using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb;     // RigidBody2D of this game object

    // Speeds of player
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float speedAir;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    
    private float doubleJumpSpeed;
   
    // Bool values for jump & doublejump
    private bool isGrounded = true;
    private bool isJumpable = true;     // Can player jump or doublejump?

    // Inputs
    private float horizontal = 0;
    private float horizontalRaw = 0;
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
        horizontalRaw = Input.GetAxisRaw("Horizontal");

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
                vertical = jumpSpeed * Time.fixedDeltaTime;
            }
            else if (isJumpable)
            {
                vertical = doubleJumpSpeed * Time.fixedDeltaTime;
                isJumpable = false;
            }
        }
        
            //rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, vertical);
           // rb.velocity = new Vector2(rb.velocity.x, vertical);
            rb.AddForce(horizontalRaw * speed * Time.fixedDeltaTime * Vector2.right);
            if (((horizontalRaw==0) || (rb.velocity.x > 0 && horizontalRaw < 0)
                || (rb.velocity.x < 0 && horizontalRaw > 0)) &&(isGrounded))
            {
           
                rb.AddForce(rb.velocity.x * (-10f) * Vector2.right);
            }
                
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed*Time.fixedDeltaTime, maxSpeed * Time.fixedDeltaTime), vertical);

        jump = false;
    }
    bool IsGrounded()   // Is player grounded?
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);
        Debug.DrawRay(transform.position, rayDistance * Vector2.down, Color.white);
        return hit.collider != null;
    }
}
