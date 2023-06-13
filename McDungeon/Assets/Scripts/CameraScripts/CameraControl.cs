using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public enum CameraMode
    {
        LockOnPlayer,
        LockOnRoom,
        MoveToTarget
    }

    // The name of Camera didn't changed due to other script depended on the name to find it
    // is muti-functional camera controller than just PositionLock
    public class PositionLockCamera : MonoBehaviour
    {
        private Camera managedCamera;
        [SerializeField] protected GameObject Target;
        [SerializeField] protected float returnSpeed = 10f;
        [SerializeField] protected float moveToSpeed = 6f;
        [SerializeField] protected CameraMode cameraMode;
        [SerializeField] protected Vector3 roomCenter;
        [SerializeField] protected Vector3 targetPos; // Use another variable for clarity
        [SerializeField] protected bool justSwitchBack;


        private void Awake()
        {
            Target = GameObject.Find("Player");
            managedCamera = gameObject.GetComponent<Camera>();
            justSwitchBack = false;
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
                if ((distance).magnitude > 0.2f && justSwitchBack)
                {
                    // Just switch back to position lock Cam, move camera twords player.
                    Vector3 direction = distance.normalized;
                    managedCamera.transform.Translate(direction * returnSpeed * Time.fixedDeltaTime);
                }
                else if (targetPosition.y != cameraPosition.y || targetPosition.x != cameraPosition.x)
                {
                    cameraPosition = new Vector3(targetPosition.x, targetPosition.y, cameraPosition.z);
                    managedCamera.transform.position = cameraPosition;
                    justSwitchBack = false;
                }
            }
            else if (cameraMode == CameraMode.MoveToTarget)
            {
                var cameraPosition = managedCamera.transform.position;
                Vector3 distance = targetPos - cameraPosition;
                distance.z = 0f;

                if ((distance).magnitude > 0.2f)
                {
                    Vector3 direction = distance.normalized;
                    managedCamera.transform.Translate(direction * moveToSpeed * Time.fixedDeltaTime);
                }
                else if (targetPos.y != cameraPosition.y || targetPos.x != cameraPosition.x)
                {
                    cameraPosition = new Vector3(targetPos.x, targetPos.y, cameraPosition.z);
                    managedCamera.transform.position = cameraPosition;
                }

            }
            else
            {
                managedCamera.transform.position = roomCenter;
            }
        }

        public void changeCameraMode(CameraMode mode, Vector2 position)
        {
            cameraMode = mode;
            roomCenter = new Vector3(position.x, position.y, managedCamera.transform.position.z);
            targetPos = new Vector3(position.x, position.y, managedCamera.transform.position.z);
            if (mode == CameraMode.LockOnPlayer)
            {
                justSwitchBack = true;
            }
        }

        public void LockOnPlayer()
        {
            cameraMode = CameraMode.LockOnPlayer;
            var targetPosition = this.Target.transform.position;
            var cameraPosition = managedCamera.transform.position;

            cameraPosition = new Vector3(targetPosition.x, targetPosition.y, cameraPosition.z);
            justSwitchBack = false;
        }
    }
}
