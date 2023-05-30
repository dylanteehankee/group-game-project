using System.Collections;
using System.Collections.Generic;
using Inventory;

public static class ItemManager
{
    private static Dictionary<string, GameItem> gameItems = new Dictionary<string, GameItem>();

    private static Dictionary<ItemStatus, List<ItemCollection>> itemCollections = new Dictionary<ItemStatus, List<ItemCollection>>();

    private static int nextItemID = 0;


    public static void RegisterItemCollectionWithStatus(ItemStatus status, ItemCollection collection)
    {
        if(!itemCollections.ContainsKey(status))
        {
            itemCollections.Add(status, new List<ItemCollection>());
        }
        itemCollections[status].Add(collection);
    }

    /// <summary>
    /// Registers an item into the game where it is assigned an ID
    /// </summary>
    /// <param name="newItem">A GameItem that needs to be registered
    /// </param>
    /// <returns>The ID of the newly registered GameItem, null if unsuccessful</retruns>
    public static string AddItemToGame(GameItem newItem)
    {
        if(newItem.GetItemID() != null)
        {
            // Item has already been created/registered
            return null;
        }
        string newID = GenerateNextItemID();
        gameItems.Add(newID, newItem);
        return newID;
    }

    /// <summary>
    /// Removes an item from the game, only used when it will never appear again
    /// </summary>
    /// <param name="itemID">The GameItem itemID that should be removed
    /// </param>
    /// <returns> If the removal is successful </retruns>
    public static bool RemoveItemFromGame(string itemID)
    {
        if(!gameItems.ContainsKey(itemID))
        {
            // Item could not be found among active gameItems
            return false;
        }
        gameItems.Remove(itemID);
        return true;
    }
    
    public static GameItem GetGameItem(string itemID)
    {
        return gameItems[itemID];
    }

    private static bool CanRemoveItem(string itemID, ItemStatus fromStatus)
    {
        if(itemCollections.ContainsKey(fromStatus))
        {
            // See if adding each item to the collection is feasible. 
            foreach(ItemCollection ic in itemCollections[fromStatus])
            {
                if(!ic.CanRemoveItem(itemID))
                {
                    return false;
                }
            }
            return true;
        }
        else if(fromStatus == ItemStatus.Unowned)
        {
            return true;
        }
        else
        {
            // Each ItemStatus should be tracked, should not happen
            return false;
        }
    }

    private static bool CanAddItem(string itemID, ItemStatus toStatus)
    {
        if(itemCollections.ContainsKey(toStatus))
        {
            // See if adding each item to the collection is feasible. 
            foreach(ItemCollection ic in itemCollections[toStatus])
            {
                if(!ic.CanAddItem(itemID))
                {
                    return false;
                }
            }
            return true;
        }
        else if(toStatus == ItemStatus.Unowned)
        {
            return true;
        }
        else
        {
            // Each ItemStatus should be tracked, should not happen
            return false;
        }
    }

    /// <summary>
    /// Changes the status of an gameItem, moving the gameItme
    /// </summary>
    /// <param name="itemID">The GameItem itemID that should be moved
    /// </param>
    /// <param name="toStatus">The new status of the GameItem
    /// </param>
    /// <returns> If the transfer is successful </retruns>
    public static bool ChangeItemStatus(string itemID, ItemStatus toStatus)
    {
        // Ensure the ItemStatus isn't one the item is already in.
        GameItem toTransfer = gameItems[itemID];
        if(toTransfer.GetStatus() == toStatus)
        {
            return false; 
        }
        //Check if can add and remove safely, then transfer 
        if(CanAddItem(itemID, toStatus) && 
            CanRemoveItem(itemID, toTransfer.GetStatus()) &&
            toTransfer.CanChangeStatus(toStatus))
        {
            if(toTransfer.GetStatus() != ItemStatus.Unowned)
            {
                foreach(ItemCollection ic in itemCollections[toTransfer.GetStatus()])
                {
                    ic.RemoveItem(itemID);
                }
            }
            if(toStatus != ItemStatus.Unowned)
            {
                foreach(ItemCollection ic in itemCollections[toStatus])
                {
                    ic.AddItem(itemID);
                }
            }
            // Update ItemStatus in the item itself
            toTransfer.ChangeStatus(toStatus);
            return true;
        }
        else
        {
            return false;
        }
        
        
    }

    private static string GenerateNextItemID()
    {
        return (nextItemID++).ToString();
    }

}