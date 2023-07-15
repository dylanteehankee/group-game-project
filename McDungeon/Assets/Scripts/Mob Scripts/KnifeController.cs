using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class KnifeController : MonoBehaviour
    {
        [SerializeField]
        private int[] knifeSpeed = {700, 800};
        [SerializeField]
        private int[] damage = {1, 1};
        private float knockbackDuration = 1.0f;
        private int difficulty;

        void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log(collider.gameObject.tag);
            if (collider.gameObject.tag == "PlayerHitbox")
            {
                Vector2 location = this.transform.position;
                Vector2 playerLocation = collider.transform.position;
                var deltaLocation = playerLocation - location;
                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * knifeSpeed[difficulty]);
                collider.gameObject.GetComponent<PlayerController>().TakeDamage(damage[difficulty], EffectTypes.None);
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
            Destroy(this.gameObject, 1);
        }
    }
}