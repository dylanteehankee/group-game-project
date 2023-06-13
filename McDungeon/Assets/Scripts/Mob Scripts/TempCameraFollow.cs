using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    protected GameObject Target;
    private Camera managedCamera;
    // Start is called before the first frame update
    void Start()
    {
        managedCamera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var targetPosition = this.Target.transform.position;
        var cameraPosition = managedCamera.transform.position;
        // Set camera x and y to target x and y
        cameraPosition.x = targetPosition.x;
        cameraPosition.y = targetPosition.y;
        managedCamera.transform.position = cameraPosition;
    }
}