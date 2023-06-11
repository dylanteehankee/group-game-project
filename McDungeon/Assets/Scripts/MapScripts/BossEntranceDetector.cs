using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntranceDetector : MonoBehaviour
{
    Animator animator;
    void Start(){
        animator = GetComponent<Animator>();
    }
    //If player is on collider2d, SlideOpen is true
    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            animator.SetBool("SlideOpen", true);
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            animator.SetBool("SlideOpen", false);
        }
    }
}
