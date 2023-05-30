using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using System;

public class PlayerCardUI : MonoBehaviour
{
    public InventorySlot weaponSlot;
    public InventorySlot armorSlot;

    // Add List<int> selectedIndices
    public void LoadItems(string weapon, string armor, string itemSelected, bool isHovered, Action<InventorySlot> onClick, Action<InventorySlot, bool> onHover)
    {
        if(weapon != null)
        {
            weaponSlot.AddItem(ItemManager.GetGameItem(weapon), onClick, onHover);
        }
        else
        {
            weaponSlot.ClearSlot();
        }

        if(armor != null)
        {
            armorSlot.AddItem(ItemManager.GetGameItem(armor), onClick, onHover);
        }
        else
        {
            armorSlot.ClearSlot();
        }
        switch(itemSelected)
        {
            case "weapon": 
                if(isHovered)
                {
                    weaponSlot.SetHoveredSlot();
                }
                else
                {
                    weaponSlot.SetSelectedSlot();
                }
                
                break;
            case "armor":
                if(isHovered)
                {
                    armorSlot.SetHoveredSlot();
                }
                else
                {
                    armorSlot.SetSelectedSlot();
                }
                break;
            default:
                break;
        }
        
    }

}
