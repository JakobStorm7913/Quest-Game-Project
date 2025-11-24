using UnityEngine;
using Unity.Cinemachine;

public class EndOfRoadCameraChanger : MonoBehaviour
{
 [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera swampStartCam;
    [SerializeField] private CinemachineCamera endOfRoadCam;
  private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
            playerCam.Priority = 1;
            endOfRoadCam.Priority = 10;
            swampStartCam.Priority = 0;
    }

     private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
            playerCam.Priority = 10;
            endOfRoadCam.Priority = 1;
            swampStartCam.Priority = 0;
        }
}
