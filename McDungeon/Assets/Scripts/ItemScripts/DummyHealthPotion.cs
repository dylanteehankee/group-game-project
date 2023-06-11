using UnityEngine;
using UnityEngine.UI;

public class DummyHealthPotion : GameItem
{
    private int hpToRestore = 0;

    public DummyHealthPotion(Sprite _icon) : base("Health Potion")
    {
        this.inventoryIcon = _icon;
    }

    public DummyHealthPotion(Sprite _icon, string _name) : base(_name)
    {
        this.inventoryIcon = _icon;
    }
    

}