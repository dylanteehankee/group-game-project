using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class FollowerController : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private float speed;
        [SerializeField] private float InnerRadius;
        [SerializeField] private float OuterRadius;

        void Update()
        {
            Vector3 distance = target.transform.position - this.transform.position;
            Vector3 UnitDir = distance.normalized;
            if (distance.magnitude > InnerRadius)
            {
                this.transform.position = this.transform.position + speed * UnitDir * Time.deltaTime;

            }

            if (distance.magnitude > OuterRadius)
            {
                this.transform.position = target.transform.position - UnitDir * OuterRadius;
            }
        }
    }
}
