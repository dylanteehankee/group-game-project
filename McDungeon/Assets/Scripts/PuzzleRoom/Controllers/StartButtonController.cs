using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartButtonController : MonoBehaviour
{
    public PuzzleController pc;

    private float timeSincePush = 0.0f;

    public Sprite pushed;
    public float startDelay = 10f;

    private bool started = false;

    private AudioSource[] audioSource;

    void Awake()
    {
        startDelay = 0.2f;
        var roomSoundManager = GameObject.FindWithTag("RoomSoundManager");
        audioSource = roomSoundManager.GetComponents<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            started = true;
            GetComponent<SpriteRenderer>().sprite = pushed;
            audioSource[1].Play();
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
