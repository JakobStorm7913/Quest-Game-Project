using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
[Header("Movement stats")]
[SerializeField] private float moveSpeed = 5f;
[SerializeField] private float jumpForce = 5f;
[SerializeField] private float dodgeDistance = 1f;

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

[SerializeField] private  AudioClip sfx_jump;

[Header("Input")]
[SerializeField] private InputAction moveAction;
[SerializeField] private InputAction jumpAction;
[SerializeField] private InputAction dodgeAction;
[SerializeField] private Vector2 moveValue;

[Header("References")]
//private Animator animator;
private SpriteRenderer spriteRenderer;
//private AudioSource audioSource;
private Rigidbody2D rb;
private PlayerDodge playerDodge;

protected Animator anim;


public bool canMove = true;
public bool canJump = true;

private void Start()

{

groundCheck = transform.Find("GroundCheck");
groundLayer = LayerMask.GetMask("Ground");

moveAction = InputSystem.actions.FindAction("Move");
jumpAction = InputSystem.actions.FindAction("Jump");
dodgeAction = InputSystem.actions.FindAction("Dodge");

moveAction.Enable();
jumpAction.Enable();
dodgeAction.Enable();

playerDodge = GetComponent<PlayerDodge>();

//animator = GetComponent<Animator>();
spriteRenderer = GetComponent<SpriteRenderer>();
//audioSource = GetComponent<AudioSource>();
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

    if (isKnockedBack)
    {
        knockbackTimer -= Time.fixedDeltaTime;
        if (knockbackTimer <= 0f)
        {
            isKnockedBack = false;
        }

        return;
    }

    if (playerDodge != null && playerDodge.isDodging) return;

float moveX = moveValue.x;

if (moveX < 0)
{

rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
//animator.SetBool("walking", true); 
anim.SetBool("walking", true);
spriteRenderer.flipX = true;
}

else if (moveX > 0)
{
rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
spriteRenderer.flipX = false;
// Animation:
//animator.SetBool("walking", true);
anim.SetBool("walking", true);
}

else 
{
rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
anim.SetBool("walking", false);
}

if (jumpPressed)
{
 
if (Mathf.Abs(rb.linearVelocity.y) > 0.01f)

anim.SetBool("walking", false);            


Vector2 impulse = new Vector2(0, jumpForce); // Vi pakker en Vector2 til AddForce
rb.AddForce(impulse, ForceMode2D.Impulse); // AddForce får sin Vector2 og får at vide at Impulse er det vi vil have..
jumpPressed = false; // Vi slår jumpPressed fra, så den kan blive true igen.
// Lyd og animation:
//audioSource.Stop();
//audioSource.PlayOneShot(sfx_jump);
//animator.SetBool("walking", false);
anim.SetTrigger("jump");


}

if (dodgePressed)
    {
        if (isKnockedBack) return;
        
        this.transform.Translate(moveX * dodgeDistance, 0, 0, Space.World);
        //Vector2 impulse = new Vector2(moveX * dodgeDistance, 0f);
        //rb.AddForce(impulse, ForceMode2D.Impulse);
        dodgePressed = false;

        if (playerDodge != null)
            {
                playerDodge.StartDodge();
            }
    }
}


public void EnableMovementAndJump(bool enable)
{
   canMove = enable;   
}


public virtual void EnableMovement(bool enable) // Movement

    {
        
        canJump = enable;

    }


public virtual void DisableMovementAndJump()
{
   canMove = false;
}  

public void ApplyKnockback(float duration)
{
    isKnockedBack = true;
    knockbackTimer = duration;
}

}


