using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class WitchToMountainTeleporter : MonoBehaviour
{
 [SerializeField] GameObject player;
   [SerializeField] float playerXPosition = 118.15f;
   [SerializeField] float playerYPosition = 35.78f;
   [SerializeField] GameObject fogPrefab;
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera swampStartCam;
    [SerializeField] private CinemachineCamera endOfRoadCam;
    [SerializeField] private CinemachineCamera witchBattleCam;

    void Awake()
    {
        fogPrefab = Resources.Load<GameObject>("Prefabs/GiantFog");
    }
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        player = GameObject.FindWithTag("Player");

        player.transform.position = new Vector3(playerXPosition, playerYPosition, player.transform.position.z);
        StartCoroutine(MakeTransition());

    }

    private IEnumerator MakeTransition()
    {
        GameObject GiantFog = Instantiate(fogPrefab, transform.position, Quaternion.identity);
        playerCam.Priority = 10;
        endOfRoadCam.Priority = 2;
        swampStartCam.Priority = 1;
        witchBattleCam.Priority = 0;
        yield return new WaitForSeconds(5f);
        Destroy(GiantFog);
    }
}
