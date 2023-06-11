using UnityEngine;
using UnityEngine.UI;

public class HealthPotion : ConsumableItem
{
    private int hpToRestore = 50;

    public HealthPotion(Sprite inventoryIcon) : base(
            name: "Health Potion", 
            sellCost: 20,
            itemType: "Potion",
            itemDescription: "Restores 50 hp to the player"
        )
    {
        this.inventoryIcon = inventoryIcon;
    }

    public override void UseItem(PlayerController playerController)
    {
        Debug.Log("Give the player some health " + hpToRestore);
        Debug.Log("Do something cool");
    }

}