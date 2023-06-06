using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class SlimeController : MonoBehaviour, IMobController
    {
        [SerializeField]
        private float mobHealth = 10;
        [SerializeField]
        private int attackRange = 1;
        [SerializeField]
        private float attackSpeed = 1f;
        private float attackCooldown = 0f;
        [SerializeField]
        public int MobDamage = 1;
        [SerializeField]
        private float moveSpeed = 1f;
        [SerializeField]
        private float hitStun = 1f;
        private float elapsedStun = 0f;
        private float attackTime = 1.0f;
        private float elapsedAttackTime = 0.0f;
        private bool isAttacking = false;
        private bool hitPlayer = false;
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
            if (this.elapsedStun < hitStun)
            {
                this.elapsedStun += Time.deltaTime;
            }
            else if (((Vector2.Distance(location, playerLocation) < this.attackRange) && (this.attackCooldown > this.attackSpeed)) || isAttacking)
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
            if (elapsedAttackTime == 0)
            {
                this.spriteRenderer.flipY = false;
                this.transform.Translate(Vector2.zero);
                this.animator.SetTrigger("Attack");
                this.isAttacking = true;
                this.hitPlayer = false;
            }
            else if (this.elapsedAttackTime > this.attackTime)
            {
                this.isAttacking = false;
                this.elapsedAttackTime = 0;
                this.attackCooldown = 0;
                return;
            }
            else if (this.elapsedAttackTime > 0.15 && !hitPlayer)
            {
                this.playerObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * 1000);
                Debug.Log("ATTACKING PLAYER");
                this.hitPlayer = true;
            }
            this.elapsedAttackTime += Time.deltaTime;
        }

        private void spriteDirection(Vector2 deltaLocation)
        {
            if (Mathf.Abs(deltaLocation.x) > Mathf.Abs(deltaLocation.y))
            {
                this.animator.SetBool("Direction", true);
                this.spriteRenderer.flipY = false;
                if (deltaLocation.x < 0)
                {
                    this.spriteRenderer.flipX = true;
                }
                else
                {
                    this.spriteRenderer.flipX = false;
                }
            }
            else
            {
                this.animator.SetBool("Direction", false);
                this.spriteRenderer.flipX = false;
                if (deltaLocation.y > 0)
                {
                    this.spriteRenderer.flipY = true;
                }
                else
                {
                    this.spriteRenderer.flipY = false;
                }
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
            this.isAttacking = false;
            this.elapsedAttackTime = 0;
            this.attackCooldown = 0;
        }
    }
}