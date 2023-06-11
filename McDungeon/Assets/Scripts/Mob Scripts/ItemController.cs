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

    public class ItemController : MonoBehaviour
    {
        [SerializeField]
        private ItemTypes type;

        public ItemTypes getItemType()
        {
            return this.type;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "PlayerHitbox")
            {
                HealthPotion toAdd = new HealthPotion(Resources.Load<Sprite>("Sprites/Health"));
                ItemManager.ChangeItemStatus(toAdd.GetItemID(), ItemStatus.ItemInventory);
                Destroy(this.gameObject);
            }
        }
    }
}
