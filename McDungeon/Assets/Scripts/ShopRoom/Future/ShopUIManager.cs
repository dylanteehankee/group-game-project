using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    public GameObject shopCanvas;
    
    public GameObject iconSlot; 
    public GameObject buyButton;
    public GameObject text_ItemName;
    public GameObject text_ItemType;
    public GameObject text_ItemDescription;
    public GameObject text_ItemPrice;


    public Sprite icon;
    public GameItem item;
    public ShopMerchantController mc;
    private int seqID;
    

    void Start()
    {   
        shopCanvas.SetActive(false);
    }

    public void LoadShopItem(ShopMerchantController mc, int price, GameItem item, int id)
    {
        this.mc = mc;
        this.item = item;
        this.seqID = id;
        this.icon = item.GetInventoryIcon();

        shopCanvas.SetActive(true);

        iconSlot.GetComponent<Image>().enabled = true;
        iconSlot.GetComponent<Image>().sprite = icon;

        text_ItemName.GetComponent<Text>().text = item.GetName();
        text_ItemType.GetComponent<Text>().text = item.itemType;
        text_ItemDescription.GetComponent<Text>().text = item.itemDescription;
        text_ItemPrice.GetComponent<Text>().text = "Price: " + price.ToString();
    }

    public void OnBuyButtonClick()
    {
        mc.BuyItem(seqID);
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
