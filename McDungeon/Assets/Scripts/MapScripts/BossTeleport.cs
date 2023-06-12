using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleport : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject ventExit;

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("PlayerHitbox")){
            other.transform.position = ventExit.transform.position;
        }
    }
}
