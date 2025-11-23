
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Entity_Enemy : MonoBehaviour
{


    // Overordnet kode der skal bruges enten inden eller når spil er igang sat

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] private float maxHealth = 25;
    [SerializeField] private float currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedBackDuration = .2f;
    private Coroutine damageFeedbackCoroutine;



    [Header("Attack details")]
    [SerializeField] protected float attackRadius; // Den radius der må være
    [SerializeField] protected Transform attackPoint; // Hvor detectiuon sker
    [SerializeField] protected LayerMask whatIsTarget; // Hvad den skal registere

    

    protected int facingDir = 1;
    protected bool facingRight = true; // Kode til retning af player
    
    
    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;

    [SerializeField] private LayerMask whatIsGround;
    protected bool canMove = true;
    
    protected bool isGrounded;


    protected virtual void Awake() 

    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
       

        currentHealth = maxHealth;

    }


    protected virtual void Update()

    {
        HandleCollision();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    public void DamageTargets() // Kode til at se om enemy tager skade eller om enemy bliver ramt

    {

        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget); // Koden vil detecte enemies colliders. 

        foreach (Collider2D enemies in enemiesColliders) // Kode til enemy detection/Encapsulation
        {
            
            Entity_Enemy entityTarget = enemies.GetComponent<Entity_Enemy>();
            entityTarget.TakeDamage();
        }

    }

    public void TakeDamage() // Kode til skade
    {
        currentHealth -= 1;

        PlayDamageFeedback();

        if (currentHealth <= 0)
            Die();  
    }

     protected virtual void Die() // Kode til die

    {
        anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        Destroy(gameObject, 3);
    }



    private void PlayDamageFeedback() // Kode til damageFeedBack
    {
        if (damageFeedbackCoroutine != null)
            StopCoroutine(damageFeedbackCoroutine);

        StartCoroutine(DamageFeedbackCo());
    }

    private IEnumerator DamageFeedbackCo() // Damage Feedback


    {
        Material originalMat = sr.material;

        sr.material = damageMaterial;

        yield return new WaitForSeconds(damageFeedBackDuration);

        sr.material = originalMat;
    }
    public virtual void EnableMovement(bool enable) // Movement

    {
        canMove = enable;
    }

    protected virtual void HandleMovement() // Movement
    {
       
    }

    protected void HandleAnimations() // Kode til animations
    {
        bool isMoving = rb.linearVelocity.x != 0;

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetFloat("xVelocity", rb.linearVelocity.x);

    }

    
    protected virtual void HandleAttack() // Kode til attack
    {
        if (isGrounded)
            anim.SetTrigger("attack");
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }


    protected virtual void HandleCollision() // Kode til 1 jump effect

    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    protected virtual void HandleFlip() // Kode til flip af player

    {

        if (rb.linearVelocity.x > 0 && facingRight == false)
            Flip();
        else if (rb.linearVelocity.x < 0 && facingRight == true)
            Flip();

    }
    public void Flip() // Flip

    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;

    }

    private void OnDrawGizmos() // Kode til Raytracing på enten enemy eller jord osv. 

    {

        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));

        if(attackPoint != null)
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

    }



    

}