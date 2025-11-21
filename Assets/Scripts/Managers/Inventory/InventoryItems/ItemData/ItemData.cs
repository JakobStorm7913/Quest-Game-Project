using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Basic info")]
    public string itemName = "New Item";
    public ItemType itemType = ItemType.Other;
    public Sprite icon;
    public string description;
}
