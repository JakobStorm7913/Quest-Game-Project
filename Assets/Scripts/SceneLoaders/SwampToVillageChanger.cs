using UnityEngine;
using UnityEngine.SceneManagement;

public class SwampToVillageChanger : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        SceneManager.LoadScene("Village");
        
    }
}
