using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float dodgeDistance = 2f;

    [Header("Groundcheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public bool isGrounded;

    [Header("States checker")]
    [SerializeField] private bool jumpPressed = false;
    [SerializeField] private bool dodgePressed = false;
    [SerializeField] public bool isKnockedBack = false;
    [SerializeField] private float knockbackTimer = 0f;

    [SerializeField] private AudioClip sfx_jump;

    [Header("Input")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction dodgeAction;
    [SerializeField] private Vector2 moveValue;

    [Header("References")]
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private PlayerDodge playerDodge;
    protected Animator anim;

    public bool canMove = true;
    public bool canJump = true;

    private void Start()
    {
        if (groundCheck == null)
            groundCheck = transform.Find("GroundCheck");

        if (groundLayer.value == 0)
            groundLayer = LayerMask.GetMask("Ground");

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dodgeAction = InputSystem.actions.FindAction("Dodge");

        moveAction.Enable();
        jumpAction.Enable();
        dodgeAction.Enable();

        playerDodge = GetComponent<PlayerDodge>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,
                                             groundCheckRadius, groundLayer);

        moveValue = moveAction.ReadValue<Vector2>();

        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            jumpPressed = true;
        }

        if (dodgeAction.WasPressedThisFrame())
        {
            Debug.Log("Dodge input detected!");
            dodgePressed = true;
        }
    }

    private void FixedUpdate()
    {
        // Knockback stops everything
        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }

            return;
        }

        // Dodge has priority over normal movement
        if (playerDodge != null && playerDodge.isDodging) return;

        // If movement is disabled (e.g. during attack), stop horizontal movement & animations
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            anim.SetBool("walking", false);
            // also clear inputs so they don't trigger once movement comes back
            jumpPressed = false;
            dodgePressed = false;
            return;
        }

        float moveX = moveValue.x;

        // Horizontal movement
        if (moveX < 0)
        {
            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
            anim.SetBool("walking", true);
            spriteRenderer.flipX = true;
        }
        else if (moveX > 0)
        {
            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
            anim.SetBool("walking", true);
            spriteRenderer.flipX = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            anim.SetBool("walking", false);
        }

        // Jump
        if (jumpPressed)
        {
            if (canJump && isGrounded)
            {
                if (Mathf.Abs(rb.linearVelocity.y) > 0.01f)
                    anim.SetBool("walking", false);

                Vector2 impulse = new Vector2(0, jumpForce);
                rb.AddForce(impulse, ForceMode2D.Impulse);
                jumpPressed = false;

                anim.SetTrigger("jump");
                // play jump sound here if you want
            }
            else
            {
                // if we can't jump now, clear the flag so it doesn't "store" the jump
                jumpPressed = false;
            }
        }

        // Dodge
        if (dodgePressed)
        {
            if (canMove)    // extra guard
            {
                float moveDir = Mathf.Sign(moveX != 0 ? moveX : (spriteRenderer.flipX ? -1 : 1));
                transform.Translate(moveDir * dodgeDistance, 0, 0, Space.World);

                dodgePressed = false;

                if (playerDodge != null)
                {
                    playerDodge.StartDodge();
                }
            }
            else
            {
                dodgePressed = false;
            }
        }
    }

    public void EnableMovementAndJump()
    {
        canMove = true;
        canJump = true;
    }

    public virtual void DisableMovementAndJump()
    {
        canMove = false;
        canJump = false;
    }

    public void ApplyKnockback(float duration)
    {
        isKnockedBack = true;
        knockbackTimer = duration;
    }
}
