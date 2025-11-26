using System.Collections;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    [SerializeField] private float dodgeDuration = 0.2f;
    [SerializeField] private string normalLayerName = "Player";
    [SerializeField] private string dodgeLayerName = "PlayerDodge";

    [Header("Effects")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer dodgeTrail;
    [SerializeField] private float flickerInterval = 0.01f;
    [SerializeField] private float flickerAlpha = 0.1f;
    [SerializeField] private ParticleSystem dodgeDustPrefab;
    

    [SerializeField] private PlayerMovementScript movementScript;
    private int normalLayer;
    private int dodgeLayer;
    public bool isDodging;
    private Color originalColor;
    
    void Awake()
    {
        normalLayer = LayerMask.NameToLayer(normalLayerName);
        dodgeLayer  = LayerMask.NameToLayer(dodgeLayerName);
        dodgeDustPrefab = Resources.Load<ParticleSystem>("Prefabs/DodgeDust");
        movementScript = GetComponentInParent<PlayerMovementScript>();
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInParent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        if (dodgeTrail != null)
            dodgeTrail.emitting = false; // off by default
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartDodge()
    {
        if (isDodging) return;
        StartCoroutine(DodgeRoutine());

    }

    private IEnumerator DodgeRoutine()
    {
        isDodging = true;

        gameObject.layer = dodgeLayer;

       /* if (dodgeDustPrefab != null)
{
        // spawn at feet - you can tweak this offset
        Vector3 pos = transform.position; //+ Vector3.down * 0.5f;
        Instantiate(dodgeDustPrefab, pos, Quaternion.identity);
}*/

       /* if (dodgeTrail != null)
        {
            dodgeTrail.Clear();
            dodgeTrail.emitting = true;
        }*/

        float elapsed = 0f;
        bool faded = false;

          while (elapsed < dodgeDuration)
        {
            elapsed += Time.deltaTime;

            if (spriteRenderer != null)
            {
                faded = !faded;
                Color c = originalColor;
                c.a = faded ? flickerAlpha : 1f;
                spriteRenderer.color = c;
            }

            yield return new WaitForSeconds(flickerInterval);
        }

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        if (dodgeTrail != null)
            dodgeTrail.emitting = false;

        ResolveStuckInsideEnemies();

        gameObject.layer = normalLayer;
        isDodging = false;
        movementScript.isDodging = false;
        movementScript.EnableMovementAndJump();
    }
     private void ResolveStuckInsideEnemies()
    {
        // Small overlap check around player
        var col = GetComponent<Collider2D>();
        if (col == null) return;

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(LayerMask.GetMask("Enemies"));

        Collider2D[] results = new Collider2D[8];
        int count = col.Overlap(filter, results);

        if (count > 0)
        {
            // Nudge player up a bit until free
            const float step = 0.1f;
            const int maxSteps = 10;

            for (int i = 0; i < maxSteps; i++)
            {
                if (col.Overlap(filter, results) == 0)
                    break;

                transform.position += Vector3.left * step;
            }
        }
    }
}
