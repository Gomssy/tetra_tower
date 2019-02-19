using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;     // RigidBody2D of this game object
    private Animator anim;
    private AudioSource audio;
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
    public bool airAttack = true;
    // Inputs
    private float horizontal = 0;
    private float horizontalRaw = 0;
    private float verticalRaw = 0;
    private bool upKeyDown = false;
    private bool downKeyDown = false;
    private bool jump = false;
    private bool dash = false;
    // Variables for collsiion checking
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask ropeLayer;
    [SerializeField]
    private float boxHeight;
    [SerializeField]
    private float ropeUp, ropeDown;
    [SerializeField]
    private LayerMask platformLayer;
    [SerializeField]
    private LayerMask itemLayer;
    [SerializeField]
    private LayerMask portalLayer;
    [SerializeField]
    private float rayDistance;
    [SerializeField]
    private IPlayerInteraction lastDropItem;
    private bool interaction;
    public PlayerState playerState, previousState;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        float previous = verticalRaw;
        horizontal = Input.GetAxis("Horizontal");
        horizontalRaw = Input.GetAxisRaw("Horizontal");
        verticalRaw = Input.GetAxisRaw("Vertical");
        if (isGrounded) dash = Input.GetButton("Dash");
        
        if (!upKeyDown) upKeyDown = previous <= 0 && verticalRaw > 0;
        if (!downKeyDown) downKeyDown = previous >= 0 && verticalRaw < 0;

        interaction = Input.GetButtonDown("Interaction");
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (!GetItemRay() && lastDropItem != null)
        {
            lastDropItem.HighlightSwitch(false);
            lastDropItem = null;
        }

        if(lastDropItem != null && interaction)
        {
            lastDropItem.Apply();
            lastDropItem = null;
        }
    }

    private void FixedUpdate()
    {
        bool tmp = IsGrounded();
        if (!isGrounded && tmp)
        {
            audio.enabled = false;
            audio.enabled = true;
        }
        isGrounded = tmp;

        if (GameManager.gameState == GameState.Tutorial || GameManager.gameState == GameState.Ingame)
        {
            if (playerState == PlayerState.Attack)
            {
                rb.gravityScale = rbGravityScale;
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
                if (OnPlatform() && downKeyDown)
                {
                    Room curRoom = MapManager.mapGrid[Player.tx, Player.ty];
                    platformCollider = curRoom.GetComponentInChildren<RoomInGame>().transform.Find("platform").GetComponent<CompositeCollider2D>();
                    StartCoroutine(DownPlatform());
                }
                if (IsInRope())
                {
                    if (playerState == PlayerState.Rope)
                    {
                        // Jump or Horizontal move in rope
                        if (jump || horizontal != 0)
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
                            airAttack = true;
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
                if (playerState != PlayerState.GoingUp && playerState != PlayerState.GoingDown && playerState != PlayerState.Attack)
                    airAttack = true;
            }
        }
        else
        {
            float xVelocity = rb.velocity.x;
            float yVelocity = rb.velocity.y;
            float airResistance = isGrounded ? 1f : 0.5f;
            float direction = Mathf.Sign(xVelocity);
            xVelocity = Mathf.Abs(xVelocity) - deceleration * airResistance * Time.fixedDeltaTime;
            if (xVelocity < 0) xVelocity = 0;
            xVelocity *= direction;
            if (isGrounded) playerState = PlayerState.Idle;
            rb.velocity = new Vector2(xVelocity, yVelocity);
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
        upKeyDown = false;
        downKeyDown = false;
        jump = false;
    }
    bool IsGrounded()   // Is player grounded?
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(Player.X, boxHeight), 0, Vector2.down, Player.Y / 2f, groundLayer);
        Debug.DrawLine(transform.position + new Vector3(0, -Player.Y / 2f) + new Vector3(-Player.X, -boxHeight, 0) / 2,
                       transform.position + new Vector3(0, -Player.Y / 2f) + new Vector3(Player.X, -boxHeight, 0) / 2);
        return hit.collider != null && rb.velocity.y == 0; // 플랫폼 점프 버그 방지
    }

    bool GetItemRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position , Vector2.down, rayDistance, itemLayer);

        if (hit.collider != null)
        {
            IPlayerInteraction temp = hit.collider.GetComponent<IPlayerInteraction>();
            if (lastDropItem != null) lastDropItem.HighlightSwitch(false);
            if (temp != null) temp.HighlightSwitch(true);
            lastDropItem = temp;
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, portalLayer);

            if (hit.collider != null)
            {
                IPlayerInteraction temp = hit.collider.GetComponent<IPlayerInteraction>();
                if (lastDropItem != null) lastDropItem.HighlightSwitch(false);
                if (temp != null) temp.HighlightSwitch(true);
                lastDropItem = temp;
            }
        }

        return hit.collider != null;
    }
    bool IsInRope()   // Is player in rope?
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + new Vector3(0, (ropeUp - ropeDown) / 2f), new Vector2(Player.X, ropeUp + ropeDown), 0, Vector2.zero, 0, ropeLayer);
        Debug.DrawLine(transform.position + new Vector3(-Player.X / 2f, ropeUp, 0),
                       transform.position + new Vector3(Player.X / 2f, ropeUp, 0));
        Debug.DrawLine(transform.position + new Vector3(-Player.X / 2f, -ropeDown, 0),
                       transform.position + new Vector3(Player.X / 2f, -ropeDown, 0));
        return hit.collider != null;
    }
    bool OnPlatform()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(Player.X, boxHeight), 0, Vector2.down, Player.Y / 2f, platformLayer);
        return hit.collider != null && rb.velocity.y == 0; // 플랫폼 점프 버그 방지
    }
    public IEnumerator DownPlatform()
    {
        Physics2D.IgnoreCollision(platformCollider, transform.GetComponent<Collider2D>(), true);
        yield return new WaitForSeconds(0.3f);
        while (playerState == PlayerState.Rope) yield return null;
        Physics2D.IgnoreCollision(platformCollider, transform.GetComponent<Collider2D>(), false);
    }
    public IEnumerator RopeDelay()
    {
        ropeEnabled = false;
        isJumpable = true;
        yield return new WaitForSeconds(0.3f);
        ropeEnabled = true;
    }
 }
