using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
namespace Mobs
{
    public enum ItemTypes
    {
        Equipable,
        Consumable
    }

    public abstract class DroppedItemController : MonoBehaviour
    {
        [SerializeField]
        private ItemTypes type;

        private GameItem myItem = null;

        protected void Init(GameItem myItem, ItemTypes itemType)
        {   
            type = itemType;
            this.myItem = myItem;
        }
        // Unused. 
        public ItemTypes getItemType()
        {
            return this.type;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (myItem != null && collider.gameObject.tag == "PlayerHitbox")
            {
                //DummyHealthPotion toAdd = new DummyHealthPotion(Resources.Load<Sprite>("Sprites/Health"));
                if(type == ItemTypes.Equipable){
                    ItemManager.ChangeItemStatus(myItem.GetItemID(), ItemStatus.EquipmentInventory);
                    
                }
                else if(type == ItemTypes.Consumable){
                    ItemManager.ChangeItemStatus(myItem.GetItemID(), ItemStatus.ItemInventory);
                }
                GameObject.Find("GameManager").GetComponent<InventoryController>().ShopItemAdded();
                Destroy(this.gameObject);
            }
        }
    }
}
