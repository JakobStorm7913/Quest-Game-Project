
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Entity_Enemy : MonoBehaviour
{


    // Overordnet kode der skal bruges enten inden eller når spil er igang sat

    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject player;
    
     
    [Header("KnockBack")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackRadius = 5f;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private bool isKnockedBack = false;
    [SerializeField] private float knockbackTimer = 0.2f;

    [Header("Health")]
    [SerializeField] private float maxHealth = 25;
    [SerializeField] private float currentHealth;


    [Header("Attack details")]
    [SerializeField] private float attackDamage = 5f;


    [Header("Movement")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private int direction; // -1 left | +1 right*
    [SerializeField] private int playerSide; // 0 = left of spawn | 1 = right of spawn
    private bool canMove = true;
    

    [Header("SoundFX")]
    [SerializeField] private AudioClip SpiderDamagedSFX;
    [SerializeField] private AudioClip SpiderDeathSFX;

    [Header("Visual effects")]
    public float fadeSpeed = 1f;          // Higher = faster fade
    public float targetAlphaOnEnter = 0f; // Fully transparent
    public float targetAlphaOnExit = 1f;  // Fully opaque
    protected virtual void Awake() 

    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
       
        currentHealth = maxHealth;
        SpiderDamagedSFX = Resources.Load<AudioClip>("SoundFX/SpiderDamagedSFX");
        SpiderDeathSFX = Resources.Load<AudioClip>("SoundFX/SpiderDeathSFX");

        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (isKnockedBack)
        {
        knockbackTimer -= Time.fixedDeltaTime;
        if (knockbackTimer <= 0f)
        {
            isKnockedBack = false;
        }
        return;
        }
        ChasePlayer();
    }

   void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
           PlayerMeleeAttack playerScript = player.GetComponent<PlayerMeleeAttack>();
           playerScript.TakeDamage(attackDamage);
            Debug.Log(gameObject.ToString() + " hit the player: Player health = " + GameData.Instance.PlayerHealth);
            SoundFXManager.Instance.PlayPlayerDamageSFX();
        }
     }

    public void TakeDamage() // Kode til skade
    {
        currentHealth -= GameData.Instance.PlayerAttackDamage;
        Knockback(0.4f);
        SoundFXManager.Instance.PlaySoundFX(SpiderDamagedSFX, transform, 3f);

        if (currentHealth <= 0)
            Die();  
    }

    private void Die() // Kode til die
    {
        SoundFXManager.Instance.PlaySoundFX(SpiderDeathSFX, transform, 3f);
        anim.enabled = false;

        StartFade(targetAlphaOnExit);
        Destroy(gameObject, 2);
    }

    void Knockback(float duration)
    {
    Debug.Log("knockback triggered");
    Vector3 knockbackPosition = player.transform.position;
    //explosionPosition.y -= 0.5f;
    rb.AddExplosionForce2D(knockbackPosition, knockbackForce, knockbackRadius);
    isKnockedBack = true;
    knockbackTimer = duration;
    }

    public void EnableMovement() // Movement
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

        void CheckPlayerSide() {
        
        Vector3 playerPosition = player.transform.position;
        
        if (playerPosition.x < transform.position.x) {
            playerSide = 0;
        } else {
            playerSide = 1;
        }
    }

    void ChasePlayer()
    {
        if(canMove) {
        Vector2 direction = player.transform.position - transform.position;

        direction.y = 0;
        direction.Normalize();

        transform.Translate(direction * speed * Time.deltaTime);

        CheckPlayerSide();
        if (playerSide == 0) {
            sr.flipX = true;
        } else if (playerSide == 1) {
            sr.flipX = false;
        }
        }
    }

     public void InitializeSpawn(int dir) {
        direction = dir;
    }

     void StartFade(float targetAlpha)
    {
        StartCoroutine(FadeTo(targetAlpha));
    }

   IEnumerator FadeTo(float targetAlpha)
{
    Color original = sr.color;
    Color c = original;

    while (!Mathf.Approximately(c.a, targetAlpha))
    {
        c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);

        // Apply overlay: mix original color with red by the fade amount
        sr.color = Color.Lerp(original, Color.red, 1f - c.a);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, c.a);

        yield return null;
    }
}




    

}