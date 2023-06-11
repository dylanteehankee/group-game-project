using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;

namespace Mobs
{
    public class SlimeController : Mob
    {
        [SerializeField]
        public int MobDamage = 1;
        [SerializeField]
        private float attackSpeed = 1.0f;
        private float attackCD = 0.0f;
        private const float ATTACKDURATION = 1.0f;
        private float elapsedAttackTime = 0.0f;
        private bool isAttacking = false;
        private bool hitPlayer = false;

        void Start()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (!this.stunned && !this.isFreeze)
            {
                Vector2 location = this.transform.position;
                Vector2 playerLocation = this.playerObject.transform.position;
                if (this.attackCD < this.attackSpeed)
                {
                    this.attackCD += Time.deltaTime;
                }
                
                if (((Vector2.Distance(location, playerLocation) < this.attackRange) && (this.attackCD > this.attackSpeed)) || isAttacking)
                {
                    this.attackPlayer(playerLocation - location);
                }
                else
                {
                    this.moveTowardPlayer(playerLocation - location);
                }
            }
        }

        protected override void attackPlayer(Vector2 deltaLocation)
        {
            if (!isAttacking)
            {
                this.spriteRenderer.flipY = false;
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
            else if (this.elapsedAttackTime > 0.15 && !hitPlayer)
            {
                this.playerObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * 1000);
                Debug.Log("ATTACKING PLAYER");
                this.hitPlayer = true;
            }
            this.elapsedAttackTime += Time.deltaTime;
        }

        protected override void spriteControl(Vector2 deltaLocation)
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

        protected override IEnumerator stunStatus()
        {
            if (!this.stunObject)
            {
                this.stunObject = statusEffects.Stun(this.transform, Vector2.one, new Vector2(0, 0.5f));
            }
            this.isAttacking = false;
            this.animator.SetBool("Stun", true);
            this.spriteControl(this.playerObject.transform.position - this.transform.position);
            this.spriteRenderer.flipY = false;
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
    }
}