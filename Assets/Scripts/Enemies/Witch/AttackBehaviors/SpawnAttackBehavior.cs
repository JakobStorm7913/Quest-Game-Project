using UnityEngine;
using System.Collections;

public class SpawnAttackBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float spawnDamage = 10f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float health = 15f;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool isInitialWave;

    [Header ("Side")]
    [SerializeField] private int direction; // -1 left | +1 right*
    [SerializeField] private int playerSide; // 0 = left of spawn | 1 = right of spawn
    private SpriteRenderer spriteRenderer;

    [Header ("Animations")]
    [SerializeField] private Animator spawnAnimator;
    [SerializeField] private float animationDelay = 0.1f;

    [Header ("SoundFX")]
    [SerializeField] private AudioClip deathClip;

    [SerializeField] private GameObject player;


    void Awake() {
        //hitSFX= Resources.Load<AudioClip>("SoundFX/");
    }

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        deathClip = Resources.Load<AudioClip>("SoundFX/SpawnDeathSFX");
        currentHealth = health;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 direction = player.transform.position - transform.position;

        direction.y = 0;
        direction.Normalize();

        transform.Translate(direction * speed * Time.deltaTime);

        CheckPlayerSide();
        if (playerSide == 0) {
            spriteRenderer.flipX = false;
        } else if (playerSide == 1) {
            spriteRenderer.flipX = true;
        }
    }

    public void InitializeSpawn(int dir, bool initialSpawn) {
        direction = dir;
        isInitialWave = initialSpawn;
    }

     void OnCollisionEnter2D(Collision2D collision) {
        /*if (collision.gameObject.tag == "Wall" ||
            collision.gameObject.tag == "Ground") {
                Debug.Log(gameObject.ToString() + " hit " + collision.gameObject.tag.ToString());
            StartCoroutine(PlayHitAnimation());
        }*/
        if (collision.gameObject.tag == "Player") {
            GameData.Instance.PlayerHealth -= spawnDamage;
            Debug.Log(gameObject.ToString() + " hit the player: Player health = " + GameData.Instance.PlayerHealth);
            SoundFXManager.Instance.PlayPlayerDamageSFX();
        }
     }

     IEnumerator PlayHitAnimation() {
        //magicAnimator.Play("MagicExplosion");
        
        yield return new WaitForSeconds(animationDelay);
        Destroy (gameObject);
     }

     void CheckPlayerSide() {
        
        Vector3 playerPosition = player.transform.position;
        
        if (playerPosition.x < transform.position.x) {
            playerSide = 0;
        } else {
            playerSide = 1;
        }
     }

      void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;
        
        Debug.Log("Spawn took " + damageAmount + "damage | health = " + currentHealth);

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        //Add goob
        Destroy(gameObject);
        SoundFXManager.Instance.PlaySoundFX(deathClip, transform);
        if (isInitialWave)
        {
            WitchCombatManager.Instance.initialEnemiesBeaten++;
        }
        Debug.Log(WitchCombatManager.Instance.initialEnemiesBeaten + "inital beaten");
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
