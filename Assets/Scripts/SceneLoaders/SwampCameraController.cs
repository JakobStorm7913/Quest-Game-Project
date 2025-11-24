using UnityEngine;
using Unity.Cinemachine;

public class SwampCameraController : MonoBehaviour
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
            witchBattleCam.Priority = 0;
            swampStartCam.Priority = 10;
    }

     private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
            playerCam.Priority = 10;
            endOfRoadCam.Priority = 2;
            swampStartCam.Priority = 1;
            witchBattleCam.Priority = 0;
        }
    }
