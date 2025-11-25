using UnityEngine;

public class MountainToWitchBattleChanger : MonoBehaviour
{
   [SerializeField] GameObject player;
   [SerializeField] float playerXPosition = 230.1f;
   [SerializeField] float playerYPosition = 43.96f;


   private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        player = GameObject.FindWithTag("Player");

        player.transform.position = new Vector3(playerXPosition, playerYPosition, player.transform.position.z);
    }
}
