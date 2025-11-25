using UnityEngine;

public class WitchHealthManager : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float currentHealth = 100;
    [SerializeField] private bool frozen = true;

    [Header("UI")]
    [SerializeField] private HealthBarFollower follower;

    void Start()
    {
        currentHealth = maxHealth;

        if (follower == null)
        {
            follower = Object.FindFirstObjectByType<HealthBarFollower>();

        }

        if (follower != null)
        {
            follower.BeginFollowing(transform); // uses the method you got errors about
        }
        else
        {
            Debug.LogError("WitchHealthManager: No HealthBarFollower found/assigned!");
        }
    }

    void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        Debug.Log("Witch took " + damageAmount + " damage | health = " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (follower != null)
        {
            follower.ShowHealthBar(false); // this now exists again
        }

        Destroy(gameObject);
        WitchCombatManager.Instance.witchSlain = true;
        SoundFXManager.Instance.StopBossBattleMusic();
        WitchCombatManager.Instance.EndCombat();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            TakeDamage(GameData.Instance.PlayerAttackDamage);
        }
    }
}
