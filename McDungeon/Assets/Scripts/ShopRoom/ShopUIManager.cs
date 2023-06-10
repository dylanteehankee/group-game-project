using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    ShopSlotUI[] slots;
    public GameObject shopParent; 
    public GameObject shopCanvas;

    void Start()
    {   
        slots = shopParent.GetComponentsInChildren<ShopSlotUI>();
        shopCanvas.SetActive(false);
    }

    public void LoadItems(MerchantController mc, List<string> items)
    {
        shopCanvas.SetActive(true);
        Debug.Log("loading items " + items.Count);
        for(int i = 0 ; i < slots.Length; i++)
        {
            if(i < items.Count)
            {
                slots[i].LoadShopItem(ItemManager.GetGameItem(items[i]), mc, i);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void CloseShop()
    {
        shopCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
