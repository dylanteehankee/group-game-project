using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class GNomeController : MonoBehaviour, IMobController
    {
        [SerializeField]
        private GameObject playerObject;
        [SerializeField]
        private StatusEffects statusEffects;
        [SerializeField]
        private GameObject knifePrefab;
        [SerializeField]
        private GameObject gNelfPrefab;
        [SerializeField]
        private float mobHealth = 20.0f;
        [SerializeField]
        private float attackRange = 5.0f;
        [SerializeField]
        private float attackSpeed = 2.0f;
        private float attackCD = 0.0f;
        private const float THROWDURATION = 0.8f;
        private float elapsedThrowTime = 0.0f;
        private bool isThrowing = false;
        private bool hasKnife = false;
        private bool isCasting = false;
        [SerializeField]
        private float castSpeed = 10f;
        private float castCD = 0.0f;
        [SerializeField]
        private float castTime = 3.6f;
        private float elapsedCastTime = 0.0f;
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
            this.animator = this.GetComponent<Animator>();
        }

        void Update()
        {
            if (!this.stunned && !this.isFreeze)
            {
                Vector2 location = this.transform.position;
                Vector2 playerLocation = this.playerObject.transform.position;
                this.attackCD += Time.deltaTime;
                this.castCD += Time.deltaTime;
                if (!isThrowing && (this.castCD > castSpeed || isCasting))
                {
                    this.casting();
                }
                else if((Vector2.Distance(location, playerLocation) < this.attackRange && this.attackCD > this.attackSpeed) || isThrowing)
                {
                    this.throwKnife(playerLocation - location);
                }
                else
                {
                    moveTowardPlayer(playerLocation - location);
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
            this.transform.Translate(deltaLocation * Time.deltaTime * moveSpeed * speedModifier);
            this.spriteDirection(deltaLocation);
        }

        private void throwKnife(Vector2 deltaLocation)
        {
            if (this.elapsedThrowTime == 0)
            {
                this.spriteDirection(deltaLocation);
                this.transform.Translate(Vector2.zero);
                this.animator.SetTrigger("Attack");
                isThrowing = true;
                hasKnife = true;
            }
            else if (this.elapsedThrowTime > THROWDURATION)
            {
                this.isThrowing = false;
                this.elapsedThrowTime = 0;
                this.attackCD = 0;
                return;
            }
            else if (this.elapsedThrowTime > 0.6 && hasKnife)
            {
                deltaLocation.Normalize();
                Vector2 location = this.transform.position;
                var knife = (GameObject)Instantiate(this.knifePrefab);
                knife.transform.position = location;
                knife.GetComponent<KnifeController>().Throw(this.playerObject.transform.position);
                this.hasKnife = false;
                Debug.Log("THROWING KNIFE");
            }
            this.elapsedThrowTime += Time.deltaTime;
        }

        private void casting()
        {
            if (!isCasting)
            {
                Debug.Log("CASTING");
                this.animator.SetBool("Cast", true);
                isCasting = true;
            }
            else if (this.elapsedCastTime > castTime)
            {
                var spawner = this.transform.parent.gameObject.GetComponent<MobManager>();
                spawner.SpawnGNelfs(this.gNelfPrefab, this.transform.position);
                this.elapsedCastTime = 0;
                this.castCD = 0;
                this.isCasting = false;
                this.animator.SetBool("Cast", false);
                return;
            }

            this.elapsedCastTime += Time.deltaTime;
        }

        private void spriteDirection(Vector2 deltaLocation)
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
                this.stunObject = statusEffects.Stun(this.transform, Vector2.one, new Vector2 (0, 0.5f));
            }
            this.isThrowing = false;
            this.isCasting = false;
            this.animator.SetBool("Cast", false);
            this.animator.SetBool("Stun", true);
            yield return new WaitForSeconds(stunDuration);
            this.animator.SetBool("Stun", false);
            this.elapsedThrowTime = 0;
            this.attackCD = Mathf.Min(this.attackCD, attackSpeed * 0.8f);
            this.elapsedCastTime = 0;
            this.castCD = 0;
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
