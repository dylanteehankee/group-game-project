using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Armor : EquipmentItem
{
    public int bonusHP = 0;
    public Armor(string name, string itemType, int sellCost, Sprite inventoryIcon) : base(
            name: name,
            sellCost: sellCost,
            itemType: itemType
        )
    {
        this.inventoryIcon = inventoryIcon;
        // Set item description based on stats. 
        string itemDescription = "";
        itemDescription += "\n";
        this.itemDescription = "";
    }
}