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
        [SerializeField]
        private GameObject playerObject;
        [SerializeField]
        private GameObject potionDropPrefab;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        void Start()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.animator = GetComponent<Animator>();
        }

        void Update()
        {
            Vector2 location = this.transform.position;
            Vector2 playerLocation = this.playerObject.transform.position;

            this.attackCooldown += Time.deltaTime;
            if (this.stunDelayTime < hitStun)
            {
                this.stunDelayTime += Time.deltaTime;
            }
            else if (Vector2.Distance(location, playerLocation) < this.attackRange)
            {
                this.attackPlayer(playerLocation - location);
            }
            else
            {
                this.moveTowardPlayer(location, playerLocation);
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

        private void moveTowardPlayer(Vector2 location, Vector2 playerLocation)
        {
            var deltaLocation = playerLocation - location;
            deltaLocation.Normalize();
            this.transform.Translate(deltaLocation * Time.deltaTime * moveSpeed);
            this.spriteDirection(deltaLocation);
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

        private void spriteDirection(Vector2 deltaLocation)
        {
            if (Mathf.Abs(deltaLocation.x) > Mathf.Abs(deltaLocation.y))
            {
                this.animator.SetInteger("Direction", 0);
                if (deltaLocation.x < 0)
                {
                    this.spriteRenderer.flipX = true;
                }
                else
                {
                    this.spriteRenderer.flipX = false;
                }
            }
            else if (deltaLocation.y < 0)
            {
                this.animator.SetInteger("Direction", -1);
            }
            else
            {
                this.animator.SetInteger("Direction", 1);
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