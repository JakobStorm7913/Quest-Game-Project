using System.Collections;
using UnityEngine;

public class WitchHealthManager : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float currentHealth = 100;
    [SerializeField] private bool frozen = true;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject curseCurePrefab;

    [Header("UI")]
    [SerializeField] private HealthBarFollower follower;
    void Awake()
    {
        curseCurePrefab = Resources.Load<GameObject>("Prefabs/CurseCure");
    }

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

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

    void TakeDamage()
    {
        currentHealth -= GameData.Instance.PlayerAttackDamage;

        Debug.Log("Witch took " + GameData.Instance.PlayerAttackDamage + " damage | health = " + currentHealth);

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
        WitchCombatManager.Instance.witchSlain = true;
        SoundFXManager.Instance.StopBossBattleMusic();
        WitchCombatManager.Instance.EndCombat();
        StartCoroutine(PlayDeathAnimation());
    }

    IEnumerator PlayDeathAnimation()
    {
        animator.Play("WitchDeath");
        yield return new WaitForSeconds(0.4f);
        GameObject CurseCure = Instantiate(curseCurePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
