using UnityEngine;
using System.Collections;
public class ItemSpawn : MonoBehaviour
{
    //ADDED UP TO 100
    [SerializeField] private float noSpawnChance = 60f; // = 0-60 to get nothing
    [SerializeField] private float smallHealthPotionChance = 80f; // = 60-80 to get Small Health Potion
    [SerializeField] private float largeHealthPotionChance = 95f; // = 80-95 to get Big Health Potion
    [SerializeField] private float witchKeyChance = 100f; // 95-100 to get the Witch Key
    [SerializeField] private GameObject smallHealthPotionPrefab;
    [SerializeField] private GameObject largeHealthPotionPrefab;
    [SerializeField] private GameObject witchKeyPrefab;

    void Awake()
    {
        smallHealthPotionPrefab = Resources.Load<GameObject>("Prefabs/SmallHealthPotion");
        largeHealthPotionPrefab = Resources.Load<GameObject>("Prefabs/LargeHealhPotion");
        witchKeyPrefab = Resources.Load<GameObject>("Prefabs/WitchKey");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float dropHit = Random.Range(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnRandomItem()
    {
        float DropHit = Random.Range(0, 101);
        if (DropHit <= noSpawnChance)
        {
            break;
        } if (DropHit > noSpawnChance && DropHit <= smallHealthPotionChance)
        {
            GameObject SmallHealthPotion = Instantiate(smallHealthPotionPrefab, transform.position, Quaternion.identity);
            break;
        } if (DropHit > smallHealthPotionChance && DropHit <= largeHealthPotionChance)
        {
            GameObject LargeHealthPotion = Instantiate(largeHealthPotionPrefab, transform.position, Quaternion.identity);
            break;
        } if (DropHit > largeHealthPotionChance)
        {
            GameObject WitchKey = Instantiate(witchKeyPrefab, transform.position, Quaternion.identity);
            witchKeyChance = 101f;
            largeHealthPotionChance = 100f;
            break;
        }
    }  
}
