using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using System;
//https://www.youtube.com/watch?v=YLhj7SfaxSE
public class InventoryGridUI : MonoBehaviour
{
    public Transform itemsParent;

    private InventorySlot[] slots;

    public GameObject nextPageButton;
    public GameObject prevPageButton;

    // Start is called before the first frame update
    void Start()
    {   
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // public void LoadItems() for 

    public void LoadItems(List<string> items, List<int> equippedIndices, List<int> selectedIndices, 
        bool isHovered, Action<InventorySlot> onClickDelegate, Action<InventorySlot, bool> onHoverDelegate)
    {
        TogglePagesButton(false, false);
        for(int i = 0 ; i < slots.Length; i++)
        {
            if(i < items.Count)
            {
                slots[i].AddItem(ItemManager.GetGameItem(items[i]), onClickDelegate, onHoverDelegate);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
        for(int i = 0 ; i < equippedIndices.Count ; i++)
        {
            int slotToChange = equippedIndices[i];
            slots[slotToChange].SetEquippedSlot();
        }
        for(int i = 0 ; i < selectedIndices.Count ; i++)
        {
            int slotToChange = selectedIndices[i];
            if(isHovered)
            {
                slots[slotToChange].SetHoveredSlot();
            }
            else
            {
                slots[slotToChange].SetSelectedSlot();
            }
            
        }
    }

    public int GetNumInGrid()
    {
        return slots.Length;
    }

    // Load items for stack of items. 
    public void LoadItems(List<KeyValuePair<string, List<string>>> items, List<int> equippedIndices, 
        List<int> selectedIndices, bool isHovered, Action<InventorySlot> onClickDelegate, Action<InventorySlot, bool> onHoverDelegate)
    {
        TogglePagesButton(false, true);
        for(int i = 0 ; i < slots.Length; i++)
        {
            if(i < items.Count)
            {
                slots[i].AddItemStack(items[i].Value, onClickDelegate, onHoverDelegate);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
        for(int i = 0 ; i < equippedIndices.Count ; i++)
        {
            int slotToChange = equippedIndices[i];
            slots[slotToChange].SetEquippedSlot();
        }
        for(int i = 0 ; i < selectedIndices.Count ; i++)
        {
            int slotToChange = selectedIndices[i];
            if(isHovered)
            {
                slots[slotToChange].SetHoveredSlot();
            }
            else
            {
                slots[slotToChange].SetSelectedSlot();
            }
            
        }
    }

    public void TogglePagesButton(bool showPrev, bool showNext)
    {
        if(showPrev == true)
        {
            prevPageButton.SetActive(true);
        }
        else
        {
            prevPageButton.SetActive(false);
        }

        if(showNext == true)
        {
            nextPageButton.SetActive(true);
        }
        else
        {
            nextPageButton.SetActive(false);
        }
    }

}
