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
        private float attackSpeed = 1f;
        [SerializeField]
        private float castSpeed = 5f;
        [SerializeField]
        private float castTime = 2f;
        [SerializeField]
        public int MobDamage = 2;
        [SerializeField]
        private float moveSpeed = 1f;
        [SerializeField]
        private float hitStun = 0.25f;
        private float elapsedStun = 0f;
        private float attackCooldown = 0f;
        private float castCooldown = 0f;
        private float castDelayTime = 0f;
        private GameObject playerObject;
        [SerializeField]
        private GameObject gNelfPrefab;
        [SerializeField]
        private GameObject potionDropPrefab;

        void Update()
        {
            this.attackCooldown += Time.deltaTime;
            this.castCooldown += Time.deltaTime;
            if (this.elapsedStun < hitStun)
            {
                this.elapsedStun += Time.deltaTime;
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
            if (Vector2.Distance(position, playerLocation) < this.attackRange || this.castDelayTime > 0)
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
            if (this.attackCooldown > this.attackSpeed && this.castDelayTime == 0)
            {
                Debug.Log("ATTACKING PLAYER");
                this.attackCooldown = 0;
            }
            else if (this.castCooldown > this.castSpeed || this.castDelayTime > 0)
            {
                if (this.castDelayTime > this.castTime)
                {
                    var spawner = this.transform.parent.gameObject.GetComponent<MobManager>();
                    var newGNelf = (GameObject)Instantiate(this.gNelfPrefab, spawner.transform);
                    spawner.Subscribe(newGNelf);
                    newGNelf.transform.position = new Vector3(this.transform.position.x + Random.Range(-1, 1), this.transform.position.y + Random.Range(-1, 1), 0);
                    spawner.Notify();
                    this.castDelayTime = 0;
                    this.castCooldown = 0;
                }
                else
                {
                    this.castDelayTime += Time.deltaTime;
                }
            }
        }

        public void TakeDamage(float damage)
        {
            this.elapsedStun = 0;
            this.castDelayTime = 0;
            this.mobHealth -= damage;
            if (this.mobHealth < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
