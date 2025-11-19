using UnityEngine;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour
{
    //public static InventorySlot Instance  { get; private set; }
    [SerializeField] public float slotNumber;
    [SerializeField] private int slotCapacity;
    [SerializeField] private int itemCount;
    [SerializeField] private List<InventoryItem> SlotItems;

    void Awake() {
      /*  if (Instance == null) {
            
            Instance = this;
            DontDestroyOnLoad(gameObject);*/

           // ItemSlot = new List<InventoryItem>(1);
        }
    public InventorySlot(int number, int capacity)
    {
        slotNumber = number;
        slotCapacity = capacity;
        itemCount = 0;
        SlotItems = new List<InventoryItem>(1);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<InventoryItem> GetList()
    {
        return SlotItems;
    }

    public void AddItem(InventoryItem item)
    {
        SlotItems.Add(item);
    }
    public int GetItemCount()
    {
        return itemCount;
    }
    public void IncreaseItemCount()
    {
        itemCount++;
    }

    public void DecreaseItemCount()
    {
        itemCount--;
    }

    public int GetSlotCapacity()
    {
        return slotCapacity;
    }
}
