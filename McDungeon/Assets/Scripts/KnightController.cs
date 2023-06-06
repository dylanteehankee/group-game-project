using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class KnightController : MonoBehaviour, IMobController
    {
        [SerializeField]
        private float mobHealth = 20;
        [SerializeField]
        private int attackRange = 2;
        [SerializeField]
        private float attackSpeed = 2f;
        private float attackCooldown = 0f;
        [SerializeField]
        public int MobDamage = 2;
        [SerializeField]
        private float moveSpeed = 1.5f;
        [SerializeField]
        private float hitStun = 1.5f;
        private float elapsedStun = 0f;
        private float shieldCooldown = 5f;
        private float elapsedShieldCD = 0f;
        private bool hasShield = true;
        private bool active = false;
        private GameObject playerObject;
        [SerializeField]
        private GameObject potionDropPrefab;
        [SerializeField]
        private GameObject swordDropPrefab;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        // Delete Start when implemented
        void Start()
        {
            GameObject[] playerObjects;
            playerObjects = GameObject.FindGameObjectsWithTag("PlayerHitbox");
            if (playerObjects.Length == 0)
            {
                Debug.Log("Player not found.");
            }
            else
            {
                this.playerObject = playerObjects[0];
            }
        }

        void Update()
        {
            if (this.active)
            {
                elapsedShieldCD += Time.deltaTime;
                if (elapsedShieldCD > shieldCooldown)
                {
                    hasShield = true;
                }

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
                    moveTowardPlayer(location, playerLocation);
                }
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
            // this.spriteDirection(deltaLocation);
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
            if (hasShield)
            {
                hasShield = false;
                elapsedShieldCD = 0;
            }
            else
            {
                this.elapsedStun = 0;
                this.mobHealth -= damage;
                if (this.mobHealth < 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        public void ActivateKnight()
        {
            this.active = true;
        }

        public void DestroyKnight()
        {
            Destroy(this.gameObject);
        }
    }
}