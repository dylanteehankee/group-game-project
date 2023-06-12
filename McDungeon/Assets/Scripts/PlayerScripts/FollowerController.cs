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
        private SpriteRenderer renderer;

        void Awake()
        {
            renderer = this.gameObject.GetComponent<SpriteRenderer>();
        }

        void FixedUpdate()
        {
            Vector3 distance = target.transform.position - this.transform.position;
            Vector3 UnitDir = distance.normalized;
            if (distance.magnitude > InnerRadius)
            {
                this.transform.position = this.transform.position + speed * UnitDir * Time.fixedDeltaTime;

            }

            if (distance.magnitude > OuterRadius)
            {
                this.transform.position = target.transform.position - UnitDir * OuterRadius;
            }

            if (distance.y > 0f)
            {
                renderer.sortingOrder = 21;
            }
            else
            {
                renderer.sortingOrder = 19;
            }
        }
    }
}
