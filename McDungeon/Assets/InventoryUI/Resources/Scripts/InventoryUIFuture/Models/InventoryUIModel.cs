using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryUIModel
{
    public int inventoryPageNum;
    public bool isActive;
    public ItemCollectionName openInventoryCollection;
    public string openInventorySortMethod;

    public List<string> selectedGameItemIDs;
    public string selectedGameItemName;
    public ItemCollectionName? selectedGameItemInventory;

    // Differentiates between being hovered over vs being selected
    public bool isHovered;


    public InventoryUIModel()
    {
        ResetUIState();
    }

    public void ClearSelectedState()
    {
        selectedGameItemIDs = null;
        selectedGameItemInventory = null;
        selectedGameItemName = null;
        isHovered = false;
    }

    public void ResetUIState()
    {
        inventoryPageNum = 1;
        isActive = false;
        openInventoryCollection = ItemCollectionName.EquipmentItems;
        openInventorySortMethod = "NameForwards";

        ClearSelectedState();
    }
}
