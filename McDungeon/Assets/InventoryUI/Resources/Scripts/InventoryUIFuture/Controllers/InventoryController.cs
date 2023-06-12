using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using McDungeon;
public class InventoryController : MonoBehaviour
{    
    private CollectionsModel collectionsModel;
    private InventoryUIModel inventoryUIModel;

    public Sprite startingWeaponSprite;

    private InventoryGridUI inventoryGridUI;
    private HoverItemUI hoverItemUI;
    private PlayerCardUI playerCardUI;
    public GameObject uiCanvas;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
        collectionsModel = new CollectionsModel();
        inventoryUIModel = new InventoryUIModel();

        inventoryGridUI = uiCanvas.GetComponent<InventoryGridUI>();
        hoverItemUI = uiCanvas.GetComponent<HoverItemUI>();
        playerCardUI = uiCanvas.GetComponent<PlayerCardUI>();
        
        NonStackableInventoryCollection equipment = new NonStackableInventoryCollection();
        ItemManager.RegisterItemCollectionWithStatus(ItemStatus.EquipmentInventory, equipment);
        ItemManager.RegisterItemCollectionWithStatus(ItemStatus.Equipped, equipment);
        collectionsModel.itemCollections.Add(ItemCollectionName.EquipmentItems, equipment);

        StackableInventoryCollection consumables = new StackableInventoryCollection();
        ItemManager.RegisterItemCollectionWithStatus(ItemStatus.ItemInventory, consumables);
        collectionsModel.itemCollections.Add(ItemCollectionName.StackableItems, consumables);

        PlayerInventory playerInventory = player.GetComponent<PlayerController>().GetPlayerInventory();
        ItemManager.RegisterItemCollectionWithStatus(ItemStatus.Equipped, playerInventory);
        collectionsModel.itemCollections.Add(ItemCollectionName.PlayerInventory, playerInventory);

        // Give Starting Weapon
        CreateStartingWeapon();

        ToggleInventory();
        ToggleInventory();
    }

    public void CreateStartingWeapon()
    {
         Weapon toAdd = new Weapon(
            name: "Rusty Sword",
            itemType: "Weapon",
            sellCost: 5,
            inventoryIcon: startingWeaponSprite,
            damage: 2,
            range: 2,
            attackSpeed: 0.8f
        );
        ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.Equipped);
    }

    public void NextPage()
    {
        inventoryUIModel.inventoryPageNum++;
        inventoryUIModel.ClearSelectedState();   
        RefreshHoverItemUI();
        RefreshInventoryGridUI();
        RefreshPlayerCardUI();
    }

    public void PrevPage()
    {
        if(inventoryUIModel.inventoryPageNum >= 2)
        {
            inventoryUIModel.inventoryPageNum--;
              inventoryUIModel.ClearSelectedState();   
            RefreshHoverItemUI();
            RefreshInventoryGridUI();
            RefreshPlayerCardUI();
        }
        else
        {
            // Instead hide the previous previous button
        }
    }

    private void RefreshHoverItemUI()
    {
        hoverItemUI.SetAllButtonsInactive();
        switch(inventoryUIModel.selectedGameItemInventory)
        {
            case ItemCollectionName.EquipmentItems:
                if(ItemManager.GetGameItem(inventoryUIModel.selectedGameItemIDs[0]).GetStatus() == ItemStatus.Equipped)
                {
                    hoverItemUI.SetButtonsActive(new List<HoverItemButtonNames>{
                        HoverItemButtonNames.UnequipItem,
                        HoverItemButtonNames.SellItem
                    });
                }
                else
                {
                    hoverItemUI.SetButtonsActive(new List<HoverItemButtonNames>{
                        HoverItemButtonNames.EquipItem,
                        HoverItemButtonNames.SellItem
                    });
                }
                hoverItemUI.LoadItemSingular(inventoryUIModel.selectedGameItemIDs[0]);
                break;
            case ItemCollectionName.PlayerInventory:
                hoverItemUI.SetButtonsActive(new List<HoverItemButtonNames>{
                    HoverItemButtonNames.UnequipItem,
                    HoverItemButtonNames.SellItem
                });
                hoverItemUI.LoadItemSingular(inventoryUIModel.selectedGameItemIDs[0]);
                break;
            case ItemCollectionName.StackableItems:
                hoverItemUI.SetButtonsActive(new List<HoverItemButtonNames>{
                    HoverItemButtonNames.UseItem,
                    HoverItemButtonNames.SellItem
                });
                hoverItemUI.LoadItemStack(
                    inventoryUIModel.selectedGameItemName,
                    inventoryUIModel.selectedGameItemIDs
                );
                break;
            case null:
                hoverItemUI.LoadNoItems();
                //Debug.Log("Nothing here for center panel UI");
                break;
            default:
                Debug.Log("Bad unhandled case");
                break;
        }
    }

    private void RefreshPlayerCardUI()
    {
        PlayerInventory playerInventory = (PlayerInventory)(collectionsModel.itemCollections[ItemCollectionName.PlayerInventory]);
        string selectedState = null;
        if(inventoryUIModel.selectedGameItemIDs != null) 
        {   
            if(inventoryUIModel.selectedGameItemIDs[0].Equals(playerInventory.GetWeapon()))
                selectedState = "weapon";
            else if(inventoryUIModel.selectedGameItemIDs[0].Equals(playerInventory.GetArmor()))
                selectedState = "armor";
        }

        playerCardUI.LoadItems(
            playerInventory.GetWeapon(),
            playerInventory.GetArmor(),
            selectedState,
            inventoryUIModel.isHovered,
            this.EquippedInventorySlotSelected,
            this.EquippedInventorySlotHovered
        );
        
        playerCardUI.LoadCoinAmount(gameObject.GetComponent<UIManager>().coinAmount);
        
    }

    private void RefreshInventoryGridUI()
    {
        /*
        if(inventoryUIModel.selectedGameItemIDs == null)
            Debug.Log("Refreshed with none selected");
        else
            Debug.Log("Refreshed with selected " + inventoryUIModel.selectedGameItemIDs);
        */
        switch(inventoryUIModel.openInventoryCollection)
        {
            case ItemCollectionName.EquipmentItems:
                // Retrieve the page of equipment items. 
                List<string> equipmentPageIDs = 
                    ((NonStackableInventoryCollection) 
                        collectionsModel.itemCollections[ItemCollectionName.EquipmentItems]).GetPage(
                            inventoryUIModel.openInventorySortMethod, 
                            inventoryUIModel.inventoryPageNum,
                            inventoryGridUI.GetNumInGrid() //num slots
                        );
                
                // Check if the selected item or equipped item is contained in the page of items.  
                List<int> equippedIndices = new List<int>();
                List<int> selectedIndices = new List<int>();
                for(int i = 0 ; i < equipmentPageIDs.Count ; i++)
                {
                    if(ItemManager.GetGameItem(equipmentPageIDs[i]).GetStatus() == ItemStatus.Equipped)
                    {
                        equippedIndices.Add(i);
                    }
                    if((inventoryUIModel.selectedGameItemInventory == ItemCollectionName.EquipmentItems || 
                        inventoryUIModel.selectedGameItemInventory == ItemCollectionName.PlayerInventory) &&
                        inventoryUIModel.selectedGameItemIDs[0].Equals(equipmentPageIDs[i]))
                    {
                        selectedIndices.Add(i);
                    }
                }
                inventoryGridUI.LoadItems(
                    equipmentPageIDs, 
                    equippedIndices,
                    selectedIndices,
                    inventoryUIModel.isHovered,
                    this.InventorySlotSelected,
                    this.InventorySlotHovered
                );
                
                break;
            case ItemCollectionName.StackableItems:
                // Retrieve the page of stackable items.
                // List of KVP where Key is name of item, Value is list of Item IDs.  
                List<KeyValuePair<string, List<string>>> stackableItemsPage = 
                    ((StackableInventoryCollection) 
                        collectionsModel.itemCollections[ItemCollectionName.StackableItems]).GetPage(
                            inventoryUIModel.openInventorySortMethod, 
                            inventoryUIModel.inventoryPageNum,
                            inventoryGridUI.GetNumInGrid() //num slots
                        );
                // Check if the selected item is contained in the page of items. 
                selectedIndices = new List<int>();
                for(int i = 0 ; i < stackableItemsPage.Count ; i++)
                {
                    if((inventoryUIModel.selectedGameItemInventory == ItemCollectionName.StackableItems) &&
                        inventoryUIModel.selectedGameItemName.Equals(stackableItemsPage[i].Key))
                    {
                        selectedIndices.Add(i);
                    }
                }
                inventoryGridUI.LoadItems(
                    stackableItemsPage, 
                    new List<int>(),
                    selectedIndices,
                    inventoryUIModel.isHovered,
                    this.InventorySlotSelected,
                    this.InventorySlotHovered
                );

                break;
            default:
                Debug.Log("well thats not good");
                break;
        }
    }

    private void ConsumeSelectedItem()
    {
        
        // Consider having ItemManager automatically set state to unowned.
        string idToRemove = inventoryUIModel.selectedGameItemIDs[0];
        ItemManager.ChangeItemStatus(idToRemove, ItemStatus.Unowned);
        ItemManager.RemoveItemFromGame(idToRemove);

        // Reset some states based on where the selected item is sold from. 
        // Then refresh the inventory UI appropriately. 
        switch(inventoryUIModel.selectedGameItemInventory)
        {
            case ItemCollectionName.EquipmentItems:
                inventoryUIModel.ClearSelectedState();
                RefreshInventoryGridUI();
                RefreshHoverItemUI();
                RefreshPlayerCardUI();
                break;
            case ItemCollectionName.PlayerInventory:
                inventoryUIModel.ClearSelectedState();
                RefreshInventoryGridUI();
                RefreshHoverItemUI();
                RefreshPlayerCardUI();
                break;
            case ItemCollectionName.StackableItems:
                // Note that the selectedGameItemIDs is a soft copy, so the item has already been removed.  
                if(inventoryUIModel.selectedGameItemIDs.Count == 0)
                {
                    inventoryUIModel.ClearSelectedState();
                    RefreshInventoryGridUI();
                    RefreshHoverItemUI();
                    RefreshPlayerCardUI();
                }
                else
                {
                    RefreshInventoryGridUI();
                    RefreshHoverItemUI();
                    RefreshPlayerCardUI();
                }
                break;

            default:
                Debug.Log("Bad unhandled case");
                break;
        }
    }

    // Sells the selected item. 
    public void SellItem()
    {
        // Refund gold amount.
        string idToRemove = inventoryUIModel.selectedGameItemIDs[0];
        // This should always be true. If not likely, dummy item is likely. 
        if(ItemManager.GetGameItem(idToRemove) is ConsumableItem)
        {
            int sellAmt = ((ConsumableItem) ItemManager.GetGameItem(idToRemove)).sellCost;
            gameObject.GetComponent<UIManager>().coinAmount += sellAmt;
        }
        else if(ItemManager.GetGameItem(idToRemove) is EquipmentItem)
        {
            int sellAmt = ((EquipmentItem) ItemManager.GetGameItem(idToRemove)).sellCost;
            gameObject.GetComponent<UIManager>().coinAmount += sellAmt;
        }
        else
        {
            Debug.Log("This should not happen");
        }
        // Remove the item.
        ConsumeSelectedItem();
    }

    public void UseItem()
    {
        // Have Item perform its effects
        string idToRemove = inventoryUIModel.selectedGameItemIDs[0];
        // This should always be true. If not likely, dummy item is likely. 
        if(ItemManager.GetGameItem(idToRemove) is ConsumableItem)
        {
            ((ConsumableItem) ItemManager.GetGameItem(idToRemove)).UseItem(player.GetComponent<PlayerController>());
        }
        // Remove the item.
        ConsumeSelectedItem(); 

    }
   

    public void EquipSelectedItem()
    {
        ItemManager.ChangeItemStatus(inventoryUIModel.selectedGameItemIDs[0], ItemStatus.Equipped);
        RefreshInventoryGridUI();
        RefreshHoverItemUI();
        RefreshPlayerCardUI();
    }

     public void UnequipSelectedItem()
    {
        Debug.Log(inventoryUIModel.selectedGameItemInventory);     
        ItemManager.ChangeItemStatus(inventoryUIModel.selectedGameItemIDs[0], ItemStatus.EquipmentInventory); 
        if(inventoryUIModel.selectedGameItemInventory != ItemCollectionName.EquipmentItems)
        {
            inventoryUIModel.ClearSelectedState();
        }
        
        RefreshInventoryGridUI();
        RefreshHoverItemUI();
        RefreshPlayerCardUI();
    }

    public void ChangeInventoryPanel(ItemCollectionName changeTo)
    {
        if(inventoryUIModel.openInventoryCollection != changeTo)
        {
            
            inventoryUIModel.openInventoryCollection = changeTo;
            // inventoryUIModel.openInventorySortMethod = "NameForwards";
            inventoryUIModel.inventoryPageNum = 1;
            inventoryUIModel.ClearSelectedState();
            RefreshInventoryGridUI();
            RefreshHoverItemUI();
            RefreshPlayerCardUI();
        }
    }

    public void ChangeInventoryPanelToEquipment()
    {
        ChangeInventoryPanel(ItemCollectionName.EquipmentItems);
    }

    public void ChangeInventoryPanelToItems()
    {
        ChangeInventoryPanel(ItemCollectionName.StackableItems);
    }

    public void EquippedInventorySlotSelected(InventorySlot slot)
    {
        hoverItemUI.ChangeHoverPosition(slot.GetComponent<RectTransform>().position);

        if(inventoryUIModel.isHovered == true || 
            inventoryUIModel.selectedGameItemIDs == null ||  
            !slot.item.GetName().Equals(inventoryUIModel.selectedGameItemName))
        {
            inventoryUIModel.selectedGameItemIDs = new List<string>();
            inventoryUIModel.selectedGameItemIDs.Add(slot.item.GetItemID());
            inventoryUIModel.selectedGameItemInventory = ItemCollectionName.PlayerInventory;
            inventoryUIModel.selectedGameItemName = slot.item.GetName();
            inventoryUIModel.isHovered = false;
            RefreshInventoryGridUI();
            RefreshHoverItemUI();
            RefreshPlayerCardUI();
        }
        else
        {
            inventoryUIModel.ClearSelectedState();
            RefreshInventoryGridUI();
            RefreshHoverItemUI();
            RefreshPlayerCardUI();
        }
    }

    public void EquippedInventorySlotHovered(InventorySlot slot, bool hoveredOn)
    {
        if(inventoryUIModel.isHovered == false && inventoryUIModel.selectedGameItemIDs != null)
        {
            return;
        }
       if(hoveredOn == true)
        {
            //hoverItemUI.ChangeHoverPosition(slot.transform.position);
            hoverItemUI.ChangeHoverPosition(slot.GetComponent<RectTransform>().position);
            inventoryUIModel.selectedGameItemIDs = new List<string>();
            inventoryUIModel.selectedGameItemIDs.Add(slot.item.GetItemID());
            inventoryUIModel.selectedGameItemInventory = ItemCollectionName.PlayerInventory;
            inventoryUIModel.selectedGameItemName = slot.item.GetName();
            inventoryUIModel.isHovered = true;
            RefreshInventoryGridUI();
            RefreshHoverItemUI();
            RefreshPlayerCardUI();
        }
        else
        {
            inventoryUIModel.ClearSelectedState();
            RefreshInventoryGridUI();
            RefreshHoverItemUI();
            RefreshPlayerCardUI();
        }
    }

    // Passed as an action delegate to the ItemSlots.  
    public void InventorySlotSelected(InventorySlot slot)
    {
        //hoverItemUI.ChangeHoverPosition(slot.transform.position);
        hoverItemUI.ChangeHoverPosition(slot.GetComponent<RectTransform>().position);
      
        switch(inventoryUIModel.openInventoryCollection)
        {
            case ItemCollectionName.EquipmentItems:
                // Check if previously hovered, nothing is selected, or different item is selected. 
                if(inventoryUIModel.isHovered == true || 
                    inventoryUIModel.selectedGameItemIDs == null ||  
                    !slot.item.GetItemID().Equals(inventoryUIModel.selectedGameItemIDs[0]))
                {
                    inventoryUIModel.selectedGameItemIDs = new List<string>();
                    inventoryUIModel.selectedGameItemIDs.Add(slot.item.GetItemID());
                    inventoryUIModel.selectedGameItemInventory = ItemCollectionName.EquipmentItems;
                    inventoryUIModel.selectedGameItemName = slot.item.GetName();
                    inventoryUIModel.isHovered = false;
                    RefreshInventoryGridUI();
                    RefreshHoverItemUI();
                    RefreshPlayerCardUI();
                }
                // Check if its already a selected item
                else
                {
                    inventoryUIModel.ClearSelectedState();
                    RefreshInventoryGridUI();
                    RefreshHoverItemUI();
                    RefreshPlayerCardUI();
                }
                
                break;
            case ItemCollectionName.StackableItems:
                 // Check if previously hovered, nothing is selected, or different item is selected. 
                if(inventoryUIModel.isHovered == true || 
                    inventoryUIModel.selectedGameItemIDs == null ||  
                    !slot.item.GetName().Equals(inventoryUIModel.selectedGameItemName))
                {
                    inventoryUIModel.selectedGameItemIDs = slot.itemStack;
                    inventoryUIModel.selectedGameItemInventory = ItemCollectionName.StackableItems;
                    inventoryUIModel.selectedGameItemName = slot.item.GetName();
                    inventoryUIModel.isHovered = false;
                    RefreshInventoryGridUI();
                    RefreshHoverItemUI();
                    RefreshPlayerCardUI();
                }
                else
                {
                    inventoryUIModel.ClearSelectedState();
                    RefreshInventoryGridUI();
                    RefreshHoverItemUI();
                    RefreshPlayerCardUI();
                }
                
                break;
            default:
                Debug.Log("Bad unhandled case");
                break;
        }
    }

    public void InventorySlotHovered(InventorySlot slot, bool hoveredOn)
    {
        
        //Debug.Log("Slot was hovered " + inventoryUIModel.isHovered + " " + (inventoryUIModel.selectedGameItemIDs == null));
        // Do nothing if item is selected. 
        if(inventoryUIModel.isHovered == false && inventoryUIModel.selectedGameItemIDs != null)
        {
            return;
        }
        switch(inventoryUIModel.openInventoryCollection)
        {
            case ItemCollectionName.EquipmentItems:
                // Check if previously hovered, nothing is selected, or different item is selected. 
                if(hoveredOn == true)
                {
                    //hoverItemUI.ChangeHoverPosition(slot.transform.position);
                    hoverItemUI.ChangeHoverPosition(slot.GetComponent<RectTransform>().position);
                    inventoryUIModel.selectedGameItemIDs = new List<string>();
                    inventoryUIModel.selectedGameItemIDs.Add(slot.item.GetItemID());
                    inventoryUIModel.selectedGameItemInventory = ItemCollectionName.EquipmentItems;
                    inventoryUIModel.selectedGameItemName = slot.item.GetName();
                    inventoryUIModel.isHovered = true;
                    RefreshInventoryGridUI();
                    RefreshHoverItemUI();
                    RefreshPlayerCardUI();
                }
                // Check if its already a selected item
                else
                {
                    inventoryUIModel.ClearSelectedState();
                    RefreshInventoryGridUI();
                    RefreshHoverItemUI();
                    RefreshPlayerCardUI();
                }
                
                break;
            case ItemCollectionName.StackableItems:
                 // Check if previously hovered, nothing is selected, or different item is selected. 
                if(hoveredOn == true)
                {
                    //hoverItemUI.ChangeHoverPosition(slot.transform.position);
                    hoverItemUI.ChangeHoverPosition(slot.GetComponent<RectTransform>().position);
                    inventoryUIModel.selectedGameItemIDs = slot.itemStack;
                    inventoryUIModel.selectedGameItemInventory = ItemCollectionName.StackableItems;
                    inventoryUIModel.selectedGameItemName = slot.item.GetName();
                    inventoryUIModel.isHovered = true;
                    RefreshInventoryGridUI();
                    RefreshHoverItemUI();
                    RefreshPlayerCardUI();
                }
                else
                {
                    inventoryUIModel.ClearSelectedState();
                    RefreshInventoryGridUI();
                    RefreshHoverItemUI();
                    RefreshPlayerCardUI();
                }
                
                break;
            default:
                Debug.Log("Bad unhandled case");
                break;
        }
    }

    public void ChangeinventorySortMethod(string newSortMethod)
    {

    }
    
    public void ToggleInventory()
    {
        if(!inventoryUIModel.isActive)
        {
            inventoryUIModel.ResetUIState();
            inventoryUIModel.isActive = true;     
            RefreshInventoryGridUI();
            RefreshHoverItemUI();
            RefreshPlayerCardUI();
        }
        else
        {
            inventoryUIModel.isActive = false;
        }
        uiCanvas.SetActive(inventoryUIModel.isActive);
    }

    public void ShopItemAdded()
    {
        if(inventoryUIModel.isActive)
        {
            this.RefreshInventoryGridUI();
            this.RefreshPlayerCardUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalStates.isPaused)
            return;
        if(Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        if(inventoryUIModel.isActive){
            if(Input.GetKeyDown(KeyCode.B))
            {
                DummyHealthPotion toAdd = new DummyHealthPotion(Resources.Load<Sprite>("Sprites/Health"));
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.EquipmentInventory);
                this.RefreshInventoryGridUI();
            }

            if(Input.GetKeyDown(KeyCode.C))
            {
                DummyHealthPotion toAdd = new DummyHealthPotion(Resources.Load<Sprite>("Sprites/Fire Rate"));
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.EquipmentInventory);
                this.RefreshInventoryGridUI();
            }

            if(Input.GetKeyDown(KeyCode.G))
            {
                DummyHealthPotion toAdd = new DummyHealthPotion(Resources.Load<Sprite>("Sprites/Health"), "Stealth Potion");
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.ItemInventory);
                this.RefreshInventoryGridUI();
            }

            if(Input.GetKeyDown(KeyCode.F))
            {
                DummyHealthPotion toAdd = new DummyHealthPotion(Resources.Load<Sprite>("Sprites/Fire Rate"), "Wealth Potion");
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.ItemInventory);
                this.RefreshInventoryGridUI();
            }
        }
    }
}
