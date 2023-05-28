using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class WaterSurgeController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;
        [SerializeField] private float surgeInterval;
        [SerializeField] private Vector3 direction;
        private float timeSinceBorn;
        private ParticleSystemRenderer particelRenderer;
        private ParticleSystem particelSystem;
        private ParticleSystem.MainModule particelModule;
        private CircleCollider2D collider2D;
        private SpriteRenderer effectRenderer;
        private float surgeCount;

        void Start()
        {
            collider2D = this.gameObject.GetComponent<CircleCollider2D>();
            particelSystem = this.gameObject.GetComponent<ParticleSystem>();
            particelRenderer = particelSystem.GetComponent<ParticleSystemRenderer>();
            effectRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            particelModule = particelSystem.main; // Get the main module of the Particle System

            // Stop the Particle System
            particelSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particelModule.duration = surgeInterval;
            particelSystem.Play();

            surgeCount = 0;
        }

        public void Config(float speed, float lifeTime, float surgeInterval, Vector3 direction)
        {


            collider2D = this.gameObject.GetComponent<CircleCollider2D>();
            particelRenderer = this.gameObject.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();
            surgeCount = 0;
        }

        void Update()
        {
            this.transform.position = this.transform.position + speed * direction * Time.deltaTime;

            if (timeSinceBorn > surgeInterval * (surgeCount + 0.1f))
            {
                // Surge Brust.
                collider2D.enabled = true;
                effectRenderer.enabled = true;


                surgeCount++;
            }
            else if (timeSinceBorn > surgeInterval * (surgeCount - 0.4f))
            {
                // End last Surge Brust.
                collider2D.enabled = false;
                effectRenderer.enabled = false;
            }
            else if (timeSinceBorn > lifeTime)
            {
                // Destroy spell.
                Destroy(this.gameObject);
            }


            timeSinceBorn += Time.deltaTime;
        }

        void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.tag == "BouncyWall")
            {
            }
        }
    }
}
