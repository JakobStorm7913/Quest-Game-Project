using UnityEngine;
using Unity.Cinemachine;

public class LeftSideVillageBorder : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera leftSideCam;
    [SerializeField] private CinemachineCamera rightSideCam;

    private bool isDisabled = false;

    public void DisableZone()
    {
        isDisabled = true;

        // Optional: reset camera priorities so this zone stops affecting them
        playerCam.Priority = 0;
        leftSideCam.Priority = 0;
        rightSideCam.Priority = 0;
    }
    public void EnableZone()
    {
        isDisabled = false;
    }
  private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDisabled) return;
        if (!other.CompareTag("Player")) return;
            playerCam.Priority = 1;
            leftSideCam.Priority = 10;
            rightSideCam.Priority = 0;
    }

     private void OnTriggerExit2D(Collider2D other)
    {
        if (isDisabled) return;
        if (!other.CompareTag("Player")) return;
             playerCam.Priority = 10;
            leftSideCam.Priority = 1;
            rightSideCam.Priority = 0;
        }
}