using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : EquipmentItem
{
    public float damage = 0;
    public int range = 1;
    public float attackSpeed = 1.5f;
    public float knockBack = 8f;
    public float attackAngle = 120f;
    public int weaponSpriteID;
    public Weapon(string name, string itemType, int sellCost, Sprite inventoryIcon, 
        float damage, int range, float attackSpeed, float knockBack, int weaponSpriteID, float attackAngle = 120f) : base(
            name: name,
            sellCost: sellCost,
            itemType: itemType
        )
    {
        this.inventoryIcon = inventoryIcon;
        this.damage = damage;
        this.range = range;
        this.attackSpeed = attackSpeed;
        this.knockBack = knockBack;
        this.attackAngle = attackAngle;
        this.weaponSpriteID = weaponSpriteID;
        // Set item description based on stats. 
        string itemDescription = "Damage: " + damage;
        itemDescription += "\n";
        itemDescription += "Attack Speed: " + attackSpeed;
        itemDescription += "\n";
        itemDescription += "Range: " + range;
        itemDescription += "\n";
        itemDescription += "Knockback: " + knockBack;
        itemDescription += "\nSell: +" + sellCost;

        this.itemDescription = itemDescription; 
    }
}