using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantController : MonoBehaviour
{
    private bool shopActive = false;

    public GameObject shopUI;

    public Sprite myIcon;

    private ShopUIManager shopUIManager;

    private List<string> itemsToSell;
    // Start is called before the first frame update
    void Start()
    {
        shopUI = GameObject.Find("ShopRoomUI");
        shopUIManager = shopUI.GetComponent<ShopUIManager>();
        itemsToSell = new List<string>();
        HealthPotion toAdd = new HealthPotion(myIcon, "Stealth Potion");
        ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
        itemsToSell.Add(toAdd.GetItemID());

        toAdd = new HealthPotion(myIcon, "Wealth Potion");
        ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
        itemsToSell.Add(toAdd.GetItemID());

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(!shopActive)
                OpenShop();
            else   
                CloseShop();
        }
        */
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            OpenShop();
        }
    }
     void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CloseShop();
        }
    }

    public bool BuyItem(int listID)
    {
        if(true)
        {
            string itemID = itemsToSell[listID];
            ItemManager.ChangeItemStatus(itemID, ItemStatus.EquipmentInventory);
            itemsToSell.RemoveAt(listID);
        }
        shopUIManager.LoadItems(this, itemsToSell);
        //shopUIManager.LoadItems(this, new List<string>());
        return true;
    }
    public void OpenShop()
    {  
        shopActive = true; 
        shopUIManager.LoadItems(this, itemsToSell);
    }

    public void CloseShop()
    {
        shopActive = false; 
        shopUIManager.CloseShop();
    }
}
