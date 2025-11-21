using UnityEngine;
using System.Collections;
public class ItemSpawn : MonoBehaviour
{
    //ADDED UP TO 100
    [SerializeField] private float noSpawnChance = 75f; // = 0-75 to get nothing
    [SerializeField] private float smallHealthPotionChance = 90f; // = 75-90 to get Small Health Potion
    [SerializeField] private float bigHealthPotionChance = 100f; // = 90-100 to get Big Health Potion

    [SerializeField] private GameObject smallHealthPotionPrefab;
    [SerializeField] private GameObject largeHealthPotionPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      //  float dropHit = Random.Range(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   /* private IEnumerator SpawnRandomItem()
    {
        float DropHit = Random.Range(0, 101);
        if (DropHit <= noSpawnChance)
        {
            break;
        } if (DropHit >= noSpawnChance && DropHit <= smallHealthPotionChance)
        {
            HealthPotion SmallHealthPotion = new HealthPotion("Potion", "Small Health Potion");
        }
    }    */
}
