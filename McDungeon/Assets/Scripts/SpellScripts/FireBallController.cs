using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobs;

namespace McDungeon
{
    public class FireBallController : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float lifeTime = 10f;
        [SerializeField] private int maxBounce;
        [SerializeField] private Vector3 direction;
        private int bouncedCount = 0;

        public void Config(float newSpeed, float newLifeTime, int newMaxBounce, Vector3 newDirection)
        {
            this.speed = newSpeed;
            this.lifeTime = newLifeTime;
            this.maxBounce = newMaxBounce;
            newDirection.z = 0f;
            this.direction = newDirection.normalized;
            Destroy(this.gameObject, lifeTime);
        }

        public void Config(float newSpeed, float newLifeTime, int newMaxBounce)
        {
            this.speed = newSpeed;
            this.lifeTime = newLifeTime;
            this.maxBounce = newMaxBounce;
            Destroy(this.gameObject, lifeTime);
        }

        void Update()
        {
            this.transform.position = this.transform.position + speed * direction * Time.deltaTime;
        }

        void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.tag == "BouncyWall")
            {
                float wallAngle = other.gameObject.GetComponent<Transform>().eulerAngles.z;
                float selfAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Calculate angle after refelection.
                float newSelfAngle = (360f - (selfAngle - wallAngle)) + wallAngle;
                newSelfAngle = newSelfAngle % 360;

                float newX = Mathf.Cos(newSelfAngle * Mathf.Deg2Rad);
                float newY = Mathf.Sin(newSelfAngle * Mathf.Deg2Rad);

                this.direction.x = newX;
                this.direction.y = newY;
                this.direction.z = 0f;
                bouncedCount++;
                if (bouncedCount > maxBounce) {
                    Destroy(this.gameObject);
                }
            }
            else if (other.gameObject.tag == "MobHitbox")
            {
                IMobController mobControl = other.gameObject.GetComponent<IMobController>();
                mobControl.TakeDamage(3f, EffectTypes.Ablaze);
                Destroy(this.gameObject);
            }
            else if (other.gameObject.tag == "BossHitbox")
            {
                BossController mobControl = other.gameObject.GetComponent<BossController>();
                mobControl.TakeDamage(3f, EffectTypes.Ablaze);
                Destroy(this.gameObject);
            }
        }
    }
}
