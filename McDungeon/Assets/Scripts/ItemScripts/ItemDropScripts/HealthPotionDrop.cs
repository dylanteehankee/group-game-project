using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
namespace Mobs
{
    public class HealthPotionDrop : DroppedItemController
    {
        [SerializeField]
        public Sprite inventoryItemSprite;

        void Start()
        {
            HealthPotion item = new HealthPotion(inventoryItemSprite);
            base.Init(item, ItemTypes.Consumable);
        }
    }
}
