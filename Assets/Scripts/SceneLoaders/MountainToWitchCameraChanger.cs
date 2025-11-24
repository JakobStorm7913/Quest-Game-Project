using UnityEngine;
using Unity.Cinemachine;


public class MountainToWitchCameraChanger : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera swampStartCam;
    [SerializeField] private CinemachineCamera endOfRoadCam;
    [SerializeField] private CinemachineCamera witchBattleCam;
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
            playerCam.Priority = 2;
            endOfRoadCam.Priority = 1;
            witchBattleCam.Priority = 10;
            swampStartCam.Priority = 0;
    }
}
