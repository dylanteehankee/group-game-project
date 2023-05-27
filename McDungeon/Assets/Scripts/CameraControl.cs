using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class PositionLockCamera : MonoBehaviour
    {
        private Camera managedCamera;
        [SerializeField]
        protected GameObject Target;
        

        private void Awake()
        {
            managedCamera = gameObject.GetComponent<Camera>();
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            var targetPosition = this.Target.transform.position;
            var cameraPosition = managedCamera.transform.position;

            if (targetPosition.y != cameraPosition.y || targetPosition.x != cameraPosition.x)
            {
                cameraPosition = new Vector3(targetPosition.x, targetPosition.y, cameraPosition.z);
            }

            managedCamera.transform.position = cameraPosition;
        }
    }
}
