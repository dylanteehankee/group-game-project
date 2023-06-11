using UnityEngine;
using UnityEngine.UI;

public class HealthPotion : GameItem
{
    public HealthPotion(string _name) : base(_name)
    {
        this.inventoryIcon = Resources.Load<Sprite>("Sprites/Health");
    }
    public HealthPotion() : base("Health Potion")
    {
        this.inventoryIcon = Resources.Load<Sprite>("Sprites/Health");
    }

    public HealthPotion(Sprite _icon) : base("Health Potion")
    {
        this.inventoryIcon = _icon;
    }

    public HealthPotion(Sprite _icon, string _name) : base(_name)
    {
        this.inventoryIcon = _icon;
    }

    public override bool CanChangeStatus(ItemStatus ic)
    {
        return true;
    }
}