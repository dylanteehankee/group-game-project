using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;

namespace Mobs
{
    public class GNelfController : Mob
    {
        [SerializeField]
        private float attackSpeed = 1.0f;
        private float attackCD = 0.0f;
        [SerializeField]
        public int MobDamage = 1;

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

                if (Vector2.Distance(location, playerLocation) < this.attackRange && this.attackCD > this.attackSpeed)
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
            if (this.attackCD > this.attackSpeed)
            {
                this.playerObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * 1000);
                Debug.Log("HIT PLAYER");
                this.attackCD = 0;
            }
        }

        protected override IEnumerator stunStatus()
        {
            if (!this.stunObject)
            {
                this.stunObject = statusEffects.Stun(this.transform, Vector2.one, new Vector2(0, 0.25f));
            }
            this.animator.SetBool("Stun", true);
            yield return new WaitForSeconds(stunDuration);
            this.animator.SetBool("Stun", false);
            this.attackCD = Mathf.Min(this.attackCD, attackSpeed * 0.8f);
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
                        this.freezeObject = this.statusEffects.Freeze(this.transform, new Vector2(2, 1.5f), new Vector2(0, -0.1f));
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