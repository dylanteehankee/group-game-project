using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : EquipmentItem
{
    public int damage = 0;
    public int range = 1;
    public float attackSpeed = 1.5f;
    public Weapon(string name, string itemType, int sellCost, Sprite inventoryIcon) : base(
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