using UnityEngine;
using System.Collections;
public class PotionAttackBehavior : MonoBehaviour
{
    [Header("Attack attributes")]
    [SerializeField] private float potionDamage = 15f;

    [Header ("Projectile")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float initialUpwardsSpeed = 3f;
    [SerializeField] private int direction; // -1 left | +1 right*

    [Header ("Animations")]
    [SerializeField] private Animator potionAnimator;
    [SerializeField] private float animationDelay = 0.1f;

    [Header ("SoundFX")]
    [SerializeField] private AudioSource HitSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(rb.linearVelocityX, initialUpwardsSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocityY);
    }

    public void InitializePotion(int dir) {
        direction = dir;
    }

     void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Wall" ||
            collision.gameObject.tag == "Ground") {
                Debug.Log(gameObject.ToString() + " hit " + collision.gameObject.tag.ToString());;
            StartCoroutine(PlayHitAnimation());
        }
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(PlayHitAnimation());
            GameData.Instance.PlayerHealth -= potionDamage;
            Debug.Log(gameObject.ToString() + " hit the player: Player health = " + GameData.Instance.PlayerHealth);
            SoundFXManager.Instance.PlayPlayerDamageSFX();
        }
     }

     IEnumerator PlayHitAnimation() {
        //magicAnimator.Play("MagicExplosion");
        
        yield return new WaitForSeconds(animationDelay);
        Destroy (gameObject);
     }
}
