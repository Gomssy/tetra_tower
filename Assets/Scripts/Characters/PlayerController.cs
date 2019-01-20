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
    // Speeds of player
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float maxDashSpeed;
    [SerializeField]
    private float accerlation;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float ropeSpeed;
    [SerializeField]
    private float doubleJumpSpeed;
    [SerializeField]
    private float dashAccerlation;
    [SerializeField]
    private bool isDashing = false;
    public TilemapCollider2D[] platformArray;
    // Bool values for jump & doublejump
    private bool isGrounded = true;
    private bool isJumpable = true;     // Can player jump or doublejump?
    private bool isDownPlatform = false;
    private bool ropeEnabled = true;
    // Inputs
    [SerializeField]
    private float horizontal = 0;
    [SerializeField]
    private float horizontalRaw = 0;
    private float verticalRaw = 0;
    private bool jump = false;
    [SerializeField]
    private int dashStart = 0;
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
    enum PlayerState { Idle, Walk, Run, GoingUp, GoingDown, Rope}
    PlayerState playerState,previousState;   

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerState = PlayerState.Idle;
        previousState = PlayerState.Idle;
    }
    
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

        if (GameManager.gameState == GameState.Ingame)
        {

            if (isGrounded)
                isJumpable = true;
            if (isGrounded)
            {
                if (horizontalRaw == 1f) transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                else if (horizontalRaw == -1f) transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            }


            if (verticalRaw == -1 && !isDownPlatform)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, platformLayer);
                if (hit.collider != null && rb.velocity.y == 0)
                {
                    Room curRoom = MapManager.mapGrid[Player.tx, Player.ty];
                    platformArray = curRoom.GetComponentsInChildren<TilemapCollider2D>();
                    isDownPlatform = true;
                    StartCoroutine(DownPlatform());
                }
            }
            if (IsInRope())
            {
                if (playerState == PlayerState.Rope)
                {
                    if (horizontalRaw != 0f && verticalRaw == 0f)
                    {
                        playerState = PlayerState.Idle;
                        rb.gravityScale = rbGravityScale;
                        StartCoroutine(RopeDelay());

                    }
                    rb.velocity = new Vector2(0f, verticalRaw * ropeSpeed);

                }
                else if (verticalRaw != 0 && ropeEnabled && horizontalRaw == 0)
                {
                    playerState = PlayerState.Rope;
                    rb.gravityScale = 0f;
                    transform.position = new Vector2(Mathf.Round(transform.position.x - 0.5f) + 0.5f, transform.position.y);
                    rb.velocity = new Vector2(0f, 0f);
                    
                }
                anim.SetFloat("ropeUpDown", verticalRaw);
            }
            else
            {
                playerState = PlayerState.Idle;
                rb.gravityScale = rbGravityScale;
            }
            if (playerState != PlayerState.Rope)
            {
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
                if (!isGrounded)
                {
                    if (vertical > 0) playerState = PlayerState.GoingUp;
                    else playerState = PlayerState.GoingDown;
                }
                

                if (horizontalRaw != 0)
                {
                    if (horizontal != 1 && horizontal != -1 && dashStart == 0)
                    {
                        //짧게 눌렀을 때
                        dashStart = 1;
                    }
                }
                if (horizontalRaw == 0 && horizontal != 0 && dashStart == 1)
                {
                    //방금 뗐을때
                    dashStart = 2;
                    //이제 빠르게 켜면 됨
                }
                if (dashStart == 2 && horizontalRaw != 0)
                {
                    isDashing = true;
                }
                if (horizontalRaw == 0 && horizontal == 0)
                {
                    dashStart = 0;
                    isDashing = false;
                }


                if (isDashing)
                    rb.AddForce(horizontalRaw * dashAccerlation * Time.smoothDeltaTime * Vector2.right);
                else
                    rb.AddForce(horizontalRaw * accerlation * Time.smoothDeltaTime * Vector2.right);

                if (isGrounded)
                {
                    if (horizontalRaw == 0) playerState = PlayerState.Idle;
                    else
                    {
                        if (isDashing) playerState = PlayerState.Run;
                        else playerState = PlayerState.Walk;
                    }
                }
                if (((horizontalRaw == 0) || (rb.velocity.x > 0 && horizontalRaw < 0)
                    || (rb.velocity.x < 0 && horizontalRaw > 0)) && (isGrounded))
                {
                   // rb.AddForce(rb.velocity.x * (-100f) * Vector2.right * Time.smoothDeltaTime);
                    rb.velocity = new Vector2(rb.velocity.x / (1.5f), rb.velocity.y);
                }
                if (isDashing) rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxDashSpeed, maxDashSpeed), vertical);
                else
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), vertical);
            }
            if(previousState != playerState)
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
        jump = false;
    }
    bool IsGrounded()   // Is player grounded?
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, platformLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, outerwallLayer);
        Debug.DrawRay(transform.position, rayDistance * Vector2.down, Color.white);
        return (hit1.collider != null || hit2.collider != null || hit3.collider != null) && rb.velocity.y == 0 ;//플랫폼 점프 버그 방지
    }
    bool IsInRope()   // Is player in rope?
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + ropeUp*Vector3.up, Vector2.right, ropeDistance, ropeLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + ropeUp*Vector3.up, Vector2.left, ropeDistance, ropeLayer);
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
        foreach (TilemapCollider2D element in platformArray)
        {
            if (element.name == "platform")
            {
                element.enabled = false;
                yield return new WaitForSeconds(0.3f);
                while(playerState == PlayerState.Rope) yield return new WaitForSeconds(0.1f);
                element.enabled = true;
                isDownPlatform = false;
            }
        }
    }
    public IEnumerator RopeDelay()
    {
        ropeEnabled = false;
        isJumpable = true;
        yield return new WaitForSeconds(0.5f);
        ropeEnabled = true;

    }
    
}
