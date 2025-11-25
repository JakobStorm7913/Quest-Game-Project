using UnityEngine;
using Unity.Cinemachine;

public class LeftSideVillageCamera : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera leftSideCam;
    [SerializeField] private CinemachineCamera rightSideCam;
  private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
            playerCam.Priority = 1;
            leftSideCam.Priority = 10;
            rightSideCam.Priority = 0;
    }

     private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
             playerCam.Priority = 10;
            leftSideCam.Priority = 1;
            rightSideCam.Priority = 0;
        }
}