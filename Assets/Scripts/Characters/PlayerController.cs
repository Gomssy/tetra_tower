using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;     // RigidBody2D of this game object
    private Animator anim;
    [SerializeField]
    private float rbGravityScale;
    [SerializeField]
    private float rbAttackGravityScale;
    // Speeds of player
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float maxDashSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float deceleration;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float ropeSpeed;
    [SerializeField]
    private float doubleJumpSpeed;
    [SerializeField]
    private float dashAcceleration;
    public Collider2D platformCollider;
    // Bool values for jump & doublejump
    private bool isGrounded = true;
    private bool isJumpable = true;     // Can player jump or doublejump?
    private bool isDownPlatform = false;
    private bool ropeEnabled = true;
    // Inputs
    private float horizontal = 0;
    private float horizontalRaw = 0;
    private float verticalRaw = 0;
    private bool upKeyDown = false;
    private bool jump = false;
    private bool dash = false;
    // Variables for IsGrounded()
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask ropeLayer;
    [SerializeField]
    private LayerMask platformLayer;
    [SerializeField]
    private LayerMask outerwallLayer;
    [SerializeField]
    private float ropeDistance = 0.3f;
    [SerializeField]
    private float rayDistance;
    [SerializeField]
    private float ropeUp, ropeDown;

    
    public PlayerState playerState, previousState;
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        float previous = verticalRaw;
        horizontal = Input.GetAxis("Horizontal");
        horizontalRaw = Input.GetAxisRaw("Horizontal");
        verticalRaw = Input.GetAxisRaw("Vertical");
        dash = Input.GetButton("Dash");
        
        if (!upKeyDown) upKeyDown = previous <= 0 && verticalRaw > 0;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();

        if (GameManager.gameState == GameState.Ingame)
        {
            if (playerState == PlayerState.Attack)
            {
                rb.gravityScale = rbAttackGravityScale;
                return;
            }
                
            if (isGrounded)
                isJumpable = true;
            if (playerState != PlayerState.Attack)
            {
                if (isGrounded)
                {
                    if (horizontalRaw > 0) transform.localScale = new Vector3(-1, 1, 1);
                    else if (horizontalRaw < 0) transform.localScale = new Vector3(1, 1, 1);
                }

                // Platform downjump
                if (verticalRaw < 0 && !isDownPlatform)
                {
                    Room curRoom = MapManager.mapGrid[Player.tx, Player.ty];
                    platformCollider = curRoom.GetComponentInChildren<RoomInGame>().transform.Find("platform").GetComponent<CompositeCollider2D>();
                    isDownPlatform = true;
                    StartCoroutine(DownPlatform());
                }
                if (IsInRope())
                {
                    if (playerState == PlayerState.Rope)
                    {
                        // Horizontal move in rope
                        if (horizontal != 0 && verticalRaw == 0)
                        {
                            playerState = PlayerState.Idle;
                            rb.gravityScale = rbGravityScale;
                            StartCoroutine(RopeDelay());
                        }
                        // Vertical move in rope
                        else
                        {
                            rb.velocity = new Vector2(0, verticalRaw * ropeSpeed);
                        }
                    }
                    else if (verticalRaw != 0 && ropeEnabled && horizontal == 0)
                    {
                        playerState = PlayerState.Rope;
                        rb.gravityScale = 0;
                        transform.position = new Vector2(Mathf.Round(transform.position.x - 0.5f) + 0.5f, transform.position.y);
                        rb.velocity = Vector2.zero;
                    }
                    anim.SetFloat("ropeUpDown", verticalRaw);
                }
                else
                {
                    rb.gravityScale = rbGravityScale;
                    playerState = PlayerState.Idle;
                }

                if (playerState != PlayerState.Rope)
                {
                    float xVelocity = rb.velocity.x;
                    float yVelocity = rb.velocity.y;
                    float airResistance = isGrounded ? 1f : 0.5f;

                    // Jump
                    if (jump || upKeyDown)
                    {
                        if (isGrounded)
                        {
                            yVelocity = jumpSpeed;
                        }
                        else if (isJumpable)
                        {
                            yVelocity = doubleJumpSpeed;
                            isJumpable = false;
                        }
                    }
                    if (!isGrounded)
                    {
                        playerState = (yVelocity > 0) ? PlayerState.GoingUp : PlayerState.GoingDown;
                    }

                    // Walk & Dash
                    if (horizontalRaw == 0)
                    {
                        float direction = Mathf.Sign(xVelocity);
                        xVelocity = Mathf.Abs(xVelocity) - deceleration * airResistance * Time.fixedDeltaTime;
                        if (xVelocity < 0) xVelocity = 0;
                        xVelocity *= direction;
                        if (isGrounded) playerState = PlayerState.Idle;
                    }
                    else if (dash)
                    {
                        xVelocity += Mathf.Sign(horizontal) * dashAcceleration * airResistance * Time.fixedDeltaTime;
                        xVelocity = Mathf.Clamp(xVelocity, -maxDashSpeed, maxDashSpeed);
                        if (isGrounded) playerState = PlayerState.Run;
                    }
                    else
                    {
                        float analogSpeed = Mathf.Abs(horizontal);
                        xVelocity += Mathf.Sign(horizontal) * acceleration * airResistance * Time.fixedDeltaTime;
                        xVelocity = Mathf.Clamp(xVelocity, -maxSpeed * analogSpeed, maxSpeed * analogSpeed);
                        if (isGrounded) playerState = PlayerState.Walk;
                    }

                    rb.velocity = new Vector2(xVelocity, yVelocity);
                }
            }
            if (previousState != playerState)
                switch (playerState)
                {
                    case PlayerState.Idle: anim.SetTrigger("idle"); break;
                    case PlayerState.Walk: anim.SetTrigger("walk"); break;
                    case PlayerState.Run: anim.SetTrigger("run"); break;
                    case PlayerState.GoingUp: anim.SetTrigger("upTrigger"); break;
                    case PlayerState.GoingDown: anim.SetTrigger("downTrigger"); break;
                    case PlayerState.Rope: anim.SetTrigger("rope"); break;
                }

            previousState = playerState;

        }
        upKeyDown = false;
        jump = false;
    }
    bool IsGrounded()   // Is player grounded?
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(Player.X/2f, 0, 0), Vector2.down, rayDistance, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(Player.X/2f, 0, 0), Vector2.down, rayDistance, platformLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position + new Vector3(Player.X/2f, 0, 0), Vector2.down, rayDistance, outerwallLayer);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position - new Vector3(Player.X/2f, 0,0), Vector2.down, rayDistance, groundLayer);
        RaycastHit2D hit5 = Physics2D.Raycast(transform.position - new Vector3(Player.X/2f, 0, 0), Vector2.down, rayDistance, platformLayer);
        RaycastHit2D hit6 = Physics2D.Raycast(transform.position - new Vector3(Player.X/2f, 0, 0), Vector2.down, rayDistance, outerwallLayer);
        Debug.DrawRay(transform.position + new Vector3(Player.X/2f, 0, 0), rayDistance * Vector2.down, Color.white);
        return (hit1.collider != null || hit2.collider != null || hit3.collider != null|| 
            hit4.collider != null || hit5.collider != null || hit6.collider != null) && rb.velocity.y == 0;//플랫폼 점프 버그 방지
    }
    bool IsInRope()   // Is player in rope?
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + ropeUp * Vector3.up, Vector2.right, ropeDistance, ropeLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + ropeUp * Vector3.up, Vector2.left, ropeDistance, ropeLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position - ropeDown * Vector3.up, Vector2.right, ropeDistance, ropeLayer);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position - ropeDown * Vector3.up, Vector2.left, ropeDistance, ropeLayer);
        Debug.DrawRay(transform.position + ropeUp * Vector3.up, ropeDistance * Vector2.right, Color.red);
        Debug.DrawRay(transform.position + ropeUp * Vector3.up, ropeDistance * Vector2.left, Color.red);
        Debug.DrawRay(transform.position - ropeDown * Vector3.up, ropeDistance * Vector2.right, Color.red);
        Debug.DrawRay(transform.position - ropeDown * Vector3.up, ropeDistance * Vector2.left, Color.red);
        return hit1.collider != null || hit2.collider != null || hit3.collider != null || hit4.collider != null;
    }
    public IEnumerator DownPlatform()
    {
        Physics2D.IgnoreCollision(platformCollider, transform.GetComponent<Collider2D>(), true);
        yield return new WaitForSeconds(0.3f);
        while (playerState == PlayerState.Rope) yield return null;
        Physics2D.IgnoreCollision(platformCollider, transform.GetComponent<Collider2D>(), false);
        isDownPlatform = false;
    }
    public IEnumerator RopeDelay()
    {
        ropeEnabled = false;
        isJumpable = true;
        yield return new WaitForSeconds(0.5f);
        ropeEnabled = true;

    }

}
