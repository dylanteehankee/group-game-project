using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    public Sprite icon;
    public GameItem item;
    public GameObject iconSlot; 
    public GameObject buyButton;
    public MerchantController mc;
    private int seqID;
    public GameObject text_ItemName;
    public GameObject text_ItemType;
    public GameObject text_ItemDescription;

    void Start()
    {   
 
    }

    public void LoadShopItem(GameItem toLoad, MerchantController mc, int id)
    {
        seqID = id;
        item = toLoad;
        icon = toLoad.GetInventoryIcon();
        this.mc = mc;
        iconSlot.GetComponent<Image>().enabled = true;
        iconSlot.GetComponent<Image>().sprite = icon;
        buyButton.SetActive(true);

        text_ItemName.GetComponent<Text>().text = item.GetName();
        text_ItemType.GetComponent<Text>().text = item.itemType;
        text_ItemDescription.GetComponent<Text>().text = item.itemDescription;
    }
    public void ClearSlot()
    {
        item = null;
        icon = null;
        iconSlot.GetComponent<Image>().enabled = false;
        iconSlot.GetComponent<Image>().sprite = null;
        buyButton.SetActive(false);

        text_ItemName.GetComponent<Text>().text = "";
        text_ItemType.GetComponent<Text>().text = "";
        text_ItemDescription.GetComponent<Text>().text = "";
    }

    public void OnBuyClick()
    {
        bool canAfford = mc.BuyItem(seqID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
