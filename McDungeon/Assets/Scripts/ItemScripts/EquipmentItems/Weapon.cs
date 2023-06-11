using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : EquipmentItem
{
    public int damage = 0;
    public int range = 1;
    public float attackSpeed = 1.5f;
    public Weapon(string name, string itemType, int sellCost, Sprite inventoryIcon, 
        int damage, int range, float attackSpeed) : base(
            name: name,
            sellCost: sellCost,
            itemType: itemType
        )
    {
        this.inventoryIcon = inventoryIcon;
        this.damage = damage;
        this.range = range;
        this.attackSpeed = attackSpeed;
        // Set item description based on stats. 
        string itemDescription = "Damage: " + damage;
        itemDescription += "\n";
        itemDescription += "Attack Speed: " + attackSpeed;
        itemDescription += "\n";
        itemDescription += "Range: " + range;
        itemDescription += "\nSellCost: " + sellCost;

        this.itemDescription = itemDescription; 
    }
}