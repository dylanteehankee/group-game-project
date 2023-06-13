using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobs;
using McDungeon;

public class ItemFactory : MonoBehaviour
{    
    // LOL This code 
    public Sprite weapon0;
    public Sprite weapon1;
    public Sprite weapon2;
    public Sprite weapon3;
    public Sprite weapon4;
    public Sprite weapon5;
    public Sprite weapon6;

    public GameObject healthPotionDrop;
    public GameObject equipmentDropPrefab;

    void Start()
    {

    }
    void Update()
    {

    }

    public void DropHealthPotions(Transform parent, int count, Vector3 position, float variance)
    {
        for(int i = 0 ; i < count ; i++)
        {
            GameObject hpDrop = Instantiate(healthPotionDrop, parent);
            hpDrop.transform.localPosition = position + new Vector3(Random.Range(variance * -1, variance), Random.Range(variance * -1, variance), 0);
        }
    }

    public void DropBigPuzzleRewards(Transform parent, Vector3 position, float variance)
    {
        //DropItem(parent, position, variance, 1);
        DropEquipmentItemFromTier(parent, position, variance, 3);
        DropEquipmentItemFromTier(parent, position, variance, 2);
    }

    public void DropSmallPuzzleRewards(Transform parent, Vector3 position, float variance)
    {
        DropEquipmentItemFromTier(parent, position, variance, 2);
        DropEquipmentItemFromTier(parent, position, variance, 1);
    }

    public void DropEquipmentItemFromTier(Transform parent, Vector3 position, float variance, int tierNum)
    {
        (Sprite dropSprite, EquipmentItem item) tuple;
        switch(tierNum)
        {
            case 1:
                tuple = GetTier1Item();
                break;
            case 2:
                tuple = GetTier2Item();
                break;
            case 3:
                tuple = GetTier3Item();
                break;
            default:
                tuple = GetTier1Item();
                break;
        }
        
        GameObject equipmentDrop = Instantiate(equipmentDropPrefab, parent);
        equipmentDrop.GetComponent<EquipmentDrop>().Init(tuple.item, tuple.dropSprite);
        equipmentDrop.transform.localPosition = position + new Vector3(Random.Range(variance * -1, variance), Random.Range(variance * -1, variance), 0);
    }

    private (Sprite dropSprite, EquipmentItem item) GetTier1Item()
    {
        int randItem = Random.Range(0, 2);
        EquipmentItem item = null;
        Sprite sprite = null;
        switch(randItem)
        {
            case 0:
                item = new Weapon(
                    name: "Wooden Sword",
                    itemType: "Weapon",
                    sellCost: Random.Range(7, 12),
                    inventoryIcon: weapon0,
                    damage: RoundToTwoDigits(Random.Range(1.8f, 2.2f)),
                    range: 2,
                    attackSpeed: RoundToTwoDigits(Random.Range(0.6f, 1.1f)),
                    knockBack: Random.Range(4, 7),
                    attackAngle: 80f
                );
                sprite = weapon0;
                break;
            case 1:
                item = new Weapon(
                    name: "Rusty Dagger",
                    itemType: "Weapon",
                    sellCost: Random.Range(7, 12),
                    inventoryIcon: weapon6,
                    damage: RoundToTwoDigits(Random.Range(0.8f, 1.2f)),
                    range: 1,
                    attackSpeed: RoundToTwoDigits(Random.Range(1.5f, 2.2f)),
                    knockBack: Random.Range(2, 4),
                    attackAngle: 60f
                );
                sprite = weapon6;
                break;
        }
        return (sprite, item);
    }

    private (Sprite dropSprite, EquipmentItem item) GetTier2Item()
    {
        int randItem = Random.Range(0, 4);
        EquipmentItem item = null;
        Sprite sprite = null;
        switch(randItem)
        {
            case 0:
                item = new Weapon(
                    name: "Practice Sword",
                    itemType: "Weapon",
                    sellCost: Random.Range(10, 14),
                    inventoryIcon: weapon0,
                    damage: RoundToTwoDigits(Random.Range(2.6f, 3.4f)),
                    range: 2,
                    attackSpeed: RoundToTwoDigits(Random.Range(0.9f, 1.4f)),
                    knockBack: Random.Range(4, 7),
                    attackAngle: 80f
                );
                sprite = weapon0;
                break;
            case 1:
                item = new Weapon(
                    name: "Silver Sword",
                    itemType: "Weapon",
                    sellCost: Random.Range(18, 22),
                    inventoryIcon: weapon2,
                    damage: RoundToTwoDigits(Random.Range(2.2f, 2.8f)),
                    range: 2,
                    attackSpeed: RoundToTwoDigits(Random.Range(0.6f, 1.1f)),
                    knockBack: Random.Range(4, 7),
                    attackAngle: 80f
                );
                sprite = weapon2;
                break;
            case 2:
                item = new Weapon(
                    name: "Trusty Dagger",
                    itemType: "Weapon",
                    sellCost: Random.Range(14, 18),
                    inventoryIcon: weapon6,
                    damage: RoundToTwoDigits(Random.Range(1.4f, 1.8f)),
                    range: 1,
                    attackSpeed: RoundToTwoDigits(Random.Range(2f, 2.5f)),
                    knockBack: Random.Range(2, 4),
                    attackAngle: 50f
                );
                sprite = weapon6;
                break;
            case 3:
                item = new Weapon(
                    name: "Squeaky Toy",
                    itemType: "Weapon",
                    sellCost: Random.Range(14, 18),
                    inventoryIcon: weapon3,
                    damage: RoundToTwoDigits(Random.Range(2.2f, 2.8f)),
                    range: 2,
                    attackSpeed: RoundToTwoDigits(Random.Range(1.5f, 2.1f)),
                    knockBack: Random.Range(7, 10),
                    attackAngle: 60f
                );
                sprite = weapon3;
                break;
        }
        return (sprite, item);
    }

    private (Sprite dropSprite, EquipmentItem item) GetTier3Item()
    {
        int randItem = Random.Range(0, 3);
        EquipmentItem item = null;
        Sprite sprite = null;
        switch(randItem)
        {
            case 0:
                item = new Weapon(
                    name: "Shiny Sword",
                    itemType: "Weapon",
                    sellCost: Random.Range(25, 35),
                    inventoryIcon: weapon1,
                    damage: RoundToTwoDigits(Random.Range(3.8f, 4.7f)),
                    range: 2,
                    attackSpeed: RoundToTwoDigits(Random.Range(1.2f, 1.6f)),
                    knockBack: Random.Range(6, 9),
                    attackAngle: 80f
                );
                sprite = weapon1;
                break;
            case 1:
                item = new Weapon(
                    name: "Curved Sword",
                    itemType: "Weapon",
                    sellCost: Random.Range(22, 30),
                    inventoryIcon: weapon4,
                    damage: RoundToTwoDigits(Random.Range(5.2f, 5.8f)),
                    range: 3,
                    attackSpeed: RoundToTwoDigits(Random.Range(0.6f, 0.8f)),
                    knockBack: Random.Range(12, 16),
                    attackAngle: 80f
                );
                sprite = weapon4;
                break;
            case 2:
                item = new Weapon(
                    name: "Goblin Dagger",
                    itemType: "Weapon",
                    sellCost: Random.Range(20, 28),
                    inventoryIcon: weapon5,
                    damage: RoundToTwoDigits(Random.Range(2.2f, 2.8f)),
                    range: 1,
                    attackSpeed: RoundToTwoDigits(Random.Range(1.8f, 2.3f)),
                    knockBack: Random.Range(1, 2),
                    attackAngle: 45f
                );
                sprite = weapon5;
                break;
        }
        return (sprite, item);
    }

    private float RoundToTwoDigits(float rand)
    {
        return Mathf.Round(rand * 100.0f) * 0.01f;
    }

}
