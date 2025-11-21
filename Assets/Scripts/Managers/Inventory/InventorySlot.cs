using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int count;

    public bool IsEmpty => item == null || count <= 0;

    public InventorySlot()
    {
        item = null;
        count = 0 ;
    }

    public InventorySlot(ItemData item, int count)
    {
        this.item = item;
        this.count = count;
    }
}