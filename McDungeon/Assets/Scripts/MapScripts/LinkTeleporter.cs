using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTeleporter : MonoBehaviour
{
    public GameObject TargetRoom {get; set;} = null;
    public bool Teleported {get; set;} = false;
    public bool isInside {get; set;} = false;
    private GameObject parent;
    private bool beenDisabled = false;
    private bool closeDoor = false; 
    Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
        parent = transform.parent.gameObject;
    }

    void LateUpdate(){
        if (!TargetRoom && !beenDisabled)
        {
            //disable collider so player can't teleport to a room that doesn't exist
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            beenDisabled = true;
        }

        if (TargetRoom && isInside){
            //if player already picked in startRoom, open door
            if (parent.CompareTag("StartRoom")){
                /*if (enemyCount == 0){
                    closeDoor = false;
                }
                else{
                    closeDoor = true;
                }*/
            }
            //if no enemies in room, set hasEnemies to false
            else if (parent.CompareTag("CombatRoom")){
                /*if (enemyCount == 0){
                    closeDoor = false;
                }
                else{
                    closeDoor = true;
                }*/
            }
            //if no puzzle in room, set hasPuzzle to false
            else if (parent.CompareTag("PuzzleRoom")){
                /*if (puzlleFinished == true){
                    closeDoor = false;
                }
                else{
                    closeDoor = true;
                }*/
            }
            //if player already is in shop, keep door open
            else if (parent.CompareTag("ShopRoom")){
                closeDoor = false;
            }
            else if (parent.CompareTag("EndRoom")){
                closeDoor = true;
            }
            
            //if player is inside room, close door if closeDoor is true
            if (closeDoor){
                animator.SetBool("CloseDoor", true);
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else{
                animator.SetBool("CloseDoor", false);
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (TargetRoom != null){
            if (other.CompareTag("Player"))
            {
                if (!Teleported){
                    TargetRoom.GetComponent<LinkTeleporter>().Teleported = true;
                    //transform position of player to a unit in front of the target room
                    //check target rotation and teleport in front of the door
                    if (TargetRoom.transform.localRotation == Quaternion.Euler(0, 0, 0)){
                        other.transform.position = new Vector2(TargetRoom.transform.position.x, TargetRoom.transform.position.y - 2);
                    }
                    else if (TargetRoom.transform.localRotation == Quaternion.Euler(0, 0, 90)){
                        other.transform.position = new Vector2(TargetRoom.transform.position.x + 2, TargetRoom.transform.position.y);
                    }
                    else if (TargetRoom.transform.localRotation == Quaternion.Euler(0, 0, 180)){
                        other.transform.position = new Vector2(TargetRoom.transform.position.x, TargetRoom.transform.position.y + 2);
                    }
                    else if (TargetRoom.transform.localRotation == Quaternion.Euler(0, 0, -90)){
                        other.transform.position = new Vector2(TargetRoom.transform.position.x - 2, TargetRoom.transform.position.y);
                    }

                    GameObject parentTarget = TargetRoom.transform.parent.gameObject;

                    GameObject Portal1 = parentTarget.transform.GetChild(1).gameObject;
                    GameObject Portal2 = parentTarget.transform.GetChild(2).gameObject;
                    GameObject Portal3 = parentTarget.transform.GetChild(3).gameObject;
                    GameObject Portal4 = parentTarget.transform.GetChild(4).gameObject;

                    Portal1.GetComponent<LinkTeleporter>().isInside = true;
                    Portal2.GetComponent<LinkTeleporter>().isInside = true;
                    Portal3.GetComponent<LinkTeleporter>().isInside = true;
                    Portal4.GetComponent<LinkTeleporter>().isInside = true;

                    Debug.Log("Teleported to " + parentTarget.name + " at " + TargetRoom.transform.position);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Teleported = false;
        }
    }
}