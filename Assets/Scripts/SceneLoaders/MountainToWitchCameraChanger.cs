using UnityEngine;
using Unity.Cinemachine;
using System.Collections;


public class MountainToWitchCameraChanger : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera swampStartCam;
    [SerializeField] private CinemachineCamera endOfRoadCam;
    [SerializeField] private CinemachineCamera witchBattleCam;
    [SerializeField] GameObject fogPrefab;

    void Awake()
    {
        fogPrefab = Resources.Load<GameObject>("Prefabs/GiantFog");
    }
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
            StartCoroutine(MakeTransition());
    }

       private IEnumerator MakeTransition()
    {
        GameObject GiantFog = Instantiate(fogPrefab, transform.position, Quaternion.identity);
        playerCam.Priority = 2;
        endOfRoadCam.Priority = 1;
        swampStartCam.Priority = 0;
        witchBattleCam.Priority = 10;
        yield return new WaitForSeconds(5f);
        Destroy(GiantFog);
    }

    
}
