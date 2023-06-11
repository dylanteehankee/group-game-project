using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentDetector : MonoBehaviour
{
    Animator animator;
    void Start(){
        GameObject parent = transform.parent.gameObject;
        GameObject bossEntrance = parent.transform.GetChild(2).gameObject;
        animator = bossEntrance.GetComponent<Animator>();
    }
    //If player is on collider2d, SlideOpen is true
    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            animator.SetBool("VentOpen", true);
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            animator.SetBool("VentOpen", false);
        }
    }
}
