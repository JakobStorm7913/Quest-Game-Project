using UnityEngine;

public class Player : Entity
{
    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 8f; // Kode til moveSpeed
    [SerializeField] private float jumpForce = 15f; // Kode til jumpforde
    private float xInput;
    private bool canJump = true;

    protected override void Update()
    {
        base.Update();
        HandleInput();
    }

    private void HandleInput() // Kode til input
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
            TryToJump();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            HandleAttack();

    }


    protected override void HandleMovement() // Kode til at kunne bevæge sig. 
    {
        if (canMove)
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);

        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

    }


    private void TryToJump() // Kode til Jump

    {
        if (isGrounded && canJump)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public override void EnableMovement(bool enable) // Movement

    {
        base.EnableMovement(enable);
        canJump = enable;

    }


    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }

}
