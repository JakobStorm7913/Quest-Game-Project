using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerMovementScript movementScript;
    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;

    [Header("Attack details")]
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float attackCooldown = 0.5f;   // time between attacks
    [SerializeField] private float attackDuration = 0.5f;   // length of attack animation (tweak in inspector)

    private float nextAttackTime = 0f;
    private bool isAttacking;
     private int deathLayer;


    [Header("Input")]
    [SerializeField] private InputAction attackAction;

    private void Awake()
    {
        if (player == null) player = gameObject;

       
        movementScript = GetComponentInParent<PlayerMovementScript>();
        sr  = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb  = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        deathLayer = LayerMask.NameToLayer("PlayerDodge");
    }

    private void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");

        if (attackAction == null)
        {
            Debug.LogError("[Melee] No Attack InputAction found. " +
                           "Make sure there is an action named 'Attack' in the same asset used by your movement.");
            return;
        }

        attackAction.Enable();
        Debug.Log("[Melee] Start OK. Using Attack action: " + attackAction.name);
    }

    private void Update()
    {
        if (attackAction == null)
            return;

        if (attackAction.WasPressedThisFrame())
        {
            Debug.Log("[Melee] Attack button pressed this frame.");
            TryStartAttack();
        }
    }

    private void TryStartAttack()
    {
        if (isAttacking) return;
        if (Time.time < nextAttackTime)
        {
            Debug.Log("[Melee] Attack blocked by cooldown. Time left: " + (nextAttackTime - Time.time));
            return;
        }

        if (movementScript != null && movementScript.isKnockedBack)
        {
            Debug.Log("[Melee] Attack blocked, player is knocked back.");
            return;
        }

        Debug.Log("[Melee] Attack started.");
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        // Freeze player during attack
        if (movementScript != null)
        {
            movementScript.DisableMovementAndJump();
        }

        if (anim != null)
        {
            anim.SetTrigger("attack");
        }

        // Wait until the “hit” moment
        yield return new WaitForSeconds(attackDuration * 0.5f);

        Debug.Log("[Melee] Dealing damage.");
        DamageTargets();

        // Wait until the animation is done
        yield return new WaitForSeconds(attackDuration);// * 0.7f);

        if (movementScript != null)
        {
            movementScript.EnableMovementAndJump();
        }
        isAttacking = false;
        Debug.Log("[Melee] Attack finished.");
    }

    public void DamageTargets()
    {
        if (attackPoint == null) 
        { 
            Debug.LogWarning("[Melee] No attackPoint assigned, cannot damage targets.");
            return;
        }

        /*Vector3 localPos = attackPoint.localPosition;
        localPos.x *= -1;   // mirror horizontally
        attackPoint.localPosition = localPos;*/

        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            whatIsTarget
        );

        /*foreach (Collider2D enemies in enemiesColliders)
        {
            Entity_Enemy entityTarget = enemies.GetComponent<Entity_Enemy>();
            if (entityTarget != null)
            {
                entityTarget.TakeDamage();
                SoundFXManager.Instance.PlayPlayerAttackSFX();
            }
        }*/

        foreach (Collider2D hit in enemiesColliders)
        {
        // Call TakeDamage() on whatever was hit, if it has it.
        hit.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);

        SoundFXManager.Instance.PlayPlayerAttackSFX();
        }

    }

    public void TakeDamage(float damage)
    {
        GameData.Instance.PlayerHealth -= damage;

        if (GameData.Instance.PlayerHealth <= 0) {
            Die();
            return;
        }

        SoundFXManager.Instance.PlayPlayerDamageSFX();
    }

    private void Die()
    {
       /* if (rb != null)
        {
            rb.gravityScale = 12;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
        } */
        SoundFXManager.Instance.SetMusicVolume(0f);
        SoundFXManager.Instance.PlayPlayerDeathSFX();
        gameObject.layer = deathLayer;
        rb.bodyType = RigidbodyType2D.Static; //freeze player position on death
        movementScript.DisableMovementAndJump();
        anim.Play("playerDeath");

        Destroy(gameObject, 3f);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }

 
}
