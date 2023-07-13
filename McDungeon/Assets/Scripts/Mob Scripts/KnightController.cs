using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class KnightController : Mob
    {
        [SerializeField]
        private int damage = 2;
        [SerializeField]
        private float attackSpeed = 3.0f;
        private float attackCD = -1.0f;
        private const float ATTACKDURATION = 0.8f;
        private float elapsedAttackTime = 0.0f;
        private bool isAttacking = false;
        private bool hitPlayer = false;
        private float shieldCooldown = 3.0f;
        private float elapsedShieldCD = 2.9f;
        private bool hasShield = false;
        [SerializeField]
        private GameObject shieldObject;
        private bool active = false;
        [SerializeField]
        private GameObject swordDropPrefab;
        private AudioSource[] audioSource;

        void Start()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.animator = this.GetComponent<Animator>();
            this.audioSource = this.GetComponents<AudioSource>();
        }

        void Update()
        {
            if (this.active)
            {
                this.getShield();
                Vector2 location = this.transform.position;
                Vector2 playerLocation = this.playerObject.transform.position;
                if (this.attackCD < this.attackSpeed)
                {
                    this.attackCD += Time.deltaTime;
                }
                if ((Vector2.Distance(location, playerLocation) < this.attackRange && this.attackCD > this.attackSpeed) || isAttacking)
                {
                    this.attackPlayer(playerLocation - location);
                }
                else 
                {
                    moveTowardPlayer(playerLocation - location);
                }
            }
        }

        protected override void attackPlayer(Vector2 deltaLocation)
        {
            if (!isAttacking)
            {
                this.transform.Translate(Vector2.zero);
                this.animator.SetTrigger("Attack");
                this.isAttacking = true;
                this.hitPlayer = false;

                audioSource[0].Play();
            }
            else if (this.elapsedAttackTime > ATTACKDURATION)
            {
                this.isAttacking = false;
                this.elapsedAttackTime = 0;
                this.attackCD = 0;
                return;
            }
            else if (this.elapsedAttackTime > ATTACKDURATION / 2 && !hitPlayer)
            {
                Vector2 location = this.transform.position;
                Vector2 playerLocation = this.playerObject.transform.position;
                if (Vector2.Distance(location, playerLocation) < this.attackRange)
                {
                    this.playerObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * 1000);
                    this.playerObject.GetComponent<PlayerController>().TakeDamage(damage, EffectTypes.None);
                    this.hitPlayer = true;
                }
            }
            this.elapsedAttackTime += Time.deltaTime;
        }

        public override void TakeDamage(float damage, EffectTypes type)
        {
            if (this.active)
            {
                if (hasShield)
                {
                    hasShield = false;
                    this.shieldObject.SetActive(false);
                    elapsedShieldCD = 0;
                }
                else
                {
                    this.mobHealth -= damage;
                    this.death();
                    StartCoroutine("hitConfirm");
                }
            }
        }

        protected override IEnumerator stunStatus()
        {
            // DO NOTHING
            if (!this.stunObject)
            {
                this.stunObject = statusEffects.Stun(this.transform, Vector2.one, new Vector2(0, 0.5f));
            }
            this.isAttacking = false;
            this.animator.SetBool("Stun", true);
            yield return new WaitForSeconds(stunDuration);
            this.animator.SetBool("Stun", false);
            this.attackCD = Mathf.Min(this.attackCD, attackSpeed * 0.8f);
            this.elapsedAttackTime = 0;
            this.stunned = false;
            Destroy(this.stunObject);
        }

        protected override void status(EffectTypes type)
        {
            // DO NOTHING
        }

        public void ActivateKnight()
        {
            this.active = true;
            this.GetComponent<Rigidbody2D>().isKinematic = false;
            this.animator.SetBool("Active", true);
        }

        public void DestroyKnight()
        {
            Destroy(this.gameObject);
        }

        private void getShield()
        {
            if (!hasShield)
            {
                if (elapsedShieldCD >= shieldCooldown)
                {
                    this.shieldObject.SetActive(true);
                    hasShield = true;
                }
                else
                {
                    elapsedShieldCD += Time.deltaTime;
                }
            }
        }
    }
}