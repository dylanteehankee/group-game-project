using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public abstract class Mob : MonoBehaviour, IMobController
    {

        [SerializeField]
        protected GameObject playerObject;
        [SerializeField]
        protected StatusEffects statusEffects;
        [SerializeField]
        protected float mobHealth;
        [SerializeField]
        protected float attackRange;
        [SerializeField]
        protected float moveSpeed;
        protected float speedModifier = 1.0f;
        private float knockbackDuration = 1.0f;
        protected float stunDuration = 1.0f;
        protected bool stunned = false;
        protected bool isAblaze = false;
        protected bool isFreeze = false;
        protected GameObject stunObject;
        protected GameObject ablazeObject;
        protected GameObject freezeObject;
        protected SpriteRenderer spriteRenderer;
        protected Animator animator;


        protected abstract void attackPlayer(Vector2 deltaLocation);
        protected abstract IEnumerator stunStatus();
        protected abstract void status(EffectTypes type);

        void OnDestroy()
        {
            this.transform.parent.gameObject.GetComponent<MobManager>().Unsubscribe(this.gameObject);
        }

        public void GetPlayer(GameObject player)
        {
            this.playerObject = player;
        }

        protected virtual void moveTowardPlayer(Vector2 deltaLocation)
        {
            deltaLocation.Normalize();
            this.transform.Translate(deltaLocation * Time.deltaTime * moveSpeed);
            this.spriteControl(deltaLocation);
        }

        protected virtual void spriteControl(Vector2 deltaLocation)
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

        public virtual void TakeDamage(float damage, EffectTypes type)
        {
            this.mobHealth -= damage;
            this.death();
            this.stunned = true;
            this.status(type);
            StopCoroutine(stunStatus());
            StartCoroutine(stunStatus());
        }

        protected IEnumerator knockback(Rigidbody2D player)
        {
            yield return new WaitForSeconds(knockbackDuration);
            player.isKinematic = true;
        }

        protected void death()
        {
            if (this.mobHealth <= 0)
            {
                statusEffects.Death(this.gameObject.transform.position, Vector2.one);
                Destroy(this.gameObject);
                return;
            }
        }

        protected IEnumerator ablazeStatus()
        {
            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(this.statusEffects.GetAblazeDuration() / 4);
                this.mobHealth -= this.statusEffects.GetAblazeDamage();
                this.death();
            }
            this.isAblaze = false;
            Destroy(this.ablazeObject);
        }


        protected IEnumerator freezeStatus()
        {
            yield return new WaitForSeconds(this.statusEffects.GetFreezeDuration());
            this.isFreeze = false;
            this.animator.SetBool("Freeze", false);
            Destroy(this.freezeObject);
        }

        protected IEnumerator slowStatus()
        {
            this.speedModifier = this.statusEffects.GetSlowModifier();
            yield return new WaitForSeconds(this.statusEffects.GetSlowDuration());
            this.speedModifier = 1.0f;
        }
    }
}