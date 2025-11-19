using UnityEngine;

public class QuestItem : InventoryItem
{
    private bool isQuestItem;
    public QuestItem(string type, string name, bool questItem)
        : base(type, name)
    {
        isQuestItem = questItem;
    }

    public override void UseItem()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
