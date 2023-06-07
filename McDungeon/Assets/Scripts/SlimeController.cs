using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class SlimeController : MonoBehaviour, IMobController
    {
        [SerializeField]
        private GameObject playerObject;
        [SerializeField]
        private StatusEffects statusEffects;
        [SerializeField]
        private float mobHealth = 10.0f;
        [SerializeField]
        private float attackRange = 1.0f;
        [SerializeField]
        private float attackSpeed = 1.0f;
        private float attackCD = 0.0f;
        private const float ATTACKDURATION = 1.0f;
        private float elapsedAttackTime = 0.0f;
        private bool isAttacking = false;
        private bool hitPlayer = false;
        [SerializeField]
        public int MobDamage = 1;
        [SerializeField]
        private float moveSpeed = 1.0f;
        private float speedModifier = 1.0f;
        [SerializeField]
        private float stunDuration = 1.0f;
        private bool stunned = false;
        private bool isAblaze = false;
        private bool isFreeze = false;
        private GameObject stunObject;
        private GameObject ablazeObject;
        private GameObject freezeObject;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

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
                this.attackCD += Time.deltaTime;
                if (((Vector2.Distance(location, playerLocation) < this.attackRange) && (this.attackCD > this.attackSpeed)) || isAttacking)
                {
                    this.attackPlayer(location, playerLocation);
                }
                else
                {
                    this.moveTowardPlayer(playerLocation - location);
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

        private void moveTowardPlayer(Vector2 deltaLocation)
        {
            deltaLocation.Normalize();
            this.transform.Translate(deltaLocation * Time.deltaTime * moveSpeed);
            this.spriteDirection(deltaLocation);
        }

        private void attackPlayer(Vector2 location, Vector2 playerLocation)
        {
            if (elapsedAttackTime == 0)
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
                var deltaLocation = playerLocation - location;
                this.playerObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * 1000);
                Debug.Log("ATTACKING PLAYER");
                this.hitPlayer = true;
            }
            this.elapsedAttackTime += Time.deltaTime;
        }

        private void spriteDirection(Vector2 deltaLocation)
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

        public void TakeDamage(float damage, EffectTypes type)
        {
            this.mobHealth -= damage;
            this.death();
            this.stunned = true;
            this.status(type);
            StopCoroutine("stunStatus");
            StartCoroutine("stunStatus");
        }

        private void death()
        {
            if (this.mobHealth <= 0)
            {
                statusEffects.Death(this.gameObject.transform.position, Vector2.one);
                Destroy(this.gameObject);
                return;
            }
        }

        private IEnumerator stunStatus()
        {
            if (!this.stunObject)
            {
                this.stunObject = statusEffects.Stun(this.transform, Vector2.one, new Vector2(0, 0.5f));
            }
            this.isAttacking = false;
            this.animator.SetBool("Stun", true);
            this.spriteDirection(this.playerObject.transform.position - this.transform.position);
            this.spriteRenderer.flipY = false;
            yield return new WaitForSeconds(stunDuration);
            this.animator.SetBool("Stun", false);
            this.attackCD = Mathf.Min(this.attackCD, attackSpeed * 0.8f);
            this.elapsedAttackTime = 0;
            this.stunned = false;
            Destroy(this.stunObject);
        }

        private void status(EffectTypes type)
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
        private IEnumerator ablazeStatus()
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

        private IEnumerator freezeStatus()
        {
            yield return new WaitForSeconds(this.statusEffects.GetFreezeDuration());
            this.isFreeze = false;
            this.animator.SetBool("Freeze", false);
            Destroy(this.freezeObject);
        }

        private IEnumerator slowStatus()
        {
            this.speedModifier = this.statusEffects.GetSlowModifier();
            yield return new WaitForSeconds(this.statusEffects.GetSlowDuration());
            this.speedModifier = 1.0f;
        }
    }
}