using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class FallingIceSharpController : MonoBehaviour
    {
        [SerializeField] private Vector3 startPos;
        [SerializeField] private Vector3 endPos;
        [SerializeField] private float formTime;
        [SerializeField] private float travelTime;
        [SerializeField] private float lifeTime;
        [SerializeField] private float targetScale;
        private Vector3 direction;
        private float speed;
        private float timeSinceBorn;
        private bool landed;
        private ParticleSystemRenderer particelRenderer;
        private CircleCollider2D collider2D;
        private GameObject iceSharp;

        const float startingScale = 0.02f;

        void Start()
        {
            collider2D = this.gameObject.GetComponent<CircleCollider2D>();
            iceSharp = this.gameObject.transform.GetChild(0).gameObject;
            particelRenderer = this.gameObject.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();
        }

        public void Config(Vector3 startPos, Vector3 endPos, float formTime, float travelTime, float lifeTime, float targetScale)
        {
            particelRenderer = this.gameObject.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();
            collider2D = this.gameObject.GetComponent<CircleCollider2D>();
            iceSharp = this.gameObject.transform.GetChild(0).gameObject;

            this.startPos = startPos;
            this.endPos = endPos;
            this.formTime = formTime;
            this.travelTime = formTime + travelTime;
            this.lifeTime = formTime + travelTime + lifeTime;
            this.targetScale = targetScale;
            var distance = endPos - startPos;
            distance.z = 0f;
            direction = distance.normalized;
            speed = distance.magnitude / travelTime;

            // Initialzie Status.
            timeSinceBorn = 0f;
            landed = false;
            this.transform.position = startPos;

            Vector3 unitVec = new Vector3(1f, 1f, 1f);
            this.transform.localScale = unitVec * startingScale;
            particelRenderer.enabled = false;
        }

        void Update()
        {
            if (timeSinceBorn < formTime)
            {
                // Forming IceSharp.
                Vector3 unitVec = new Vector3(1f, 1f, 1f);
                var currentScale = Mathf.Lerp(startingScale, targetScale, timeSinceBorn / formTime);
                this.transform.localScale = currentScale * unitVec;
            }
            else if (timeSinceBorn < travelTime)
            {
                // IceSharp Falling.
                this.transform.position = this.transform.position + speed * direction * Time.deltaTime;
            }
            else if (landed == false)
            {
                // IceSharp Landed.
                this.transform.position = endPos;
                this.landed = true;

                collider2D.enabled = true;
                particelRenderer.enabled = true;
                iceSharp.transform.localScale = new Vector3(0.05f, 0.2f, 0.2f);
            }
            else if (timeSinceBorn > lifeTime)
            {
                // IceSharp Expired.
                // Debug.Log("Destroied");
                Destroy(this.gameObject);
            }


            timeSinceBorn += Time.deltaTime;
            // Debug.Log("timeSinceBorn" + timeSinceBorn + "    -    lifeTime" + lifeTime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.tag == "BouncyWall")
            {
            }
        }
    }
}
