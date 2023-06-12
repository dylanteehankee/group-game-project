using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    enum CameraMode
    {
        LockOnPlayer,
        LockOnRoom
    }

    public class PositionLockCamera : MonoBehaviour
    {
        private Camera managedCamera;
        [SerializeField] protected GameObject Target;
        [SerializeField] protected float returnSpeed;
        [SerializeField] protected CameraMode cameraMode;
        [SerializeField] protected Vector3 roomCenter;


        private void Awake()
        {
            managedCamera = gameObject.GetComponent<Camera>();
            returnSpeed = 6f;
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void FixedUpdate()
        {
            if (cameraMode == CameraMode.LockOnPlayer)
            {
                var targetPosition = this.Target.transform.position;
                var cameraPosition = managedCamera.transform.position;

                Vector3 distance = targetPosition - cameraPosition;
                distance.z = 0f;
                if ((distance).magnitude > 0.5f)
                {
                    // Just switch back to position lock Cam, move camera twords player.
                    Vector3 direction = distance.normalized;
                    managedCamera.transform.Translate(direction * returnSpeed * Time.fixedDeltaTime);
                }
                else if (targetPosition.y != cameraPosition.y || targetPosition.x != cameraPosition.x)
                {
                    cameraPosition = new Vector3(targetPosition.x, targetPosition.y, cameraPosition.z);
                    managedCamera.transform.position = cameraPosition;
                }
            }
            else
            {
                managedCamera.transform.position = roomCenter;
            }
        }

        void changeCameraMode(CameraMode mode, Vector2 center)
        {
            cameraMode = mode;
            roomCenter = new Vector3(center.x, center.y, managedCamera.transform.position.z);
        }
    }
}
