using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTeleporter : MonoBehaviour
{
    public GameObject TargetRoom {get; set;} = null;
    public bool Teleported {get; set;} = false;
    public bool isInside {get; set;} = false;
    private GameObject parent, grandparent;
    public bool beenDisabled = false;
    private bool RoomCompleted = false;
    private bool closeDoor = false; 
    private PuzzleStateModel puzzleState;
    public bool onWallTile {get; set;} = false;
    private Animator animator;

    private PuzzleController puzzleController;

    void Start(){
        animator = GetComponent<Animator>();
        parent = transform.parent.gameObject;
        if(parent.CompareTag("TutorialRoom") || parent.CompareTag("PuzzleRoom")){
            puzzleController = parent.transform.GetChild(5).GetComponent<PuzzleController>();
        }
        grandparent = parent.transform.parent.gameObject;
    }

    void LateUpdate(){
        if (!TargetRoom && !beenDisabled)
        {
            //disable collider so player can't teleport to a room that doesn't exist
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Animator>().enabled = false;
            //GetComponent<BoxCollider2D>().enabled = false;
            beenDisabled = true;
        }

        //RoomCompleted makes sure that this update only runs up until the room is completed
        if (TargetRoom && isInside && !RoomCompleted){
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
            else if (parent.CompareTag("TutorialRoom") || parent.CompareTag("PuzzleRoom")){
                if (puzzleController != null){
                    if (puzzleController.GetPuzzleRoomState() != PuzzleRoomState.Completed){
                        closeDoor = true;
                        if(puzzleController.GetPuzzleRoomState() == PuzzleRoomState.InProgress){
                           GetComponent<SpriteRenderer>().enabled = false;
                        }
                        else{
                            GetComponent<SpriteRenderer>().enabled = true;
                        }
                    }
                    else {
                        Debug.Log ("Puzzle completed");
                        closeDoor = false;
                        RoomCompleted = true;
                    }
                }
            }
            //if player already is in shop, keep door open
            else if (parent.CompareTag("ShopRoom")){
                closeDoor = false;
                RoomCompleted = true;
            }
            //if player already is in endRoom, keep door closed
            else if (parent.CompareTag("EndRoom")){
                closeDoor = true;
                RoomCompleted = true;
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
        if(other.CompareTag("Wall")){
            onWallTile = true;
        }

        if (TargetRoom != null){
            if (other.CompareTag("Player"))
            {
                //if (!Teleported){
                //TargetRoom.GetComponent<LinkTeleporter>().Teleported = true;
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

                grandparent.GetComponent<MapGenerator>().updateMiniMap(parentTarget);
                Debug.Log("Teleported to " + parentTarget.name + " at " + TargetRoom.transform.position);
            //}
            }
        }
    }

    //Reset all global variables
    //TODO: reset all variables in the room
    void resetAll(){
        GameObject parentTarget = TargetRoom.transform.parent.gameObject;

        GameObject Portal1 = parentTarget.transform.GetChild(1).gameObject;
        GameObject Portal2 = parentTarget.transform.GetChild(2).gameObject;
        GameObject Portal3 = parentTarget.transform.GetChild(3).gameObject;
        GameObject Portal4 = parentTarget.transform.GetChild(4).gameObject;

        Portal1.GetComponent<LinkTeleporter>().isInside = false;
        Portal2.GetComponent<LinkTeleporter>().isInside = false;
        Portal3.GetComponent<LinkTeleporter>().isInside = false;
        Portal4.GetComponent<LinkTeleporter>().isInside = false;

        Teleported = false;
        isInside = false;
        beenDisabled = false;
        RoomCompleted = false;
        closeDoor = false;
        onWallTile = false;

        //Instatiate all again
        Start();
    }
}