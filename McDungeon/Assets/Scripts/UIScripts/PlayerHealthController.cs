using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public int maxHealth;
    public float curHealth;
    public int healthPerHeart = 1;

    private List<GameObject> hearts;
    public GameObject healthPrefab;

    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    public float heartMargin = 25.0f;

    void Start()
    {   
        hearts = new List<GameObject>();
        healthPerHeart = 2;
    }
    
    public void ChangeMaxHealth(int newMax)
    {
        if(((int)newMax) % ((int)healthPerHeart) != 0)
        {
            return;
        }
        if(maxHealth < newMax)
        {
            int numHearts = (int) (newMax / healthPerHeart); 
            while(hearts.Count < numHearts)
            {
                GameObject newHeart = Instantiate(healthPrefab, gameObject.transform);
                hearts.Add(newHeart);
                newHeart.transform.localPosition = new Vector3(hearts.Count * heartMargin, 0, 0); 
                newHeart.GetComponent<Image>().sprite = heartEmpty;
            }
        }
        else if(maxHealth > newMax)
        {
            int numHearts = newMax / healthPerHeart; 
            // Take the maxHealth, first heart represents 0-5, second is 5-10, 
            while(hearts.Count > numHearts)
            {
                Destroy(hearts[hearts.Count - 1]);
                hearts.RemoveAt(hearts.Count - 1);
            }
        }
        maxHealth = newMax;
    }

    public void SetNewHealth(float newHealth)
    {
        if(newHealth > maxHealth)
        {
            return;
        }
        curHealth = newHealth;
        int numHalfHearts = (int)(((newHealth * 2) - 0.01f)/ healthPerHeart) + 1;
        if(newHealth <= 0.0f)
        {
            numHalfHearts = 0;
        }
        Debug.Log(newHealth + " leads to " + numHalfHearts);
        for(int i = 0 ; i < hearts.Count ; i++)
        {
            // 4 half hearts, i = 0 is full, i = 1 is full, i = 2 is empty
            // 3 half hearts, i = 0 is full, i = 1 is half, i = 2 is empty
            if(numHalfHearts <= i * 2)
            {
                hearts[i].GetComponent<Image>().sprite = heartEmpty;
            }
            else if(numHalfHearts == (i * 2) + 1)
            {
                hearts[i].GetComponent<Image>().sprite = heartHalf;
            }
            else
            {
                hearts[i].GetComponent<Image>().sprite = heartFull;
            }
        }
        
    }

    public void SetHealthPerHeart(int newRatio)
    {
        healthPerHeart = newRatio;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            //ChangeMaxHealth(Random.Range(2, 10));
            ChangeMaxHealth(10);
            SetNewHealth(Random.Range(0.0f, 10.0f));
        }
    }
}
