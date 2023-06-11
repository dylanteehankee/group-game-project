using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ConsumableItem : GameItem
{
    private int sellCost = 0;
    public ConsumableItem(string name, int sellCost, string itemType, string itemDescription) : base(name)
    {
        this.sellCost = sellCost;
        this.itemType = itemType;
        this.itemDescription = itemDescription;
    }
    public override bool CanChangeStatus(ItemStatus ic)
    {
        if(ic == ItemStatus.EquipmentInventory || ic == ItemStatus.Equipped)
            return false;
        else
            return true;
    }
    public abstract void UseItem(PlayerController playerController);

}