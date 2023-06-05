using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class PlayerInventory : ItemCollection
{

    private string armor = null;

    private string weapon = null;

    public string GetArmor()
    {
        return armor;
    }

    public string GetWeapon()
    {
        return weapon; 
    }

    public void AddItem(string itemID)
    {
        if(!CanAddItem(itemID))
        {
            return;
        }
        if(weapon == null)
        {
            weapon = itemID;
        }
        else if(armor == null)
        {
            armor = itemID;
        }
    }

    public void RemoveItem(string itemID)
    { 
        if(!CanRemoveItem(itemID))
        {
            return;
        }
        if(weapon == itemID)
        {
            weapon = null;
        }
        else if(armor == itemID)
        {
            armor = null;
        }
    }

    public bool CanAddItem(string itemID)
    {
        if(weapon != null && armor != null)
        {
            return false; 
        }

        return true;
    }

    public bool CanRemoveItem(string itemID)
    {
        if(weapon == itemID || armor == itemID)
        {
            return true; 
        }
        
        return false;
    }
}
