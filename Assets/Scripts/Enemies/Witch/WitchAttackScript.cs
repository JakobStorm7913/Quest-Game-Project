using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class WitchAttackScript : MonoBehaviour
{

    [Header("Attack attributes")]
    [SerializeField] private float minTimeBetweenAttacks = 3f;
    [SerializeField] private float maxTimeBetweenAttacks = 5f;

    [Header("Attack References")]
    [SerializeField] private GameObject magicPrefab;
   // [SerializeField] private GameObject splashpotionPrefab;
    [SerializeField] private GameObject batPrefab;
    [SerializeField] private GameObject spawnPrefab;

    [Header("Other References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Animator animator;

    [Header("Witch Explosion")]
    [SerializeField] private float explosionForce = 100f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private Rigidbody2D playerRB;
    //[SerializeField] private WitchCombatManager combatManager;

    [Header ("Player Position")]
    [SerializeField] private int playerSide; // 0 = left of witch | 1 = right of witch

    [Header ("Magic  Spawn Variables")]
    [SerializeField] private float MagicYOffset = -0.1f;
    [SerializeField] private float MagicXOffset = 0.5f;

    /*[Header ("Potion Spawn Variables")]
    [SerializeField] private float PotionYOffset = 0.5f;
    [SerializeField] private float PotionXOffset = 0.5f;
    */
    [Header ("Enemy Spawn Variables")]
    [SerializeField] private float SpawnYOffset = 0f;
    [SerializeField] private float SpawnXOffset = 2f;
     [SerializeField] LayerMask groundMask;

    [Header ("Bat Spawn Variables")]
    [SerializeField] private float BatYOffset = 5f;
    [SerializeField] private float BatXOffset = 2f;

    [Header ("Animation Delays")]
    [SerializeField] private float MagicAnimDelay = 0.1f;
    [SerializeField] private float PotionAnimDelay = 0.2f;
    [SerializeField] private float SpawnAnimDelay = 0.3f;
    [SerializeField] private float BatAnimDelay = 0.2f;

        [Header("Witch SFX")]
    [SerializeField] private AudioClip witchScreamClip;
    //[SerializeField] private AudioClip witchMagicAttackClip;
    //[SerializeField] private AudioClip potionAttackClip;
    [SerializeField] private AudioClip spawnAttackClip;

    [Header("Magic attack SFX")]
    [SerializeField] private AudioClip magicAttackClip;

    [Header("Bat")]
    [SerializeField] private AudioClip batFlyingClip;
    [SerializeField] private AudioClip batAttackClip;

    [Header("Spawn Attack SFX")]
    [SerializeField] private AudioClip spawnClip;


    /*[SerializeField] private AudioClip potionAttackSoundClip;
    [SerializeField] private AudioClip magicAttackSoundClip;
    [SerializeField] private AudioClip enemySpawnSoundClip
    [SerializeField] private AudioClip batSpawnSoundClip
    */

    [SerializeField] private int AttackID; // 0 = (Base) Magic atk | 1 = Splashpotion attack | 2 = Mob spawn attack | 3 = Bat spawn attack
    [SerializeField] private bool initialCombatBeaten = false;
    [SerializeField] private bool normalCombatStarted = false;

     void Awake() {
        //Set prefabs
        magicPrefab = Resources.Load<GameObject>("Prefabs/MagicBallAttack");
       // splashpotionPrefab = Resources.Load<GameObject>("Prefabs/WitchAttackPotion");
        batPrefab = Resources.Load<GameObject>("Prefabs/WitchBatAttack");
        spawnPrefab = Resources.Load<GameObject>("Prefabs/Spider");

        playerRB = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();

        InitializeSounds();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       /* if (combatManager == null) {
            combatManager = GameObject.Find("WitchFightZone").GetComponent<WitchCombatManager>(); 
        } */
        player = GameObject.FindWithTag("Player");
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!normalCombatStarted)
        {
            CheckIfInitialCombatBeaten();
            StartCoroutine(NormalAttackSequence());
        }
    }

    public void SpawnInitialCombat() {
        for (float gap = 0f; gap <= 0.9f; gap = gap + 0.3f) {
        StartCoroutine(InitialSpawnAttack(gap));
        }
    }

    public IEnumerator NormalAttackSequence() {
        while(initialCombatBeaten) {
        float timeToNextAttack = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);

        yield return new WaitForSeconds(timeToNextAttack);

        DoRandomAttack();
        }
    }

    private void CheckIfInitialCombatBeaten()
    {
        if (WitchCombatManager.Instance.initialEnemiesBeaten >= 5)
        {
            initialCombatBeaten = true;
            normalCombatStarted = true;
        }
    }

    void DoRandomAttack() {
        AttackID = Random.Range(0, 3); // 0 = (Base) Magic atk | 1 = Splashpotion attack | 2 = Mob spawn attack | 3 = Bat spawn attack --- 4 will never trigger, chooses random between 0-3

        switch (AttackID) {
            case 0:
                StartCoroutine(MagicAttack());
                //Explode();
                break;
            case 1:
                StartCoroutine(EnemySpawnAttack());
                break;
            case 2:
                StartCoroutine(BatAttack());
                break;
        }
    }

    void CheckPlayerSide() {
        
        Vector3 playerPosition = player.transform.position;
        
        if (playerPosition.x < transform.position.x) {
            playerSide = 0;
        } else {
            playerSide = 1;
        }

        Debug.Log($"ATTACKSCRIPT: Player x: {playerPosition.x}, Witch x: {transform.position.x}, Side: {playerSide}");
    }

    IEnumerator MagicAttack() {
        if (animator != null) {
            animator.SetTrigger("MagicAttack");
        } 
        yield return new WaitForSeconds(MagicAnimDelay);

        Vector3 MagicSpawnPosition = transform.position;

        CheckPlayerSide();

        if (playerSide == 0) {
            MagicSpawnPosition.x -= MagicXOffset;
        } else if (playerSide == 1) {
            MagicSpawnPosition.x += MagicXOffset;
        }
        
        MagicSpawnPosition.y += MagicYOffset;

        //Spawn MagicBall
        GameObject Magic = Instantiate(magicPrefab, MagicSpawnPosition, Quaternion.identity);
        MagicAttackBehavior MagicAttack = Magic.GetComponent<MagicAttackBehavior>();
         if (Magic != null) {
            int dir = (playerSide == 0) ? -1 : 1;
            MagicAttack.InitializeMagic(dir);
         }

         SoundFXManager.Instance.PlaySoundFX(magicAttackClip, transform);

    }

   /* IEnumerator SplashPotionAttack() {
        if (animator != null) {
            animator.SetTrigger("PotionAttack");
        }
        yield return new WaitForSeconds(PotionAnimDelay);

        Vector3 PotionSpawnPosition = transform.position;

        CheckPlayerSide();

        if (playerSide == 0) {
            PotionSpawnPosition.x -= PotionXOffset;
        } else if (playerSide == 1) {
            PotionSpawnPosition.x += PotionXOffset;
        }

        PotionSpawnPosition.y += PotionYOffset;

        //Spawn Potion
        GameObject WitchPotion = Instantiate(splashpotionPrefab, PotionSpawnPosition, Quaternion.identity);
        PotionAttackBehavior PotionAttack = WitchPotion.GetComponent<PotionAttackBehavior>();
         if (WitchPotion != null) {
            int dir = (playerSide == 0) ? -1 : 1;
            PotionAttack.InitializePotion(dir);
         }
        
    }*/

    IEnumerator EnemySpawnAttack() {
       Vector3 EnemySpawnPosition = transform.position;
        
        CheckPlayerSide();

        if (playerSide == 0) {
            EnemySpawnPosition.x -= SpawnXOffset;
        } else if (playerSide == 1) {
            EnemySpawnPosition.x += SpawnXOffset;
        }

        EnemySpawnPosition.y += SpawnYOffset;

        RaycastHit2D hit = Physics2D.Raycast(EnemySpawnPosition + Vector3.up * 2f, Vector2.down, 10f, groundMask);

    if (hit.collider != null)
    {
        // Place spider on the ground surface
        EnemySpawnPosition = hit.point;
        EnemySpawnPosition.y += SpawnYOffset; // slight offset so it isn’t inside the ground
    }
    else
    {
        // No ground found → don't spawn
        Debug.LogWarning("No ground under spawn position, skipping spider spawn.");
    }

    GameObject spider = Instantiate(spawnPrefab, EnemySpawnPosition, Quaternion.identity);

    StartCoroutine(SpawnEffect(spider.transform));
    SoundFXManager.Instance.PlaySoundFX(spawnAttackClip, transform);
    yield return null;
    }

    IEnumerator BatAttack() {
        if (animator != null) {
            animator.SetTrigger("BatAttack");
        }

        yield return new WaitForSeconds(BatAnimDelay);

        Vector3 BatSpawnPosition = transform.position;

        CheckPlayerSide();

        if (playerSide == 0) {
            BatSpawnPosition.x += BatXOffset;
        } else if (playerSide == 1) {
            BatSpawnPosition.x -= BatXOffset;
        }

        BatSpawnPosition.y += BatYOffset;

        //Spawn Bat
        GameObject Bat = Instantiate(batPrefab, BatSpawnPosition, Quaternion.identity);
        BatBehavior BatAttack = Bat.GetComponent<BatBehavior>();
         if (Bat != null) {
            int dir = (playerSide == 0) ? -1 : 1;
            BatAttack.InitializeBat(dir);
        }

        SoundFXManager.Instance.PlaySoundFX(batAttackClip, transform);
    }
    
    IEnumerator InitialSpawnAttack(float gap) {

        Vector3 EnemySpawnPosition = transform.position;
        
        CheckPlayerSide();

        if (playerSide == 0) {
            EnemySpawnPosition.x -= SpawnXOffset + gap;
        } else if (playerSide == 1) {
            EnemySpawnPosition.x += SpawnXOffset;
        }

        EnemySpawnPosition.y += SpawnYOffset;

        RaycastHit2D hit = Physics2D.Raycast(EnemySpawnPosition + Vector3.up * 2f, Vector2.down, 10f, groundMask);

    if (hit.collider != null)
    {
        // Place spider on the ground surface
        EnemySpawnPosition = hit.point;
        EnemySpawnPosition.y += SpawnYOffset; // slight offset so it isn’t inside the ground
    }
    else
    {
        // No ground found → don't spawn
        Debug.LogWarning("No ground under spawn position, skipping spider spawn.");
    }

    GameObject spider = Instantiate(spawnPrefab, EnemySpawnPosition, Quaternion.identity);

    StartCoroutine(SpawnEffect(spider.transform));
    SoundFXManager.Instance.PlaySoundFX(spawnAttackClip, transform);
    yield return null;
    }

    void Explode()
    {
    Debug.Log("EXPLOSION triggered");
    Vector3 explosionPosition = transform.position;
    //explosionPosition.y -= 0.5f;
    playerRB.AddExplosionForce2D(explosionPosition, explosionForce, explosionRadius);
    var movement = playerRB.GetComponent<PlayerMovementScript>();
    if (movement != null)
    {
        movement.ApplyKnockback(1f);
    }
    }

    private void InitializeSounds()
    {
        //Witch
        witchScreamClip = Resources.Load<AudioClip>("SoundFX/WitchScream");
        magicAttackClip = Resources.Load<AudioClip>("SoundFX/MagicSFX");
        batFlyingClip = Resources.Load<AudioClip>("SoundFX/BatFlyingSFX");

        //WitchSpawns
        spawnClip = Resources.Load<AudioClip>("SoundFX/WitchSpawnSFX");
        
        //Explosion
    }

  IEnumerator SpawnEffect(Transform t, float distance = 0.6f, float duration = 0.6f)
{
    // Save final values
    Vector3 finalPosition = t.position;
    Vector3 finalScale    = t.localScale;

    // Start hidden & underground
    t.localScale = Vector3.zero;
    t.position   = finalPosition - new Vector3(0f, distance, 0f);

    // Optional random delay – but spider stays invisible during this
    float randomDelay = Random.Range(0f, 0.4f);
    if (randomDelay > 0f)
        yield return new WaitForSeconds(randomDelay);

    float time = 0f;

    while (time < duration)
    {
        float tNorm = time / duration;

        // Move up from underground to final position
        t.position = Vector3.Lerp(t.position, finalPosition, tNorm);

        // Scale from 0 → full
        t.localScale = Vector3.Lerp(Vector3.zero, finalScale, tNorm);

        time += Time.deltaTime;
        yield return null;
    }

    // Snap to final values to avoid tiny float errors
    t.position   = finalPosition;
    t.localScale = finalScale;
}
}
