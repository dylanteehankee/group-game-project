using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameItem
{
    private string? itemID = null;

    private string name;
    public string itemType = "Item type";
    public string itemDescription = "Some spicy item description";

    private ItemStatus status;

    private float timeAcquired;

    private float timeCreated;

    public Sprite inventoryIcon;

    public GameItem(string name)
    {
        this.name = name;
        status = ItemStatus.Unowned;
        itemID = ItemManager.AddItemToGame(this); 
        // When gameItem is created, register it with the ItemManager
    }

    public ItemStatus GetStatus()
    {
        return status;
    }

    public Sprite GetInventoryIcon()
    {
        return inventoryIcon;
    }

    public virtual bool CanChangeStatus(ItemStatus toStatus){
        return true;
    }

    public void ChangeStatus(ItemStatus toStatus)
    {
        if(this.CanChangeStatus(toStatus))
            this.status = toStatus;
    }

    public string GetItemID()
    {
        return itemID;
    }

    public string GetName()
    {
        return name;
    }

}