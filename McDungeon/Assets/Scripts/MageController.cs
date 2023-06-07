using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class MageController : MonoBehaviour, IMobController
    {
        [SerializeField]
        private float mobHealth = 10;
        [SerializeField]
        private int attackRange = 3;
        [SerializeField]
        private float castTime = 1f;
        private float elapsedCast = 0f;
        [SerializeField]
        public int MobDamage = 1;
        [SerializeField]
        private float moveSpeed = 1f;
        [SerializeField]
        private float hitStun = 0.25f;
        private float elapsedStun = 0f;

        private GameObject playerObject;
        [SerializeField]
        private GameObject potionDropPrefab;

        void Update()
        {
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
            if (Vector2.Distance(position, playerLocation) < this.attackRange || this.elapsedCast > 0)
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
            if (this.elapsedCast > castTime)
            {
                if (Random.Range(0,2) == 1)
                {
                    Debug.Log("CAST PYROBLAST");
                }
                else
                {
                    Debug.Log("CAST FROSTBOLT");
                }
                
                this.elapsedCast = 0;
            }
            else
            {
                this.elapsedCast += Time.deltaTime;
            }
        }   

        public void TakeDamage(float damage, EffectTypes type)
        {
            this.elapsedStun = 0;
            this.elapsedCast = 0;
            this.mobHealth -= damage;
            if (this.mobHealth < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}