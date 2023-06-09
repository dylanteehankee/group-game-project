using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantController : MonoBehaviour
{
    private bool shopActive = false;

    private List<GameItem> itemsToSell;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            shopActive = true; 
            OpenShop();
        }
    }
     void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            shopActive = false; 
            CloseShop();
        }
    }

    public void OpenShop()
    {
        
        itemsToSell = new List<GameItem>();
        HealthPotion toAdd = new HealthPotion(Resources.Load<Sprite>("Sprites/Health"), "Stealth Potion");
        ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
        itemsToSell.Add(toAdd);

        toAdd = new HealthPotion(Resources.Load<Sprite>("Sprites/Health"), "Wealth Potion");
        ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Unowned);
        itemsToSell.Add(toAdd);
    }

    public void CloseShop()
    {

    }
}
