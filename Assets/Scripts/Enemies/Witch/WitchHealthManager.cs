using UnityEngine;

public class WitchHealthManager : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float currentHealth = 100;
    [SerializeField] private bool frozen = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;
        
        Debug.Log("Witch took " + damageAmount + "damage | health = " + currentHealth);

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        //Add goob
        Destroy(gameObject);
        WitchCombatManager.Instance.witchSlain = true;
        SoundFXManager.Instance.StopBossBattleMusic();
        WitchCombatManager.Instance.EndCombat();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the thing hitting the witch is the player's attack
        if (other.CompareTag("PlayerAttack"))
        {
                TakeDamage(GameData.Instance.PlayerAttackDamage);
        }
    }
}
