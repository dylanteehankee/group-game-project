using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EquipmentItem : GameItem
{
    public int sellCost = 0;
    public EquipmentItem(string name, int sellCost, string itemType) : base(name)
    {
        this.sellCost = sellCost;
        this.itemType = itemType;
    }
    public override bool CanChangeStatus(ItemStatus ic)
    {
        if(ic == ItemStatus.ItemInventory)
            return false;
        else
            return true;
    }
}