using UnityEngine;

public class HealthPotion : InventoryItem
{
    private string itemType;
    private string itemName;
    private float healingAmount;

    public HealthPotion(string type, string name)
    {
        itemType = type;
        itemName = name;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void UseItem()
    {

        throw new System.NotImplementedException();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
