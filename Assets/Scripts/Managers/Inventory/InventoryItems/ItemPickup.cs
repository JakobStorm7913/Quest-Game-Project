using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class ItemPickup : MonoBehaviour
{

    public ItemData itemData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        InventoryManager.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
