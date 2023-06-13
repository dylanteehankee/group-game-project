using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
namespace Mobs
{
    public class EquipmentDrop : DroppedItemController
    {


        void Init(EquipmentItem item, Sprite dropIcon)
        {   
            GetComponent<SpriteRenderer>().sprite = dropIcon;
            base.Init(item, ItemTypes.Consumable);
        }
    }
}
