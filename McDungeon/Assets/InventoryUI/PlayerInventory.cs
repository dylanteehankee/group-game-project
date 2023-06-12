using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using McDungeon;

public class PlayerInventory : ItemCollection
{

    private PlayerController playerController;
    public PlayerInventory(PlayerController pc)
    {
        this.playerController = pc;
    }
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

    public Weapon GetWeaponItem()
    {
        if(weapon == null)
            return null;
        return (Weapon)(ItemManager.GetGameItem(weapon));
    }

    public Armor GetArmorItem()
    {
        if(armor == null)
            return null;
        return (Armor)(ItemManager.GetGameItem(armor));
    }

    public void AddItem(string itemID)
    {
        if(!CanAddItem(itemID))
        {
            return;
        }
        GameItem g = ItemManager.GetGameItem(itemID);
        if(g is Weapon)
        {
            //if(weapon == null)
            {
                if(weapon != null)
                    ItemManager.ChangeItemStatus(weapon, ItemStatus.EquipmentInventory);
                weapon = itemID;
                playerController.SyncWeaponWithInventory();
            }
        }
        else if(g is Armor)
        {
            //if(armor == null)
            {
                  armor = itemID;
            }
          
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
            playerController.SyncWeaponWithInventory();
        }
        else if(armor == itemID)
        {
            armor = null;
        }
    }

    public bool CanAddItem(string itemID)
    {
        GameItem g = ItemManager.GetGameItem(itemID);
        if(g is Weapon)
        {
            /*
            if(weapon == null)
            {
                return true; 
            }
            else
            {
                return false;
            }
            */
            return true;
        }
        else if(g is Armor)
        {
            /*
            if(armor == null)
            {
                return true; 
            }
            else
            {
                return false;
            }
            */
            return true;
        }
        else
        {
            return false;
        }
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
