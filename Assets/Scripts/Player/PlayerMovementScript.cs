using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float dodgeDistance = 2f;

    [Header("Groundcheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Input")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction dodgeAction;

    [Header("References")]
    [SerializeField] private Transform attackPoint;   // assign your AttackPoint child here
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    public bool canMove = true;
    public bool canJump = true;
    public bool isGrounded { get; private set; }
    public bool isKnockedBack { get; private set; }
    private float knockbackTimer;

    private Vector2 moveValue;
    private bool facingRight = true;
    private bool jumpPressed;
    private bool dodgePressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (groundCheck == null) groundCheck = transform.Find("GroundCheck");
        if (groundLayer.value == 0) groundLayer = LayerMask.GetMask("Ground");

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dodgeAction = InputSystem.actions.FindAction("Dodge");

        moveAction?.Enable();
        jumpAction?.Enable();
        dodgeAction?.Enable();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        moveValue = moveAction.ReadValue<Vector2>();

        if (jumpAction.WasPressedThisFrame() && isGrounded) jumpPressed = true;
        if (dodgeAction.WasPressedThisFrame()) dodgePressed = true;
    }

    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f) isKnockedBack = false;
            return;
        }

        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            anim.SetBool("walking", false);
            jumpPressed = false;
            dodgePressed = false;
            return;
        }

        float moveX = moveValue.x;

        // Flip by scaling so children (attackPoint) follow automatically
        HandleFlipByScale(moveX);

        // Horizontal movement
        if (Mathf.Abs(moveX) > 0.001f)
        {
            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
            anim.SetBool("walking", true);
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
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                anim.SetTrigger("jump");
            }
            jumpPressed = false;
        }

        // Dodge
        if (dodgePressed)
        {
            float dir = Mathf.Abs(moveX) > 0.001f ? Mathf.Sign(moveX) : (facingRight ? 1f : -1f);
            transform.Translate(Vector3.right * dir * dodgeDistance, Space.World);
            dodgePressed = false;
            // trigger your dodge animation/effects here if needed
        }
    }

    private void HandleFlipByScale(float moveX)
    {
        if (moveX > 0f && !facingRight)
            SetFacing(true);
        else if (moveX < 0f && facingRight)
            SetFacing(false);
    }

    private void SetFacing(bool faceRight)
    {
        facingRight = faceRight;

        // Flip the entire transform so children (attackPoint) follow
        Vector3 s = transform.localScale;
        s.x = faceRight ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);
        transform.localScale = s;

    }

    public void EnableMovementAndJump()
    {
        canMove = true;
        canJump = true;
    }

    public void DisableMovementAndJump()
    {
        canMove = false;
        canJump = false;
    }

    public void ApplyKnockback(float duration)
    {
        isKnockedBack = true;
        knockbackTimer = duration;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, 0.5f);
        }
    }
}