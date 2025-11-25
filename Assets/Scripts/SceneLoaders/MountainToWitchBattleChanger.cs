using UnityEngine;

public class MountainToWitchBattleChanger : MonoBehaviour
{
   [SerializeField] GameObject player;
   [SerializeField] float playerXPosition = 153.74f;
   [SerializeField] float playerYPosition = 36.3f;

   private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        player = GameObject.FindWithTag("Player");

        player.transform.position = new Vector3(playerXPosition, playerYPosition, player.transform.position.z);
    }
}
