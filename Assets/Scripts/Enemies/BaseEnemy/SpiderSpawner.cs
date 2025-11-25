using UnityEngine;
using System.Collections;

public class SpiderSpawner : MonoBehaviour
{
    [SerializeField] float spawnRate = 10f;
    [SerializeField] int MaxSpidersOnScreen = 3;
    [SerializeField] int enemyCount;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float activationRange = 12f;


    [Header("References")]
    [SerializeField] GameObject spiderPrefab;
    [SerializeField] GameObject player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        spiderPrefab = Resources.Load<GameObject>("Prefabs/Spider");
    }
    // Update is called once per frame
  void Update()
    {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist > activationRange) return;

        enemyCount = GameObject.FindGameObjectsWithTag("Enemies").Length;

        // Example: only spawn when under the limit
        if (enemyCount < MaxSpidersOnScreen)
        {
            spawnRate -= Time.deltaTime;
            if (spawnRate <= 0f)
            {
                SpawnSpider();
                spawnRate = 7f;
            }
        }
    }

void SpawnSpider()
{
    Vector3 spawnPos = player.transform.position;
    spawnPos.x += Random.Range(-10f, 10f);

    // Raycast downwards to find the ground
    RaycastHit2D hit = Physics2D.Raycast(spawnPos + Vector3.up * 2f, Vector2.down, 10f, groundMask);

    if (hit.collider != null)
    {
        // Place spider on the ground surface
        spawnPos = hit.point;
        spawnPos.y += 0.3f; // slight offset so it isn’t inside the ground
    }
    else
    {
        // No ground found → don't spawn
        Debug.LogWarning("No ground under spawn position, skipping spider spawn.");
        return;
    }

    GameObject spider = Instantiate(spiderPrefab, spawnPos, Quaternion.identity);

    StartCoroutine(SpawnEffect(spider.transform));
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
