using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class GNelfController : MonoBehaviour, IMobController
    {
        [SerializeField]
        private float mobHealth = 10;
        [SerializeField]
        private float attackRange = 1.5f;
        [SerializeField]
        private float attackSpeed = 1f;
        [SerializeField]
        public int MobDamage = 1;
        [SerializeField]
        private float moveSpeed = 1f;
        [SerializeField]
        private float hitStun = 1f;
        private float stunDelayTime = 0f;
        private float attackCooldown = 0f;

        private GameObject playerObject;
        [SerializeField]
        private GameObject potionDropPrefab;

        void Update()
        {
            this.attackCooldown += Time.deltaTime;
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
            deltaLocation.Normalize();

            if (deltaLocation.x > Mathf.Abs(deltaLocation.y))
            {
                this.GetComponent<Animator>().SetInteger("Direction", 0);
                if (deltaLocation.x < 0)
                {
                    this.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            else if (deltaLocation.y < 0)
            {
                this.GetComponent<Animator>().SetInteger("Direction", 1);
            }
            else
            {
                this.GetComponent<Animator>().SetInteger("Direction", -1);
            }

            if (Vector2.Distance(position, playerLocation) < this.attackRange)
            {
                this.transform.Translate(Vector2.zero);
                this.attackPlayer(deltaLocation);
            }
            else
            {
                this.transform.Translate(deltaLocation * Time.deltaTime * moveSpeed);
            }
        }

        private void attackPlayer(Vector2 deltaLocation)
        {
            if (this.attackCooldown > this.attackSpeed)
            {
                this.playerObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * 1000);
                Debug.Log("ATTACKING PLAYER");
                this.attackCooldown = 0;
            }
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
    }
}