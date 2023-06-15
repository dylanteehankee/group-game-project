using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentDetector : MonoBehaviour
{
    private Animator animator;
    private AudioSource[] roomAudioSource;
    void Start()
    {
        var roomSoundManager = GameObject.FindWithTag("RoomSoundManager");
        roomAudioSource = roomSoundManager.GetComponents<AudioSource>();
        GameObject parent = transform.parent.gameObject;
        GameObject bossEntrance = parent.transform.GetChild(2).gameObject;
        animator = bossEntrance.GetComponent<Animator>();
    }
    //If player is on collider2d, SlideOpen is true
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            roomAudioSource[5].Play();
            animator.SetBool("VentOpen", true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            roomAudioSource[6].Play();
            animator.SetBool("VentOpen", false);
        }
    }
}
