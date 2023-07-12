using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobs;

namespace McDungeon
{
    public class ThunderController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject targetMob;
        [SerializeField] private GameObject indicator;
        [SerializeField] private GameObject animation;
        private Vector3 direction;
        private bool reached = false;
        private bool hasTarget = true;
        private float strikeTimer = 0.5f;

        public void Config(float speed, GameObject targetMob, bool hasTarget = true)
        {
            this.speed = speed;
            this.targetMob = targetMob;

            if (!hasTarget)
            {
                // Randomnize a direction
                float angle = Random.Range(0f, 360f * Mathf.Deg2Rad);
                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);

                direction = new Vector3(x, y, 0f);
            }
            Destroy(this.gameObject, 10f);
        }

        void Update()
        {
            Vector3 distance;
            if (targetMob != null && !targetMob.Equals(null))
            {
                distance = targetMob.transform.position - this.transform.position;
            }
            else
            {
                distance = new Vector3(0f, 0f, 0f);
            }

            if (hasTarget)
            {
                direction = distance.normalized;
            }


            if (distance.magnitude > speed * Time.deltaTime || !hasTarget)
            {
                this.transform.position = this.transform.position + speed * direction * Time.deltaTime;
            }
            else
            {
                reached = true;
                indicator.SetActive(false);
                animation.SetActive(true);
            }

            if (reached && targetMob != null && !targetMob.Equals(null) && targetMob.gameObject.GetComponent<IMobController>() != null)
            {
                this.transform.position = targetMob.transform.position;
                strikeTimer -= Time.deltaTime;
                if (strikeTimer < 0f && strikeTimer > -10f)
                {

                    // Damage Mob ========================================================
                    IMobController mobControl = targetMob.gameObject.GetComponent<IMobController>();
                    mobControl.TakeDamage(2f, EffectTypes.Slow);


                    strikeTimer = -10f; // Will never triger again.
                } 
            }
        }
    }
}
