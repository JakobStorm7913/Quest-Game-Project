using UnityEngine;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance  { get; private set; }
    [SerializeField] public int InventorySlots { get; set; }
    [SerializeField] public int SlotsMaxCount { get; set; }
    [SerializeField] public List<InventorySlot> Inventory { get; set; }

    void Awake() {
        if (Instance == null) {
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InventorySlots = 5;
            SlotsMaxCount = 10;
            Inventory = new List<InventorySlot>(InventorySlots);
            InitializeInventorySlots();
        }
    }

    public void AddItemToInventory(InventoryItem item)
    {
        foreach (InventorySlot slot in Inventory)
        {
            List<InventoryItem> tempList = slot.GetList();
            if (tempList.Count == 0)
            {
                slot.AddItem(item);
                slot.IncreaseItemCount();
                break;
            } if (tempList.Count > 0)
            {
                if (tempList[1].getName().Equals(item.name))
                {
                    slot.IncreaseItemCount();
                    break;
                }
            }
            if (slot.GetSlotNumber().Equals(10))
            {
                break;
            }
        }
    }

     public void RemoveItemFromInventory(InventoryItem item)
    {
        foreach (InventorySlot slot in Inventory)
        {
            List<InventoryItem> tempList = slot.GetList();
            if (tempList.Count == 0)
            {
                break;
            } if (tempList.Count > 0)
            {
                if (tempList[1].getName().Equals(item.name))
                {
                    slot.DecreaseItemCount();
                    if (slot.GetItemCount() == 0)
                    {
                        slot.DeleteItem(item);
                    }
                    break;
                }
            }
            if (slot.GetSlotNumber().Equals(10))
            {
                break;
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeInventorySlots()
    {
        for (int i = 1; i <= InventorySlots; i++)
        {
            InventorySlot InventorySlot = new InventorySlot(i, SlotsMaxCount);
            Inventory.Add(InventorySlot);
            Debug.Log("Inventory slot created, number: " + i);
        }
    }

    void UpdateInventoryUI()
    {
        
    }
}
