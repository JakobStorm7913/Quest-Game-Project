using UnityEngine;
using Unity.Cinemachine;

public class SwampCameraController : MonoBehaviour
{
     [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera swampStartCam;
  private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
            playerCam.Priority = 0;
            swampStartCam.Priority = 10;
    }

     private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
            playerCam.Priority = 10;
            swampStartCam.Priority = 0;
        }
    }
