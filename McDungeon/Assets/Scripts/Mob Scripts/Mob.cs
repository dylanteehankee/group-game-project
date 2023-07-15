using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public abstract class Mob : MonoBehaviour, IMobController
    {
        public ItemFactory itemFactory;
        [SerializeField]
        protected GameObject playerObject;
        [SerializeField]
        protected StatusEffects statusEffects;
        [SerializeField]
        protected GameSettings gameSettings;
        protected int difficulty;
        [SerializeField]
        protected float mobHealth;
        [SerializeField]
        protected float attackRange;
        [SerializeField]
        protected float moveSpeed;
        [SerializeField]
        protected int itemTier;
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
            this.status(type);
            
            StartCoroutine("hitConfirm");
        }

        protected void death()
        {
            if (this.mobHealth <= 0)
            {
                this.itemDrops();
                statusEffects.Death(this.gameObject.transform.position, Vector2.one);
                Destroy(this.gameObject);
            }
        }

        private IEnumerator hitConfirm()
        {
            for (int i = 0; i < 2; i++)
            {
                yield return new WaitForSeconds(0.15f);
                this.spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.15f);
                this.spriteRenderer.color = Color.white;
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

        protected virtual void itemDrops()
        {            
            var hpCount = 0;
            var hpChance = Random.Range(0f, 1f);
            
            var itemChance = Random.Range(0f, 1f);
            if (hpChance >= 0.5)
            {
                if (hpChance >= 0.8)
                    hpCount = 2;
                else 
                    hpCount = 1;
                Debug.Log(hpCount);
                itemFactory.DropHealthPotions(hpCount, this.gameObject.transform.position);
            }
            if (itemChance >= 0.8)
            {
                itemFactory.DropEquipmentItemFromTier(itemTier, this.gameObject.transform.position);
            }
        }
    }
}