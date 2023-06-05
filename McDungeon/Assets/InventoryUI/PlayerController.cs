using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
public class PlayerController : MonoBehaviour
{
/*
    public GameObject equipmentUICanvas;
    InventoryUI equipmentUI;
    NonStackableInventoryCollection equipment; 
    // Start is called before the first frame update
    int pageNum = 1;

    private bool inventoryOpen = true;

    void Start()
    {
        equipmentUI = equipmentUICanvas.GetComponent<InventoryUI>();
        equipment = new NonStackableInventoryCollection();
        ItemManager.RegisterItemCollectionWithStatus(ItemStatus.EquipmentInventory, equipment);
    }

    // Update is called once per frame

    public void NextPage()
    {
        pageNum++;
        equipmentUI.UpdateUI(equipment.GetPage("NameForwards", pageNum, 15));
    }

    public void PrevPage()
    {
        pageNum = System.Math.Max(pageNum - 1, 1);
        equipmentUI.UpdateUI(equipment.GetPage("NameForwards", pageNum, 15));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(!inventoryOpen)
            {
                inventoryOpen = true;
                pageNum = 1;
            }
            else
            {
                inventoryOpen = false;
            }
            equipmentUICanvas.SetActive(inventoryOpen);
        }
        if(inventoryOpen){
            if(Input.GetKeyDown(KeyCode.B))
            {
                HealthPotion toAdd = new HealthPotion(Resources.Load<Sprite>("Sprites/Health"));
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.EquipmentInventory);
                equipmentUI.UpdateUI(equipment.GetPage("NameForwards", pageNum, 15));
            }

            if(Input.GetKeyDown(KeyCode.C))
            {
                HealthPotion toAdd = new HealthPotion(Resources.Load<Sprite>("Sprites/Fire Rate"));
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.EquipmentInventory);
                equipmentUI.UpdateUI(equipment.GetPage("NameForwards", pageNum, 15));
            }

            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.NextPage();
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.PrevPage();
            }
        }
    }
    */
}
