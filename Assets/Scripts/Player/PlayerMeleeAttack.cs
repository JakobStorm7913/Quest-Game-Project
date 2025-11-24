using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttack : MonoBehaviour
{


    protected SpriteRenderer sr;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    [SerializeField] GameObject player;
    [SerializeField] PlayerMovementScript movementScript;

   
    [Header("Attack details")]
    [SerializeField] protected float attackRadius; // Den radius der må være
    [SerializeField] protected Transform attackPoint; // Hvor detectiuon sker
    [SerializeField] protected LayerMask whatIsTarget; // Hvad den skal registere
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackTimer = 0f;
    [SerializeField] private bool isAttacking = true;

    [Header("Health")] // Ellers du Attack details ikke
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedBackDuration = .2f;
    private Coroutine damageFeedbackCoroutine;

    protected int facingDir = 1;
    protected bool facingRight = true; // Kode til retning af player
    
    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;

    [SerializeField] private LayerMask whatIsGround;
    protected bool canMove = true;
    
    protected bool isGrounded;


    [Header("Movement details")]
    private float xInput;
    private bool canJump = true;

    [Header("Input")]
    [SerializeField] private InputAction attackAction;




    private void Awake()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        attackAction.Enable();
        movementScript = player.GetComponent<PlayerMovementScript>();

        sr = GetComponentInChildren<SpriteRenderer>();
    }

   
    /*private IEnumerator DamageFeedbackCo() // Damage Feedback 
    {
        Material originalMat = sr.material;

        sr.material = damageMaterial;

        yield return new WaitForSeconds(damageFeedBackDuration);

        sr.material = originalMat;
    }*/
    void Update()
    {
        if (attackAction.WasPressedThisFrame()) 
        {
            if (attackCooldown > attackTimer)
            {
            Debug.Log("Attack input detected!");
            isAttacking = true;
            }

          if (isAttacking)
            {
            attackTimer += Time.fixedDeltaTime;
            }
    }
    }
    void FixedUpdate()
    {
        if (isAttacking)
        {
        if (movementScript.isKnockedBack) return;
        
        DamageTargets();
        }
    }

    public void DamageTargets() // Kode til at se om enemy tager skade eller om enemy bliver ramt
    {
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget); // Koden vil detecte enemies colliders. 

        foreach (Collider2D enemies in enemiesColliders) // Kode til enemy detection/Encapsulation
        {

            Entity_Enemy entityTarget = enemies.GetComponent<Entity_Enemy>();
            entityTarget.TakeDamage();
            SoundFXManager.Instance.PlayPlayerDamageSFX();
        }
        isAttacking = false;
    }

/*
          private void PlayDamageFeedback() // Kode til damageFeedBack
    {
        if (damageFeedbackCoroutine != null)
            StopCoroutine(damageFeedbackCoroutine);
        StartCoroutine(DamageFeedbackCo());
    }

    private IEnumerator DamageFeedbackCo() // Damage Feedback
    {
        SoundFXManager.Instance.PlaySoundFX(SpiderDamaged, transform.position, 3f);
    }*/
    

    public void TakeDamage(float damage) // Kode til skade
    {
        
        GameData.Instance.PlayerHealth -= damage;

        //PlayDamageFeedback();

        if (currentHealth <= 0)
        Die();
    }


     protected virtual void Die() // Kode til die

    {
        anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        Destroy(gameObject, 3f);
    }
}
