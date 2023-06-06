using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class GNelfController : MonoBehaviour, IMobController
    {
        [SerializeField]
        private float mobHealth = 4.0f;
        [SerializeField]
        private float attackRange = 1f;
        [SerializeField]
        private float attackSpeed = 1f;
        private float attackCooldown = 0f;
        [SerializeField]
        public int MobDamage = 1;
        [SerializeField]
        private float moveSpeed = 2f;
        [SerializeField]
        private float hitStun = 1f;
        private float elapsedStun = 0f;
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
            if (this.elapsedStun < hitStun)
            {
                this.elapsedStun += Time.deltaTime;
            }
            else if (Vector2.Distance(location, playerLocation) < this.attackRange)
            {
                this.attackPlayer(playerLocation - location);
            }
            else
            {
                this.moveTowardPlayer(playerLocation - location);
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

        private void moveTowardPlayer(Vector2 deltaLocation)
        {
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
            this.mobHealth -= damage;
            if (this.mobHealth < 0)
            {
                Destroy(this.gameObject);
                return;
            }
            this.elapsedStun = 0;
        }
    }
}