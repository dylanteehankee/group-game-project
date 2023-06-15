using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleport : MonoBehaviour
{
    [SerializeField] GameObject ventExit;
    [SerializeField] GameObject map;
    private MapGenerator mapGenerator;
    private AudioSource[] roomAudioSource;

    void Start()
    {
        var roomSoundManager = GameObject.FindWithTag("RoomSoundManager");
        roomAudioSource = roomSoundManager.GetComponents<AudioSource>();
        mapGenerator = map.GetComponent<MapGenerator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            roomAudioSource[6].Play();
            other.transform.position = ventExit.transform.position;
            mapGenerator.DisableMiniMap();
        }
    }
}
