using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartButtonController : MonoBehaviour
{
    public PuzzleController pc;

    private float timeSincePush = 0.0f;

    public Sprite pushed;
    public float startDelay = 1.0f;

    private bool started = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            started = true;
            GetComponent<SpriteRenderer>().sprite = pushed;
        }
    }
    
    void Update()
    {
        if(!started)
        {
            return;
        }
        timeSincePush += Time.deltaTime;

        Color oldColor = gameObject.GetComponent<SpriteRenderer>().color;
        oldColor.a = (startDelay - timeSincePush)/startDelay;
        gameObject.GetComponent<SpriteRenderer>().color = oldColor;

        if(timeSincePush > startDelay)
        {
            pc.StartPuzzleRoom();
        }
    }

}
