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
    public Sprite weapon7;

    public GameObject healthPotionDrop;
    public GameObject weaponDrop;

    public GameObject armorDrop;

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

    public void DropBigPuzzleRewards(Transform parent, float position, float variance)
    {

    }
    public void DropSmallPuzzleRewards(Transform parent, float position, float variance)
    {

    }

    public EquipmentItem GetTierItem(int tier)
    {
        /*
        switch(tier) Random.Range(variance * -1, variance)
        {
            case 1:

            case 2:

            default:

        }*/
        return null;
    }

    private EquipmentItem GetTier1Item()
    {
        return null;
    }
}
