using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMerchantController : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject shopInfoUI;
    private ShopUIManager shopUIManager;
    private ShopCrateController[] crates;

// For resting puprose;s
    public Sprite myIcon;

    private List<string> itemsToSell;
    private List<int> itemPrices;
    // Start is called before the first frame update
    void Start()
    {
        shopInfoUI = GameObject.Find("ShopRoomUI");
        shopUIManager = shopInfoUI.GetComponent<ShopUIManager>();
        gameManager = GameObject.Find("GameManager");

        itemsToSell = new List<string>();
        itemPrices = new List<int>();
        DummyHealthPotion toAdd = new DummyHealthPotion(myIcon, "Stealth Potion");
        ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
        itemsToSell.Add(toAdd.GetItemID());
        itemPrices.Add(10);

        toAdd = new DummyHealthPotion(myIcon, "Wealth Potion");
        ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
        itemsToSell.Add(toAdd.GetItemID());
        itemPrices.Add(20);

        crates = GetComponentsInChildren<ShopCrateController>();
        RestockItems();
    }

    public void BuyItem(int listID)
    {
        UIManager coinManager = gameManager.GetComponent<UIManager>();
        if(coinManager.coinAmount >= itemPrices[listID])
        {
            string itemID = itemsToSell[listID];
            coinManager.coinAmount -= itemPrices[listID];
            ItemManager.ChangeItemStatus(itemID, ItemStatus.EquipmentInventory);
            itemsToSell.RemoveAt(listID);
            itemPrices.RemoveAt(listID);
            RestockItems();
            // Remove previous 3 lines and set to null if you don't want items to move among crates. 
            shopUIManager.CloseShop();
            
            gameManager.GetComponent<InventoryController>().ShopItemAdded();
        }
    }

    public void RestockItems()
    {
        Debug.Log("loading items " + itemsToSell.Count);
        for(int i = 0 ; i < crates.Length; i++)
        {
            if(i < itemsToSell.Count)
            {
                crates[i].LoadShopItem(ItemManager.GetGameItem(itemsToSell[i]), this, i);
            }
            else
            {
                crates[i].ClearSlot();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            RestockItems();
        }
    }

    public void SelectItem(int id)
    {  
        Debug.Log("You selected this");
        shopUIManager.LoadShopItem(this, itemPrices[id], ItemManager.GetGameItem(itemsToSell[id]), id);

    }

    public void UnselectItem()
    {
        Debug.Log("You unselected this");
        shopUIManager.CloseShop();
    }
}
