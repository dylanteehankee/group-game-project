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
        private float throwTime = 0.8f;
        private bool hasBone = true;
        private bool isThrowing = false;
        [SerializeField]
        private GameObject playerObject;
        [SerializeField]
        private GameObject potionDropPrefab;
        [SerializeField]
        private GameObject bonePrefab;
        private GameObject bone;
        private float elapsedThrowCD = 0f;
        private float elapsedThrowTime = 0f;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        void Start()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.animator = this.GetComponent<Animator>();
        }

        void Update()
        {
            Vector2 location = this.transform.position;
            Vector2 playerLocation = this.playerObject.transform.position;
            
            if (this.stunDelayTime < hitStun)
            {
                this.stunDelayTime += Time.deltaTime;
            }
            else if ((Vector2.Distance(location, playerLocation) < this.attackRange && hasBone) || isThrowing)
            {
                if (this.elapsedThrowCD < this.throwCooldown)
                {
                    this.elapsedThrowCD += Time.deltaTime;
                    this.moveTowardPlayer(location, playerLocation);
                }
                else
                {
                    this.attackPlayer(playerLocation - location);
                }
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
            if (!hasBone)
            {
                Vector2 boneLocation = this.bone.transform.position;
                deltaLocation = boneLocation - location;
            }
            deltaLocation.Normalize();
            this.transform.Translate(deltaLocation * Time.deltaTime * moveSpeed);
            this.spriteControl(deltaLocation);
        }

        private void attackPlayer(Vector2 deltaLocation)
        {
            if (this.elapsedThrowTime == 0)
            {
                this.transform.Translate(Vector2.zero);
                this.animator.SetTrigger("Attack");
                isThrowing = true;
            }
            else if (this.elapsedThrowTime > (this.throwTime / 2) && hasBone)
            {
                deltaLocation.Normalize();
                Vector2 location = this.transform.position;
                this.bone = (GameObject)Instantiate(this.bonePrefab);
                this.bone.transform.position = location + deltaLocation;
                this.bone.GetComponent<BoneController>().Throw(this.playerObject.transform.position);
                this.hasBone = false;
                Debug.Log("THROWING BONE");
            }
            else if (this.elapsedThrowTime > this.throwTime)
            {
                this.isThrowing = false;
                this.elapsedThrowTime = 0;
                return;
            }
            this.elapsedThrowTime += Time.deltaTime;
        }

        private void spriteControl(Vector2 deltaLocation)
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