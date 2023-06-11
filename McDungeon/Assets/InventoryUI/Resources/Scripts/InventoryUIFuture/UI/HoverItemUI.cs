using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Inventory;
using System;

public class HoverItemUI : MonoBehaviour
{
    private Dictionary<HoverItemButtonNames, GameObject> hoverItemButtons;

    public GameObject movablePanel;

    public GameObject panel_itemNameText;
    public GameObject panel_itemTypeText;
    public GameObject panel_itemDescriptionText;
    public GameObject panel_itemCount;

    public GameObject sellItemButton;
    public GameObject useItemButton;
    public GameObject equipItemButton;
    public GameObject unequipItemButton;

    [SerializeField] public float hoverPanelWidth = 170.0f;
    [SerializeField] public float hoverPanelHeight = 150.0f;
    [SerializeField] public float hoverPanelOffset = -5f;

    [SerializeField] public float leftCoefficient = -0.0083f;

    void Start()
    {
        hoverItemButtons = new Dictionary<HoverItemButtonNames, GameObject>();
        hoverItemButtons.Add(HoverItemButtonNames.SellItem, sellItemButton);
        hoverItemButtons.Add(HoverItemButtonNames.UseItem, useItemButton);
        hoverItemButtons.Add(HoverItemButtonNames.EquipItem, equipItemButton);
        hoverItemButtons.Add(HoverItemButtonNames.UnequipItem, unequipItemButton);
    }

    public void LoadItemStack(string itemName, List<string> itemIDs)
    {
        movablePanel.SetActive(true);

        GameItem tokenItem = ItemManager.GetGameItem(itemIDs[0]);

        panel_itemNameText.GetComponent<Text>().text = tokenItem.GetName();
        panel_itemTypeText.GetComponent<Text>().text = tokenItem.itemType;
        panel_itemDescriptionText.GetComponent<Text>().text = tokenItem.itemDescription;
        
        panel_itemCount.SetActive(true);
        panel_itemCount.transform.GetChild(0).GetComponent<Text>().text = "Count: " + itemIDs.Count.ToString();
    }

    public void ChangeHoverPosition(Vector3 newPos)
    {
        //Debug.Log(newPos);
        RectTransform rt = movablePanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(hoverPanelWidth, hoverPanelHeight);
        // Centers panel at slot and moves -x offset. 
        
        movablePanel.transform.position = newPos + new Vector3((rt.sizeDelta[0]*leftCoefficient) + hoverPanelOffset, 0, 0);
        //rt.position = newPos + new Vector3((rt.sizeDelta[0]*leftCoefficient) + hoverPanelOffset, 0, 0);
        panel_itemDescriptionText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            rt.sizeDelta[0] - 20.0f,
            panel_itemDescriptionText.GetComponent<RectTransform>().sizeDelta[1]
        );
    }

    public void LoadItemSingular(string itemID)
    {
        movablePanel.SetActive(true);
        GameItem selectedItem = ItemManager.GetGameItem(itemID);

        panel_itemNameText.GetComponent<Text>().text = selectedItem.GetName();
        panel_itemTypeText.GetComponent<Text>().text = selectedItem.itemType;;
        panel_itemDescriptionText.GetComponent<Text>().text = selectedItem.itemDescription;;
        
        panel_itemCount.SetActive(false);
    }

    public void LoadNoItems()
    {
        movablePanel.SetActive(false);
    }   

    public void SetButtonsActive(List<HoverItemButtonNames> setActive)
    {
        for(int i = 0 ; i < setActive.Count ; i++)
        {
            hoverItemButtons[setActive[i]].SetActive(true);
        }
    }

    public void SetAllButtonsInactive()
    {
        // Go through all the buttons and set them as inactive. 
        foreach (HoverItemButtonNames buttonEnum in Enum.GetValues(typeof(HoverItemButtonNames)))
        {
            hoverItemButtons[buttonEnum].SetActive(false);
        }
    }
}
public enum HoverItemButtonNames
{
    SellItem,
    EquipItem,
    UnequipItem,
    UseItem
}