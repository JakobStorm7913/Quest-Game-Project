using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class WitchCombatTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private WitchCombatManager combatManager;

    [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera bossCam;

    [Header("Vine")]
    [SerializeField] private GameObject vinesPrefab;
    [SerializeField] private float vinesX = -10.6f;
    [SerializeField] private float vinesY = -1.76f;

    [Header("House")]
    //[SerializeField] private GameObject witchHouse;
    //[SerializeField] private GameObject housePrefab;
    //[SerializeField] private float houseX = 4.16f;
    //[SerializeField] private float houseY = 7.51f;

    [Header("SoundFX")]
    [SerializeField] private AudioClip witchEnterSFX;
    



    void Awake() {
        combatManager = WitchCombatManager.Instance;
        vinesPrefab = Resources.Load<GameObject>("Prefabs/Vines");
        //housePrefab = Resources.Load<GameObject>("Prefabs/WitchHouse");
        //witchHouse = GameObject.Find("WitchHouse");
        witchEnterSFX = Resources.Load<AudioClip>("SoundFX/WitchScream");
    }
    void Start() 
    {
        if (combatManager == null) 
        {
           Debug.LogError("WitchCombatManager.Instance 404");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (combatManager != null)
        {
            //Camera switch
            //Destroy(witchHouse);
            bossCam.Priority = 10;
            playerCam.Priority = 0;

        if(!WitchCombatManager.Instance.witchSlain) {
            Debug.Log("Player entered Witch fight");
            SoundFXManager.Instance.StartBossBattleMusic();
            StartCoroutine(SpawnVines());
            SoundFXManager.Instance.PlaySoundFX(witchEnterSFX, transform, 1f);
            combatManager.StartCombat();
            }
        }
        else {
            Debug.LogError("null error");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) {
            return;
        }
        if (other.CompareTag(playerTag))
        {
            playerCam.Priority = 10;
            bossCam.Priority = 0;
           // witchHouse = Instantiate(housePrefab, new Vector3(houseX, houseY, 0), Quaternion.identity);
        }
    }

    IEnumerator SpawnVines()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject Vines = Instantiate(vinesPrefab, new Vector3(vinesX, vinesY, 0), Quaternion.identity);
    }
}
