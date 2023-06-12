using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;

public class LinkTeleporter : MonoBehaviour
{
    public GameObject TargetRoom {get; set;} = null;
    public bool Teleported {get; set;} = false;
    public bool isInside {get; set;} = false;
    private GameObject parent, grandparent;
    public bool beenDisabled = false;
    private bool combatStarted = false;
    private bool RoomCompleted = false;
    private bool closeDoor = false; 
    private Vector2 candlePos1, candlePos2;
    private PuzzleStateModel puzzleState;
    public bool onWallTile {get; set;} = false;
    private Animator animator;
    private MobManager mobManager;
    private PositionLockCamera positionLockCamera;
    private bool updateCameraLock;
    private PuzzleController puzzleController;

    void Start(){
        //look for gameobject with tag "MobSpawner"
        var CameraController = GameObject.FindWithTag("MainCamera");
        positionLockCamera = CameraController.GetComponent<PositionLockCamera>();

        var mobSpawner = GameObject.FindWithTag("MobSpawner");
        mobManager = mobSpawner.GetComponent<MobManager>();
        animator = GetComponent<Animator>();
        parent = transform.parent.gameObject;
        //if parent is a puzzle room, get puzzle controller
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
                if (mobManager.GetMobs().Count == 0){
                    closeDoor = false;
                    RoomCompleted = true;
                }
                else{
                    closeDoor = true;
                }
            }
            //if no puzzle in room, set hasPuzzle to false
            else if (parent.CompareTag("TutorialRoom") || parent.CompareTag("PuzzleRoom")){
                if (puzzleController != null){
                    if (puzzleController.GetPuzzleRoomState() != PuzzleRoomState.Completed){
                        //do this once
                        if (!updateCameraLock){
                            //set position lock to center of room
                            var tele1 = parent.transform.GetChild(1).gameObject;
                            var tele2 = parent.transform.GetChild(2).gameObject;
                            //center is in the middle of tele1 and tele2
                            var centerX = (tele1.transform.position.x + tele2.transform.position.x) / 2;
                            var centerY = (tele1.transform.position.y + tele2.transform.position.y) / 2;
                            var center = new Vector2Int((int)centerX, (int)centerY);
                            positionLockCamera.changeCameraMode(CameraMode.LockOnRoom, center);
                            updateCameraLock = true;
                        }
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
                        if(updateCameraLock){
                            //set position lock to player
                            positionLockCamera.changeCameraMode(CameraMode.LockOnPlayer, new Vector2Int(0, 0));
                            updateCameraLock = false;
                        }
                        GetComponent<SpriteRenderer>().enabled = true;
                        GetComponent<Animator>().enabled = true;
                        if(mobManager.GetMobs().Count == 0){
                            closeDoor = false;
                            RoomCompleted = true;
                        }
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
            if (other.CompareTag("PlayerHitbox"))
            {
                //if (!Teleported){
                //TargetRoom.GetComponent<LinkTeleporter>().Teleported = true;
                //transform position of player to a unit in front of the target room
                //check target rotation and teleport in front of the door
                if (TargetRoom.transform.localRotation == Quaternion.Euler(0, 0, 0)){
                    other.transform.position = new Vector2(TargetRoom.transform.position.x, TargetRoom.transform.position.y - 2.5f);
                }
                else if (TargetRoom.transform.localRotation == Quaternion.Euler(0, 0, 90)){
                    other.transform.position = new Vector2(TargetRoom.transform.position.x + 2, TargetRoom.transform.position.y);
                }
                else if (TargetRoom.transform.localRotation == Quaternion.Euler(0, 0, 180)){
                    other.transform.position = new Vector2(TargetRoom.transform.position.x, TargetRoom.transform.position.y + 2.5f);
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

                if (parent.CompareTag("TutorialRoom") || parent.CompareTag("PuzzleRoom"))
                {
                    positionLockCamera.LockOnPlayer();
                }

                if (parentTarget.CompareTag("CombatRoom") && !TargetRoom.GetComponent<LinkTeleporter>().RoomCompleted)
                {
                    GameObject grid = parentTarget.transform.GetChild(0).gameObject;
                    GameObject candle1 = grid.transform.GetChild(2).gameObject;
                    GameObject candle2 = grid.transform.GetChild(5).gameObject;
                    candlePos1 = candle1.transform.position;
                    candlePos2 = candle2.transform.position;

                    var RandomMob = Random.Range(0, 4);

                    mobManager.SpawnMobs((MobTypes)RandomMob, candlePos1, candlePos2);
                }
                grandparent.GetComponent<MapGenerator>().UpdateMiniMap(parentTarget);
                Debug.Log("Teleported to " + parentTarget.name + " at " + TargetRoom.transform.position);
            //}
            }
        }
    }

    //Reset all global variables
    //TODO: reset all variables in the room
    // public void ResetAll(){
    //     //reset all variables
    //     TargetRoom = null;
    //     Teleported = false;
    //     isInside = false;
    //     beenDisabled = false;
    //     RoomCompleted = false;
    //     closeDoor = false;
    //     onWallTile = false;
    //     candlePos1 = Vector3.zero;
    //     candlePos2 = Vector3.zero;
    //     grandparent = null;
    //     parent = null;
    //     mobSpawner = null;
    //     puzzleController = null;
    //     animator = null;

    //     //set animator to true
    //     GetComponent<SpriteRenderer>().enabled = true;
    //     GetComponent<Animator>().enabled = true;

    //     //Instatiate all again
    //     Start();
    // }
}