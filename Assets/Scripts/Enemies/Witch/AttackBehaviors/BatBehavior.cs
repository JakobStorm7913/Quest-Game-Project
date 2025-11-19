using UnityEngine;
using System.Collections;
public class BatBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float batDamage = 5f;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float health = 15f;
    [SerializeField] private float currentHealth;
    [SerializeField] private int direction; // -1 left | +1 right*

    [Header("Player Reference")]
    [SerializeField] private GameObject player;
    Vector2 flightPath;

    [Header ("Effects")]
    [SerializeField] private Animator batAnimator;
    [SerializeField] private float animationDelay = 0.1f;
    [SerializeField] private AudioClip batDeathClip;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        flightPath = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(flightPath.y, flightPath.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        Destroy(gameObject, 30f);
        batDeathClip = Resources.Load<AudioClip>("SoundFX/BatDeathSFX");
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(flightPath * speed * Time.deltaTime, Space.World);
    }

    public void InitializeBat(int dir) {
        direction = dir;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Wall" ||
            collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Door" ||
            collision.gameObject.tag == "Vines") {
                Debug.Log(gameObject.ToString() + " hit " + collision.gameObject.tag.ToString());
            StartCoroutine(DestroyAndPlayHitAnimation());
        }
        if (collision.gameObject.tag == "Player") {
            var dodge = collision.gameObject.GetComponent<PlayerDodge>();
                if (dodge != null && dodge.isDodging)
                {
                // ignore damage during dodge
                return;
                }

            StartCoroutine(DestroyAndPlayHitAnimation());
            GameData.Instance.PlayerHealth -= batDamage;
            Debug.Log(gameObject.ToString() + " hit the player: Player health = " + GameData.Instance.PlayerHealth);
            SoundFXManager.Instance.PlayPlayerDamageSFX();
        }
     }

       void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;
        
        Debug.Log("Spawn took " + damageAmount + "damage | health = " + currentHealth);

        if (currentHealth <= 0) {
            StartCoroutine(DestroyAndPlayHitAnimation());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the thing hitting the witch is the player's attack
        if (other.CompareTag("PlayerAttack"))
        {
                TakeDamage(GameData.Instance.PlayerAttackDamage);
        }
    }

        IEnumerator DestroyAndPlayHitAnimation() {
        //magicAnimator.Play("MagicExplosion");
        
        yield return new WaitForSeconds(animationDelay);
        Destroy (gameObject);
        SoundFXManager.Instance.PlaySoundFX(batDeathClip, transform);
     }
}
