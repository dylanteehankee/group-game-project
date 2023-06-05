using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class BoneController : MonoBehaviour
    {
        private bool active;
         public void Throw(Vector2 playerLocation)
        {
            Vector2 location = this.transform.position;
            var deltaLocation = playerLocation - location;
            deltaLocation.Normalize();
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * 800);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var collider = collision.collider;
            if (this.active && collider.gameObject.tag == "PlayerHitbox")
            {
                Vector2 location = this.transform.position;
                Vector2 playerLocation = collider.transform.position;
                var deltaLocation = playerLocation - location;
                deltaLocation.Normalize();
                collider.GetComponent<Rigidbody2D>().AddForce(deltaLocation * 1000);
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                this.GetComponent<Animator>().SetTrigger("BoneIdle");
                this.active = false;
            }
        }
    }
}