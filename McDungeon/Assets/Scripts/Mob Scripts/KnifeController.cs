using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class KnifeController : MonoBehaviour
    {
        [SerializeField]
        private int knifeSpeed = 1000;
        [SerializeField]
        private int damage = 1;
        private float knockbackDuration = 1.0f;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "PlayerHitbox")
            {
                Vector2 location = this.transform.position;
                Vector2 playerLocation = collision.transform.position;
                var deltaLocation = playerLocation - location;
                playerRigidbody.AddForce(deltaLocation * knifeSpeed);
                Destroy(this.gameObject);
            }
        }

        public void Throw(Vector2 playerLocation)
        {
            Vector2 location = this.transform.position;
            var deltaLocation = playerLocation - location;
            deltaLocation.Normalize();
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * knifeSpeed);
            Destroy(this.gameObject, 3);
        }
    }
}