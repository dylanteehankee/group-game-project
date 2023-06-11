using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    public GameItem item;

    public List<string> itemStack;

    public GameObject myButton;

    public GameObject myItemCount;

    Action<InventorySlot> onClick;

    Action<InventorySlot, bool> onHover;

    void Start()
    {

    }

    public void AddItem(GameItem newItem, Action<InventorySlot> _onClick, Action<InventorySlot, bool> _onHover)
    {
        myButton.GetComponent<Image>().color = new Color32(176,176,176,255);
        item = newItem;
        icon.sprite = item.GetInventoryIcon();
        icon.enabled = true;
        onClick = _onClick;
        onHover = _onHover;
        myItemCount.SetActive(false);
    }

    public void AddItemStack(List<string> items, Action<InventorySlot> _onClick, Action<InventorySlot, bool> _onHover)
    {
        myItemCount.GetComponent<Text>().text = items.Count.ToString();
        myButton.GetComponent<Image>().color = new Color32(176,176,176,255);
        itemStack = items;
        item = ItemManager.GetGameItem(itemStack[0]);
        icon.sprite = item.GetInventoryIcon();
        icon.enabled = true;
        onClick = _onClick;
        onHover = _onHover;
        myItemCount.SetActive(true);
    }


    public void ClearSlot()
    {
        myButton.GetComponent<Image>().color = new Color32(176,176,176,255);
        onClick = null;
        onHover = null;
        item = null;
        icon.enabled = false;
        myItemCount.SetActive(false);
    }

    public void OnClickItem()
    {
        if(onClick != null)
        {
            onClick(this);
        }
    }


    public void OnEnterItem()
    {
        if(onHover != null)
        {
            onHover(this, true);
        }
        
    }
    public void OnExitItem()
    {
       if(onHover != null)
        {
            onHover(this, false);
        }
    }

    public void SetSelectedSlot()
    {
        myButton.GetComponent<Image>().color = new Color32(127,255,107,250);
    }

    public void SetEquippedSlot()
    {
        myItemCount.GetComponent<Text>().text = "E";
        myItemCount.SetActive(true);
    }

     public void SetHoveredSlot()
    {
        myButton.GetComponent<Image>().color = new Color32(217,255,211,255);
    }
}
