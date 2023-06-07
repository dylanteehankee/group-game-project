using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    // Start is called before the first frame update

    // Update is called once per frame
    void LateUpdate()
    {
        //set postion of minimap on the upper right corner of camera
        
        transform.position = new Vector3(mainCamera.transform.position.x + 5.5f, mainCamera.transform.position.y + 3.5f, 0);
    }
}
