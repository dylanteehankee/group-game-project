using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryComparators
{
    class NSIC_CompareItemNameForwards : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if(x.Equals(y)) 
            {
                return 0; //same item
            }

            GameItem xItem = ItemManager.GetGameItem(x);
            GameItem yItem = ItemManager.GetGameItem(y);
            int compareValue = yItem.GetName().CompareTo(xItem.GetName());
            if(compareValue < 0) 
                return 1;
            else if(compareValue == 0)
            {
                int compareItemID = Int32.Parse(y) - Int32.Parse(x);
                if(compareItemID > 0)
                    return 1;
                else
                    return -1;
            }
            else
                return -1;
        }
    }

    class NSIC_CompareItemNameBackwards : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if(x.Equals(y)) 
            {
                return 0; //same item
            }

            GameItem xItem = ItemManager.GetGameItem(x);
            GameItem yItem = ItemManager.GetGameItem(y);
            int compareValue = yItem.GetName().CompareTo(xItem.GetName());
            if(compareValue > 0) 
                return 1;
            else if(compareValue == 0)
            {
                int compareItemID = Int32.Parse(y) - Int32.Parse(x);
                if(compareItemID > 0)
                    return 1;
                else
                    return -1;
            }
            else
                return -1;
        }
    }
     class SIC_CompareItemNameForwards : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int compareValue = y.CompareTo(x);
            if(compareValue < 0) 
                return 1;
            else if(compareValue == 0)
                return 0;
            else
                return -1;
        }
    }

    class SIC_CompareItemNameBackwards : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int compareValue = y.CompareTo(x);
            if(compareValue > 0) 
                return 1;
            else if(compareValue == 0)
                return 0;
            else
                return -1;
        }
    }

    class ICompareItemDate : IComparer<GameItem>
    {
        public int Compare(GameItem x, GameItem y)
        {
            return 0;
        }
    }
}