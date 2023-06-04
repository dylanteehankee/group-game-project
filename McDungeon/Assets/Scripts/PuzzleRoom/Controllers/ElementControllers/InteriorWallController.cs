using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteriorWallController : MonoBehaviour
{
    
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name.Equals("FIreBall(Clone)"))
        {
            Debug.Log("Bang");
            Destroy(collision.gameObject);
        }
    }

    void Update()
    {

    }

}
