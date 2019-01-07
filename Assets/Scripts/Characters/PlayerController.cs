using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    private float ropeSpeed;
    [SerializeField]
    private float doubleJumpSpeed;

    // Bool values for jump & doublejump
    private bool isGrounded = true;
    private bool isJumpable = true;     // Can player jump or doublejump?
    private bool isInRope = false;
    // Inputs
    private float horizontal = 0;
    private float horizontalRaw = 0;
    private float verticalRaw = 0;
    private bool jump = false;

    // Variables for IsGrounded()
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask ropeLayer;
    [SerializeField]
    private float ropeDistance = 0.3f;
    [SerializeField]
    private float rayDistance;

    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        horizontalRaw = Input.GetAxisRaw("Horizontal");
        verticalRaw = Input.GetAxisRaw("Vertical");
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

        if (IsInRope())
        {
            if (isInRope)
            {
                if (horizontalRaw != 0f && verticalRaw == 0f)
                {
                    isInRope = false;
                    rb.gravityScale = 2f;
                }
                rb.velocity = new Vector2(0f, verticalRaw * Time.smoothDeltaTime * ropeSpeed);

            }
            else if (jump)
            {
                isInRope = true;
                rb.gravityScale = 0f;
                transform.position = new Vector2(Mathf.Round(transform.position.x - 0.5f) + 0.5f, transform.position.y);
                rb.velocity = new Vector2(0f, 0f);
            }
        }
        else
        {
            isInRope = false;
            rb.gravityScale = 2f;
        }
        if (!isInRope)
        {
            float vertical = rb.velocity.y;
            if (jump)
            {
                if (isGrounded)
                {
                    vertical = jumpSpeed *  Time.smoothDeltaTime;
                }
                else if (isJumpable)
                {
                    vertical = doubleJumpSpeed *  Time.smoothDeltaTime;
                    isJumpable = false;
                }
            }

            //rb.velocity = new Vector2(horizontal * speed *  Time.smoothDeltaTime, vertical);
            // rb.velocity = new Vector2(rb.velocity.x, vertical);
            rb.AddForce(horizontalRaw * speed * Time.smoothDeltaTime * Vector2.right);
            if (((horizontalRaw == 0) || (rb.velocity.x > 0 && horizontalRaw < 0)
                || (rb.velocity.x < 0 && horizontalRaw > 0)) && (isGrounded))
            {

                rb.AddForce(rb.velocity.x * (-10f) * Vector2.right);
            }

            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed * Time.smoothDeltaTime, maxSpeed * Time.smoothDeltaTime), vertical);
        }
        jump = false;
    }
    bool IsGrounded()   // Is player grounded?
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);
        Debug.DrawRay(transform.position, rayDistance * Vector2.down, Color.white);
        return hit.collider != null;
    }
    bool IsInRope()   // Is player in rope?
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.right, ropeDistance, ropeLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left, ropeDistance, ropeLayer);
        Debug.DrawRay(transform.position, ropeDistance * Vector2.right, Color.red);
        Debug.DrawRay(transform.position, ropeDistance * Vector2.left, Color.red);
        return hit1.collider != null || hit2.collider != null;
    }
}
