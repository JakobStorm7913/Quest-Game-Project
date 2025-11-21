using UnityEngine;

[CreateAssetMenu(fileName = "NewKey", menuName = "Keys")]
public class KeyItemData : ItemData
{
    [Header("Key Settings")]
    public int useAmount = 1;
}
