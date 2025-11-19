using UnityEngine;

public class WitchDoor : MonoBehaviour
{
    [SerializeField] private float maxHealth = 15;
    [SerializeField] private float currentHealth = 15;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;
        
        Debug.Log("Door took " + damageAmount + "damage | health = " + currentHealth);

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        //Add goob
        Destroy(gameObject);
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
