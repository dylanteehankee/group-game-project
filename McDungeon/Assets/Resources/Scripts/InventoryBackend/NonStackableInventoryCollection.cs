using System.Collections;
using System.Collections.Generic;
using InventoryComparators;
using UnityEngine;

namespace Inventory
{
    public class NonStackableInventoryCollection : ItemCollection 
    {
        private Dictionary<string, SortedList<string, string>> sortedItems;
        private SortedList<string, string> allItems;

        public NonStackableInventoryCollection()
        {
            sortedItems = new Dictionary<string, SortedList<string, string>>(); // key is the name of sorting method, value is sorted names
            allItems = new SortedList<string, string>(); // key is the name, value is the list of IDs of that gameobject name
            this.RegisterSortMethod("NameForwards", new NSIC_CompareItemNameForwards());
            this.RegisterSortMethod("NameBackwards", new NSIC_CompareItemNameBackwards());
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
            allItems.Add(itemID, itemID);
            foreach(KeyValuePair<string, SortedList<string, string>> sortMethod in sortedItems)
            {
                sortMethod.Value.Add(itemID, itemID);
            }
        }
        public void RemoveItem(string itemID)
        { 
            if(!CanRemoveItem(itemID))
            {
                return;
            }
            allItems.Remove(itemID);
            foreach(KeyValuePair<string, SortedList<string, string>> sortMethod in sortedItems)
            {
                sortMethod.Value.Remove(itemID);
            }
        }
        public bool CanAddItem(string itemID)
        {
            if(ItemManager.GetGameItem(itemID) is GameItem == false)
            {
                return false;
            }
            if(allItems.ContainsKey(itemID))
            {
                return true;
            }
            // Could also check capacity, etc.
            return true;
        }

        public bool CanRemoveItem(string itemID)
        {
            if(!allItems.ContainsKey(itemID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<string> GetPage(string sortMethod, int pageNum, int numPerPage)
        {
            if(!sortedItems.ContainsKey(sortMethod))
            {
                return null;
            }
            SortedList<string, string> chosenSortedItems = this.sortedItems[sortMethod];
            List<string> toReturn = new List<string>();
            int startIndex = (pageNum - 1) * numPerPage;
            for(int i = 0 ; i < numPerPage && (i + startIndex) < chosenSortedItems.Count ; i++)
            {
                toReturn.Add(chosenSortedItems.Keys[i + startIndex]);
            }

            return toReturn;
        }

    }
}