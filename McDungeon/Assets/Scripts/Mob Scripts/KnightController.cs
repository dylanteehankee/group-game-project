using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;

namespace Mobs
{
    public class KnightController : Mob
    {
        [SerializeField]
        private GameObject shieldPrefab;
        [SerializeField]
        public int MobDamage = 2;
        [SerializeField]
        private float attackSpeed = 3.0f;
        private float attackCD = 0.0f;
        private const float ATTACKDURATION = 0.8f;
        private float elapsedAttackTime = 0.0f;
        private bool isAttacking = false;
        private bool hitPlayer = false;
        private float shieldCooldown = 10.0f;
        private float elapsedShieldCD = 0.0f;
        private bool shieldObject;
        private bool active = false;
        [SerializeField]
        private GameObject swordDropPrefab;

        void Start()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.animator = this.GetComponent<Animator>();
        }

        void Update()
        {
            if (this.active && !this.stunned && !this.isFreeze)
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
                Rigidbody2D playerRigidbody = this.playerObject.GetComponent<Rigidbody2D>();
                playerRigidbody.isKinematic = false;
                this.playerObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * 1000);
                Debug.Log("ATTACKING PLAYER");
                this.hitPlayer = true;
            }
            this.elapsedAttackTime += Time.deltaTime;
        }

        public override void TakeDamage(float damage, EffectTypes type)
        {
            if (this.active)
            {
                if (shieldObject)
                {
                    shieldObject = false;
                    elapsedShieldCD = 0;
                }
                else
                {
                    this.mobHealth -= damage;
                    this.death();
                    this.status(type);
                    this.stunned = true;
                    StopCoroutine(stunStatus());
                    StartCoroutine(stunStatus());
                }
            }
        }

        protected override IEnumerator stunStatus()
        {
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
            switch (type)
            {
                case EffectTypes.None:
                    break;
                case EffectTypes.Ablaze:
                    if (!this.ablazeObject)
                    {
                        this.ablazeObject = this.statusEffects.Ablaze(this.transform, Vector2.one, Vector2.zero);
                        this.isAblaze = true;
                    }
                    StopCoroutine("ablazeStatus");
                    StartCoroutine("ablazeStatus");
                    break;
                case EffectTypes.Freeze:
                    if (!this.freezeObject)
                    {
                        this.freezeObject = this.statusEffects.Freeze(this.transform, new Vector2(3, 1.5f), Vector2.zero);
                        this.animator.SetBool("Freeze", true);
                        this.isFreeze = true;
                    }
                    StopCoroutine("freezeStatus");
                    StartCoroutine("freezeStatus");
                    break;
                case EffectTypes.Slow:
                    StopCoroutine("slowStatus");
                    StartCoroutine("slowStatus");
                    break;
            }
        }

        public void ActivateKnight()
        {
            this.active = true;
            this.animator.SetBool("Active", true);
        }

        public void DestroyKnight()
        {
            Destroy(this.gameObject);
        }

        private void getShield()
        {
            if (!shieldObject)
            {
                if (elapsedShieldCD >= shieldCooldown)
                {
                    Debug.Log("GET SHIELD");
                    shieldObject = true;
                }
                else
                {
                    elapsedShieldCD += Time.deltaTime;
                }
            }
        }
    }
}