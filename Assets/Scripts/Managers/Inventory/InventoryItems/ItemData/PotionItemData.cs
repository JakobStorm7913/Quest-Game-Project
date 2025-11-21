using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Items/Potion Item")]
public class PotionItemData : ItemData
{
    [Header("Potion Settings")]
    public float healAmount = 10f;

}
