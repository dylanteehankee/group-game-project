using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class GNomeController : MonoBehaviour, IMobController
    {
        [SerializeField]
        private float mobHealth = 20;
        [SerializeField]
        private int attackRange = 5;
        [SerializeField]
        private float attackSpeed = 2f;
        [SerializeField]
        private float castSpeed = 10f;
        [SerializeField]
        private float castTime = 4f;
        [SerializeField]
        private float moveSpeed = 1f;
        [SerializeField]
        private float hitStun = 0.25f;
        private float elapsedStun = 0f;
        private float attackCooldown = 0f;
        private float castCooldown = 0f;
        private float castDelayTime = 0f;
        private float throwTime = 0.8f;
        private float elapsedThrowTime = 0f;
        private bool isThrowing = false;
        private bool hasKnife = false;
        private bool isCasting = false;
        [SerializeField]
        private GameObject playerObject;
        [SerializeField]
        private GameObject gNelfPrefab;
        [SerializeField]
        private GameObject potionDropPrefab;
        [SerializeField]
        private GameObject knifePrefab;
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

            this.attackCooldown += Time.deltaTime;
            this.castCooldown += Time.deltaTime;
            if (this.elapsedStun < hitStun)
            {
                this.elapsedStun += Time.deltaTime;
            }
            else if (!isThrowing && (this.castCooldown > this.castSpeed || isCasting))
            {
                this.casting();
            }
            else if((Vector2.Distance(location, playerLocation) < this.attackRange && this.attackCooldown > this.attackSpeed) || isThrowing)
            {
                this.throwKnife(playerLocation - location);
            }
            else
            {
                moveTowardPlayer(playerLocation - location);
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

        private void throwKnife(Vector2 deltaLocation)
        {
            if (this.elapsedThrowTime == 0)
            {
                this.transform.Translate(Vector2.zero);
                this.animator.SetTrigger("Attack");
                isThrowing = true;
                hasKnife = true;
            }
            else if (this.elapsedThrowTime > this.throwTime)
            {
                this.isThrowing = false;
                this.elapsedThrowTime = 0;
                this.attackCooldown = 0;
                return;
            }
            else if (this.elapsedThrowTime > 0.6 && hasKnife)
            {
                deltaLocation.Normalize();
                Vector2 location = this.transform.position;
                var knife = (GameObject)Instantiate(this.knifePrefab);
                knife.transform.position = location;
                knife.GetComponent<KnifeController>().Throw(this.playerObject.transform.position);
                this.hasKnife = false;
                Debug.Log("THROWING KNIFE");
            }
            this.elapsedThrowTime += Time.deltaTime;
        }

        private void casting()
        {
            if (!isCasting)
            {
                Debug.Log("CASTING");
                this.animator.SetTrigger("Cast");
                isCasting = true;
            }
            else if (this.castDelayTime > this.castTime)
            {
                var spawner = this.transform.parent.gameObject.GetComponent<MobManager>();
                spawner.SpawnGNelfs(this.gNelfPrefab, this.transform.position);
                this.castDelayTime = 0;
                this.castCooldown = 0;
                this.isCasting = false;
                this.animator.SetTrigger("Cast");
                return;
            }

            this.castDelayTime += Time.deltaTime;
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
            this.isThrowing = false;
            this.elapsedThrowTime = 0;
            this.attackCooldown = 0;
            this.castDelayTime = 0;
            this.castCooldown = 0;
            this.isCasting = false;
        }
    }
}
