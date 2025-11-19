using UnityEngine;

public abstract class InventoryItem : MonoBehaviour
{
    private string itemType;
    private string itemName;

    protected InventoryItem(string type, string name) {
        itemType = type;
        itemName = name;
    }
    public abstract void UseItem();
    public virtual void PickUpItem()
    {
        InventoryManager.Instance.AddItemToInventory(this);
    }

    public virtual string getName()
    {
        return name;
    }
}
