using UnityEngine;
using UnityEngine.UI;
using McDungeon;
public class HealthPotion : ConsumableItem
{
    private int hpToRestore;

    public HealthPotion(Sprite inventoryIcon) : base(
            name: "Health Potion", 
            sellCost: 20,
            itemType: "Potion",
            itemDescription: "Restores 4hp to the player"
        )
    {
        this.inventoryIcon = inventoryIcon;
         hpToRestore = 4;
    }

    public override void UseItem(PlayerController playerController)
    {
        Debug.Log("Give the player some health " + hpToRestore);
        Debug.Log("Do something cool");
        playerController.RestoreHealth(hpToRestore);
    }

}