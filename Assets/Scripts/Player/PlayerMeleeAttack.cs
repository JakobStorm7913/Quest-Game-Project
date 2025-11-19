using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Hitbox")]
    [SerializeField] private Collider2D attackHitbox;
    [SerializeField] private float attackDuration = 0.15f;
    [SerializeField] private float attackCooldown = 0.4f;

    private bool isAttacking = false;
    private float lastAttackTime = -999f;

    private InputAction attackAction;

    private void Awake()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        attackAction.Enable();
        attackAction.performed += OnAttackTriggered;
    }

    private void Start()
    {
        if (attackHitbox != null)
            attackHitbox.enabled = false;
    }

    private void OnAttackTriggered(InputAction.CallbackContext ctx)
    { 
        if (isAttacking) return;
        if (!isAttacking) {
        
            if (Time.time < lastAttackTime + attackCooldown) return;

            StartCoroutine(DoAttack());
        }
    }

    private IEnumerator DoAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        attackHitbox.enabled = true;
        SoundFXManager.Instance.PlayPlayerAttackSFX();
        Debug.Log("Hitbox state = " + attackHitbox.enabled.ToString());

        yield return new WaitForSeconds(attackDuration);

        attackHitbox.enabled = false;
        Debug.Log("Hitbox state = " + attackHitbox.enabled.ToString());
        isAttacking = false;
    }
}
