using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleport : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject ventExit;

    private AudioSource[] roomAudioSource;

    void Start()
    {
        var roomSoundManager = GameObject.FindWithTag("RoomSoundManager");
        roomAudioSource = roomSoundManager.GetComponents<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("PlayerHitbox")){
            roomAudioSource[6].Play();
            other.transform.position = ventExit.transform.position;
        }
    }
}
