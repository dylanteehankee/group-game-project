using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCrateController : MonoBehaviour
{
    public GameObject itemPlace;
    public GameItem myItem;

    private Sprite icon;
    
    private ShopMerchantController mc;

    private int seqID;

    void Start()
    {   
 
    }

    public void LoadShopItem(GameItem toLoad, ShopMerchantController mc, int id)
    {
        seqID = id;
        myItem = toLoad;
        icon = toLoad.GetInventoryIcon();
        this.mc = mc;

        itemPlace.GetComponent<SpriteRenderer>().enabled = true;
        itemPlace.GetComponent<SpriteRenderer>().sprite = icon;
    }
    public void ClearSlot()
    {
        myItem = null;
        icon = null;
        itemPlace.GetComponent<SpriteRenderer>().enabled = false;
        itemPlace.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(myItem != null && collision.tag == "Player")
        {
            mc.SelectItem(seqID);
        }
    }
    
    public void OnTriggerExit2D(Collider2D collision)
    {
         if(myItem != null && collision.tag == "Player")
        {
            mc.UnselectItem();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
