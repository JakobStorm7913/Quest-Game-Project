using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }
    //[SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private InventorySlotUI[] slotUIs;

     private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Refresh();
    }
    private void OnEnable()
    {
       // if (inventoryManager == null)
         //   inventoryManager = InventoryManager.Instance;

        //InventoryManager.Instance.OnInventoryChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        //if (inventoryManager != null)
           //InventoryManager.Instance.OnInventoryChanged -= Refresh;
    }

    public void Refresh()
    {
        var slots = InventoryManager.Instance.slots;

        for (int i = 0; i < slotUIs.Length; i++)
        {
            var ui = slotUIs[i];

            if (i >= slots.Count)
            {
                ClearSlot(ui);
                continue;
            }

            var slot = slots[i];

            if (slot.IsEmpty)
            {
                ClearSlot(ui);
            }
            else
            {
                ui.iconImage.enabled = true;
                ui.iconImage.sprite = slot.item.icon;

                if (slot.count > 1)
                {
                    ui.countText.gameObject.SetActive(true);
                    ui.countText.text = slot.count.ToString() + "x";
                }
                else
                {
                    ui.countText.gameObject.SetActive(false);
                }
            }
        }
    }

    private void ClearSlot(InventorySlotUI ui)
    {
        ui.iconImage.enabled = false;
        ui.iconImage.sprite = null;
        ui.countText.gameObject.SetActive(false);
    }
}