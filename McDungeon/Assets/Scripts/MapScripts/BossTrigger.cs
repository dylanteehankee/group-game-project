using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private GameObject bossBlocker;
    private AudioSource[] bgAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        var parent = transform.parent.gameObject;
        bossBlocker = parent.transform.GetChild(0).gameObject;
        var backgroundSoundManager = GameObject.FindWithTag("BGSoundManager");
        bgAudioSource = backgroundSoundManager.GetComponents<AudioSource>();
    }

    // On Trigger play audio 2 and enable boss blocker box collider 2d.
    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("PlayerHitbox")){
            bgAudioSource[1].Play();
            bossBlocker.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
