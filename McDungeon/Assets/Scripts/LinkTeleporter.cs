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
            //disable mesh renderer
            GetComponent<SpriteRenderer>().enabled = false;
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
                    other.transform.position = new Vector2(targetRoom.transform.position.x, targetRoom.transform.position.y);
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