using UnityEngine;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance  { get; private set; }
    [SerializeField] public float InventorySpace { get; set; }
    [SerializeField] public List<InventorySlot> Inventory { get; set; }

    void Awake() {
        if (Instance == null) {
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InventorySpace = 10;
            Inventory = new List<InventorySlot>(10);
            InitializeInventorySlots();
        }
    }

    public void addItemToInventory(InventoryItem item)
    {
        foreach (InventorySlot slot in Inventory)
        {
            List<InventoryItem> tempList = slot.GetList();
            if (tempList.Count == 0)
            {
                slot.AddItem(item);
                slot.IncreaseItemCount();
            } if (tempList.Count > 0)
            {
                if (tempList[1].getName().Equals(item.name))
                {
                    slot.AddItem(item);
                }
            }
        }
        {
            
        }
        {
            
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
        for (int i = 1; i <= 10; i++)
        {
            InventorySlot InventorySlot = new InventorySlot(i, 10);
            Inventory.Add(InventorySlot);
            Debug.Log("Inventory slot created, number: " + i);
        }
    }
}
