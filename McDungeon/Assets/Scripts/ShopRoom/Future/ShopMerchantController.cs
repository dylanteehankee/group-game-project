using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMerchantController : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject shopInfoUI;
    private ShopUIManager shopUIManager;
    private ShopCrateController[] crates;

    // For resting puproses
    public Sprite myIcon;

    [SerializeField] public int shopID;
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
        GameItem toAdd;
        switch(shopID)
        {
            case 0:
                toAdd = new DummyHealthPotion(myIcon, "Stealth Potion");
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
                itemsToSell.Add(toAdd.GetItemID());
                itemPrices.Add(10);

                toAdd = new DummyHealthPotion(myIcon, "Wealth Potion");
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
                itemsToSell.Add(toAdd.GetItemID());
                itemPrices.Add(20);
                break;
            case 1:
                toAdd = new Weapon(
                    name: "King's Sword",
                    itemType: "Weapon",
                    sellCost: 80,
                    inventoryIcon: myIcon,
                    damage: 5,
                    range: 2,
                    attackSpeed: 0.8f
                );
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
                itemsToSell.Add(toAdd.GetItemID());
                itemPrices.Add(100);

                toAdd = new Weapon(
                    name: "Madman's Dagger",
                    itemType: "Weapon",
                    sellCost: 100,
                    inventoryIcon: myIcon,
                    damage: 2,
                    range: 1,
                    attackSpeed: 0.2f
                );
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
                itemsToSell.Add(toAdd.GetItemID());
                itemPrices.Add(150);

                toAdd = new Weapon(
                    name: "Heavy greatsword",
                    itemType: "Weapon",
                    sellCost: 30,
                    inventoryIcon: myIcon,
                    damage: 5,
                    range: 2,
                    attackSpeed: 1.5f
                );
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
                itemsToSell.Add(toAdd.GetItemID());
                itemPrices.Add(60);
                break;
            default:
                break;
        }
       

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
            /*
            itemsToSell.RemoveAt(listID);
            itemPrices.RemoveAt(listID);
            RestockItems();
            */
            // Remove previous 3 lines and set to null if you don't want items to move among crates. 
            itemsToSell[listID] = null;
            itemPrices[listID] = 0;
            RestockItems();
            // Remove previous 3 lines and restore previous code for items to move among crates. 
            
            shopUIManager.CloseShop();
            
            gameManager.GetComponent<InventoryController>().ShopItemAdded();
        }
    }

    public void RestockItems()
    {
        for(int i = 0 ; i < crates.Length; i++)
        {
            if(i < itemsToSell.Count && itemsToSell[i] != null)
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
        shopUIManager.LoadShopItem(this, itemPrices[id], ItemManager.GetGameItem(itemsToSell[id]), id);

    }

    public void UnselectItem()
    {
        shopUIManager.CloseShop();
    }
}
