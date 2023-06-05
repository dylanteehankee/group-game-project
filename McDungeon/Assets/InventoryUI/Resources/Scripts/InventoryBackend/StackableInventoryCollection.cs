using System.Collections;
using System.Collections.Generic;
using InventoryComparators;

namespace Inventory
{
    public class StackableInventoryCollection : ItemCollection 
    {
        private Dictionary<string, SortedList<string, string>> sortedItems;
        private SortedList<string, List<string>> allItems;

        public StackableInventoryCollection()
        {
            sortedItems = new Dictionary<string, SortedList<string, string>>(); // key is the name of sorting method, value is sorted names
            allItems = new SortedList<string, List<string>>(); // key is the name, value is the list of IDs of that gameobject name
            this.RegisterSortMethod("NameForwards", new SIC_CompareItemNameForwards());
            this.RegisterSortMethod("NameBackwards", new SIC_CompareItemNameBackwards());
        }

        public bool RegisterSortMethod(string sortMethod, IComparer<string> comparer)
        {
            if(allItems.Count == 0 && !sortedItems.ContainsKey(sortMethod))
            {
                sortedItems.Add(sortMethod, new SortedList<string, string>(comparer));
                return true;
            }
            return false;
        }

        public void AddItem(string itemID)
        {
            if(!CanAddItem(itemID))
            {
                return;
            }
            GameItem toAdd = ItemManager.GetGameItem(itemID);
            string itemNameKey = toAdd.GetName();
            if(!allItems.ContainsKey(itemNameKey))
            {
                allItems.Add(itemNameKey, new List<string>());
                foreach(KeyValuePair<string, SortedList<string, string>> sortMethod in sortedItems)
                {
                    sortMethod.Value.Add(itemNameKey, itemNameKey);
                }
            }
            allItems[itemNameKey].Add(itemID);
        }

        // This method is never used. 
        public List<string> GetItemStack(string itemName)
        {
            if(allItems.ContainsKey(itemName))
                return allItems[itemName];
            else   
                return null;
        }

        public void RemoveItem(string itemID)
        { 
            if(!CanRemoveItem(itemID))
            {
                return;
            }
            GameItem toRemove = ItemManager.GetGameItem(itemID);
            string itemNameKey = toRemove.GetName();
            allItems[itemNameKey].Remove(itemID);
            if(allItems[itemNameKey].Count == 0)
            {
                allItems.Remove(itemNameKey);
                foreach(KeyValuePair<string, SortedList<string, string>> sortMethod in sortedItems)
                {
                    sortMethod.Value.Remove(itemNameKey);
                }
            }
        }

        public bool CanAddItem(string itemID)
        {
            if(ItemManager.GetGameItem(itemID) is GameItem == false)
            {
                return false;
            }
            // Could also check stack limit, capacity, etc.
            return true;
        }

        public bool CanRemoveItem(string itemID)
        {
            GameItem toRemove = ItemManager.GetGameItem(itemID);
            string itemNameKey = toRemove.GetName();
            if(!allItems.ContainsKey(itemNameKey))
            {
                return false;
            }
            else
            {
                if(!allItems[itemNameKey].Contains(itemID))
                {
                    return false;
                }
                return true;
            }
            
        }

        public List<KeyValuePair<string, List<string>>> GetPage(string sortMethod, int pageNum, int numPerPage)
        {
            if(!sortedItems.ContainsKey(sortMethod))
            {
                return null;
            }
            SortedList<string, string> chosenSortedItems = this.sortedItems[sortMethod];
            List<KeyValuePair<string, List<string>>> toReturn = new List<KeyValuePair<string, List<string>>>();
            int startIndex = (pageNum - 1) * numPerPage;
            for(int i = startIndex ; i < numPerPage && (i + startIndex) < chosenSortedItems.Count ; i++)
            {
                toReturn.Add(new KeyValuePair<string, List<string>>
                    (chosenSortedItems.Keys[i], allItems[chosenSortedItems.Keys[i]]));
            }

            return toReturn;
        }
        
    }
}