using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInRoom : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject parent, grandParent, Portal1, Portal2, Portal3, Portal4;
    void Start()
    {
        parent = transform.parent.gameObject;
        grandParent = parent.transform.parent.gameObject;
        Portal1 = grandParent.transform.GetChild(1).gameObject;
        Portal2 = grandParent.transform.GetChild(2).gameObject;
        Portal3 = grandParent.transform.GetChild(3).gameObject;
        Portal4 = grandParent.transform.GetChild(4).gameObject;
    }

    // Update is called once per frame
    /*void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")){ 
            Debug.Log("Player is in room"); 
            Portal1.GetComponent<LinkTeleporter>().isInside = true;
            Portal2.GetComponent<LinkTeleporter>().isInside = true;
            Portal3.GetComponent<LinkTeleporter>().isInside = true;
            Portal4.GetComponent<LinkTeleporter>().isInside = true;
        }
    }*/
}
