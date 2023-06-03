using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartButtonController : MonoBehaviour
{
    public PuzzleController pc;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            pc.StartPuzzleRoom();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {

        }
    }

    void Update()
    {
        
    }

}
