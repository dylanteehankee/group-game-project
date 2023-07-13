using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class KnifeController : MonoBehaviour
    {
        [SerializeField]
        private int[] knifeSpeed = {800, 1000};
        [SerializeField]
        private int[] damage = {1, 1};
        private float knockbackDuration = 1.0f;
        private int difficulty;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "PlayerHitbox")
            {
                Vector2 location = this.transform.position;
                Vector2 playerLocation = collision.transform.position;
                var deltaLocation = playerLocation - location;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * knifeSpeed[difficulty]);
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage[difficulty], EffectTypes.None);
            }
            if (collision.gameObject.tag != "Throwable")
            {
                Destroy(this.gameObject);
            }
        }

        public void Throw(Vector2 playerLocation, int difficulty)
        {
            this.difficulty = difficulty;
            Vector2 location = this.transform.position;
            var deltaLocation = playerLocation - location;
            deltaLocation.Normalize();
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * knifeSpeed[difficulty]);
            Destroy(this.gameObject, 3);
        }
    }
}