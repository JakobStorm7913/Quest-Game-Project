using UnityEngine;
using System.Collections;

public class MagicAttackBehavior : MonoBehaviour
{
    [Header("Attack attributes")]
    [SerializeField] private float magicDamage = 10f;
    [SerializeField] private float speed = 3f;

    [Header ("Projectile")]
    [SerializeField] private int direction; // -1 left | +1 right*
    
    [Header("Player Reference")]
    [SerializeField] private GameObject player;
    Vector2 flightPath;

    [Header ("Animations")]
    [SerializeField] private Animator magicAnimator;
    [SerializeField] private float animationDelay = 0.1f;

    [Header ("SoundFX")]
    [SerializeField] private AudioSource hitSFX;

    void Awake() {
        //hitSFX= Resources.Load<AudioClip>("SoundFX/");
    }

    void Start() {
        
        player = GameObject.FindWithTag("Player");
        flightPath = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(flightPath.y, flightPath.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        Destroy(gameObject, 30f);
    }
    // Update is called once per frame
    void Update()
    {
        
       transform.Translate(flightPath * speed * Time.deltaTime, Space.World);
    }

    public void InitializeMagic(int dir) {
        direction = dir;
    }

     void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Wall" ||
            collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Vines") {
                Debug.Log(gameObject.ToString() + " hit " + collision.gameObject.tag.ToString());
            StartCoroutine(PlayHitAnimation());
        }
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(PlayHitAnimation());
            GameData.Instance.PlayerHealth -= magicDamage;
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
