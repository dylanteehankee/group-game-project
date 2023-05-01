using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private GameObject Target;

    private Camera managedCamera;

    // Start is called before the first frame update
    void Awake()
    {
        managedCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var targetPosition = this.Target.transform.position;
        var cameraPosition = managedCamera.transform.position;

        managedCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, cameraPosition.z);
    }
}
