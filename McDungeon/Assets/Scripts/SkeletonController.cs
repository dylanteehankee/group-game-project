using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class SkeletonController : MonoBehaviour, IMobController
    {
        [SerializeField]
        private float mobHealth = 15;
        [SerializeField]
        private float attackRange = 5f;
        [SerializeField]
        private float moveSpeed = 1f;
        [SerializeField]
        private float hitStun = 0.5f;
        private float stunDelayTime = 0f;
        private float throwCooldown = 1f;
        private bool hasBone = true;
        [SerializeField]
        private GameObject playerObject;
        [SerializeField]
        private GameObject potionDropPrefab;
        [SerializeField]
        private GameObject bonePrefab;
        private GameObject bone;
        private float elapsedThrowCD = 0f;

        void Update()
        {
            if (this.stunDelayTime < hitStun)
            {
                this.stunDelayTime += Time.deltaTime;
            }
            else
            {
                moveTowardPlayer();
            }
        }

        void OnDestroy()
        {
            this.transform.parent.gameObject.GetComponent<MobManager>().Unsubscribe(this.gameObject);
        }

        public void GetPlayer(GameObject player)
        {
            this.playerObject = player;
        }

        private void moveTowardPlayer()
        {
            Vector2 position = this.transform.position;
            Vector2 playerLocation = this.playerObject.transform.position;
            var deltaLocation = playerLocation - position;
            
            if (Vector2.Distance(position, playerLocation) < this.attackRange && hasBone)
            {
                if (this.elapsedThrowCD < this.throwCooldown)
                {
                    this.elapsedThrowCD += Time.deltaTime;
                }
                else
                {
                    this.transform.Translate(Vector2.zero);
                    this.attackPlayer(deltaLocation);
                }
            }
            else if (!hasBone)
            {
                Vector2 boneLocation = this.bone.transform.position;
                deltaLocation = boneLocation - position;
            }
            deltaLocation.Normalize();
            this.transform.Translate(deltaLocation * Time.deltaTime * moveSpeed);
        }

        private void attackPlayer(Vector2 deltaLocation)
        {
            deltaLocation.Normalize();
            Vector2 location = this.transform.position;
            this.bone = (GameObject)Instantiate(this.bonePrefab);
            this.bone.transform.position = location + deltaLocation;
            this.bone.GetComponent<BoneController>().Throw(this.playerObject.transform.position);
            Debug.Log("ATTACKING PLAYER");
            this.hasBone = false;
        }

        public void TakeDamage(float damage)
        {
            this.stunDelayTime = 0;
            this.mobHealth -= damage;
            if (this.mobHealth < 0)
            {
                Destroy(this.gameObject);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var collider = collision.collider;
            if (collider.gameObject.tag == "Bone")
            {
                Destroy(collision.gameObject);
                this.hasBone = true;
                this.elapsedThrowCD = 0.0f;
            }
        }
    }
}