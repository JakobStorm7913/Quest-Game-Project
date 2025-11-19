using UnityEngine;

public class HealthPotion : InventoryItem
{
    private float healingAmount;

    public  HealthPotion(string type, string name, float healAmount) 
    : base(type, name) 
        {
            healingAmount = healAmount;
        }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void UseItem()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
