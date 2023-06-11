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
        [SerializeField]
        public int MobDamage = 2;
        [SerializeField]
        private float moveSpeed = 1.5f;
        [SerializeField]
        private float hitStun = 1.5f;
        private float stunDelayTime = 0f;
        private float attackCooldown = 0f;
        private bool active = false;
        private GameObject playerObject;
        [SerializeField]
        private GameObject potionDropPrefab;
        [SerializeField]
        private GameObject swordDropPrefab;

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
            if (Vector2.Distance(position, playerLocation) < this.attackRange)
            {
                this.transform.Translate(Vector2.zero);
                this.attackPlayer();
            }
            else
            {

                this.transform.Translate(deltaLocation * Time.deltaTime * moveSpeed);
            }
        }

        private void attackPlayer()
        {
            if (this.attackCooldown > this.attackSpeed)
            {
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