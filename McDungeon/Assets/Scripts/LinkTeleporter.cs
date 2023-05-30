using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTeleporter : MonoBehaviour
{
    public GameObject targetRoom {get; set;} = null;
    public bool teleported {get; set;} = false;
    private bool beenDisabled = false;
    
    public Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate(){
        if (!targetRoom && !beenDisabled)
        {
            //disable collider so player can't teleport to a room that doesn't exist
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            beenDisabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (targetRoom != null){
            if (other.CompareTag("Player"))
            {
                if (!teleported){
                    targetRoom.GetComponent<LinkTeleporter>().teleported = true;
                    //transform position of player to a unit in front of the target room
                    //check target rotation and teleport in front of the door
                    if(targetRoom.transform.localRotation == Quaternion.Euler(0, 0, 0)){
                        other.transform.position = new Vector2(targetRoom.transform.position.x, targetRoom.transform.position.y - 2);
                    }
                    else if(targetRoom.transform.localRotation == Quaternion.Euler(0, 0, 90)){
                        other.transform.position = new Vector2(targetRoom.transform.position.x + 2, targetRoom.transform.position.y);
                    }
                    else if(targetRoom.transform.localRotation == Quaternion.Euler(0, 0, 180)){
                        other.transform.position = new Vector2(targetRoom.transform.position.x, targetRoom.transform.position.y + 2);
                    }
                    else if(targetRoom.transform.localRotation == Quaternion.Euler(0, 0, -90)){
                        other.transform.position = new Vector2(targetRoom.transform.position.x - 2, targetRoom.transform.position.y);
                    }
                    GameObject parentObject = targetRoom.transform.parent.gameObject;
                    mainCamera.transform.position = new Vector3(parentObject.transform.position.x, parentObject.transform.position.y, -10);
                    Debug.Log("Teleported to " + parentObject.name + " at " + targetRoom.transform.position);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            teleported = false;
        }
    }
}