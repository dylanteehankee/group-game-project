using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Inventory;

public class CollectionsModel
{
    public Dictionary<ItemCollectionName, ItemCollection> itemCollections;
    public CollectionsModel()
    {
        itemCollections = new Dictionary<ItemCollectionName, ItemCollection>();
    }

}
public enum ItemCollectionName
{
    EquipmentItems,
    StackableItems,
    PlayerInventory
}