using UnityEngine;
using System.Collections.Generic; 
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Settings")]
    [SerializeField] private int slotCount = 5;
    [SerializeField] private int stackLimit = 10;

    [SerializeField] private List<InventorySlot> slots;

    public IReadOnlyList<InventorySlot> Slots => slots;
    public int SlotCount => slotCount;
    public int StackLimit => stackLimit;

    [Header("Special Items")]
    [SerializeField] private ItemData witchKeyItem;
    [SerializeField] private ItemData antidoteItem;

    [Header("Actions")]
    public System.Action OnInventoryChanged;
    private InputAction slot1Action;
    private InputAction slot2Action;
    private InputAction slot3Action;
    private InputAction slot4Action;
    private InputAction slot5Action;

    [Header("References")]
    private PlayerUseQuestItem playerUse;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (slots == null || slots.Count != slotCount)
        {
            slots = new List<InventorySlot>(slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                slots.Add(new InventorySlot());
            }
        }
        EnableInventoryActions();
        playerUse = GameObject.FindWithTag("Player").GetComponent<PlayerUseQuestItem>();
    }

    public void AddItem(ItemData itemData)
    {
        foreach (var slot in slots) //First check if we already hold an item of this type and adds 1 to the stack.
        {
            if (slot.item == itemData && slot.count < stackLimit)
            {
                slot.count++;
                OnInventoryChanged?.Invoke();
                return;
            }
        }
        foreach (var slot in slots) //Second put it in an empty slot if there is one
        {
            if (slot.IsEmpty)
            {
                slot.item = itemData;
                slot.count = 1;
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        Debug.Log("Inventory full, cant add: " + itemData.itemName);
    }

    public void UseItem(int slotIndex)
    {
        Debug.Log("UseItem called for slot " + slotIndex);
        if (slotIndex < 0 || slotIndex >= slotCount) return; //making sure we dont grab an index out of range

    var slot = slots[slotIndex];
    if (slot.IsEmpty) return;

    if (slot.item is PotionItemData potion)
        {
            if (GameData.Instance != null)
            {
                Debug.Log("Previous Player Health: " + GameData.Instance.PlayerHealth);
                GameData.Instance.PlayerHealth += potion.healAmount;
                if (GameData.Instance.PlayerHealth > 100) {
                    GameData.Instance.PlayerHealth = 100;
                    Debug.Log("Player reached full health");
                }
                slot.count--;
                Debug.Log("Player was healed: " + potion.healAmount + ", New HP: " + GameData.Instance.PlayerHealth);
            }
        }
    else if (slot.item.itemType == ItemType.QuestItem)
        {
        if (slot.item == antidoteItem) 
            {
            if (playerUse != null)
                {
                if (playerUse.UseAntidote())
                    {
                    slot.count--;
                    }
                }
            }
        }
    else if (slot.item.itemType == ItemType.Key)
        {
        if (slot.item == witchKeyItem) 
            {
            if (playerUse != null)
                {
                if (playerUse.UseWitchKey())
                    {
                    slot.count--;
                    }
                }
            }
        }
    
    if (slot.count <= 0)
        {
            slot.item = null;
            slot.count = 0;
        }

    OnInventoryChanged?.Invoke();
}
    

void EnableInventoryActions()
{
    slot1Action = InputSystem.actions.FindAction("UseItem0");
    if (slot1Action != null)
    {
        slot1Action.Enable();
        slot1Action.performed += ctx => UseItem(0);
    }

    slot2Action = InputSystem.actions.FindAction("UseItem1");
    if (slot2Action != null)
    {
        slot2Action.Enable();
        slot2Action.performed += ctx => UseItem(1);
    }

    slot3Action = InputSystem.actions.FindAction("UseItem2");
    if (slot3Action != null)
    {
        slot3Action.Enable();
        slot3Action.performed += ctx => UseItem(2);
    }

    slot4Action = InputSystem.actions.FindAction("UseItem3");
    if (slot4Action != null)
    {
        slot4Action.Enable();
        slot4Action.performed += ctx => UseItem(3);
    }

    slot5Action = InputSystem.actions.FindAction("UseItem4");
    if (slot5Action != null)
    {
        slot5Action.Enable();
        slot5Action.performed += ctx => UseItem(4);
    }
}
}