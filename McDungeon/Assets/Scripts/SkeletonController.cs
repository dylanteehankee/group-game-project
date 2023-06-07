using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class SkeletonController : MonoBehaviour, IMobController
    {
        [SerializeField]
        private GameObject playerObject;
        [SerializeField]
        private StatusEffects statusEffects;
        [SerializeField]
        private GameObject bonePrefab;
        [SerializeField]
        private float mobHealth = 15.0f;
        [SerializeField]
        private float attackRange = 5.0f;
        [SerializeField]
        private float attackSpeed = 1.0f;
        private float attackCD = 0.0f;
        private const float THROWDURATION = 0.8f;
        private float elapsedThrowTime = 0.0f;
        private bool isThrowing = false;
        private bool hasBone = true;
        private GameObject bone;
        [SerializeField]
        private float moveSpeed = 1.0f;
        private float speedModifier = 1.0f;
        [SerializeField]
        private float stunDuration = 0.5f;
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
                if ((Vector2.Distance(location, playerLocation) < this.attackRange && hasBone) || isThrowing)
                {
                    if (this.attackCD < this.attackSpeed)
                    {
                        this.attackCD += Time.deltaTime;
                        this.moveTowardPlayer(location, playerLocation);
                    }
                    else
                    {
                        this.attackPlayer(playerLocation - location);
                    }
                }
                else
                {
                    this.moveTowardPlayer(location, playerLocation);
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

        private void moveTowardPlayer(Vector2 location, Vector2 playerLocation)
        {
            var deltaLocation = playerLocation - location;
            if (!hasBone)
            {
                Vector2 boneLocation = this.bone.transform.position;
                deltaLocation = boneLocation - location;
            }
            deltaLocation.Normalize();
            this.transform.Translate(deltaLocation * Time.deltaTime * moveSpeed);
            this.spriteControl(deltaLocation);
        }

        private void attackPlayer(Vector2 deltaLocation)
        {
            if (this.elapsedThrowTime == 0)
            {
                this.transform.Translate(Vector2.zero);
                this.animator.SetTrigger("Attack");
                isThrowing = true;
            }
            else if (this.elapsedThrowTime > THROWDURATION)
            {
                this.isThrowing = false;
                this.elapsedThrowTime = 0;
                return;
            }
            else if (this.elapsedThrowTime > (THROWDURATION / 2) && hasBone)
            {
                deltaLocation.Normalize();
                Vector2 location = this.transform.position;
                this.bone = (GameObject)Instantiate(this.bonePrefab);
                this.bone.transform.position = location + deltaLocation;
                this.bone.GetComponent<BoneController>().Throw(this.playerObject.transform.position, this.gameObject);
                this.hasBone = false;
                Debug.Log("THROWING BONE");
            }
            this.elapsedThrowTime += Time.deltaTime;
        }

        private void spriteControl(Vector2 deltaLocation)
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
        
        public bool HasBone()
        {
            return hasBone;
        }

        public void GrabBone()
        {
            if (!hasBone)
            {
                Debug.Log("Pickup");
                this.hasBone = true;
                this.attackCD = 0.0f;
            }
        }

        public GameObject GetBone()
        {
            return this.bone;
        }

        public void Reassign(GameObject newBone)
        {
            if (!hasBone)
            {
                Debug.Log("Triggered");
                newBone.GetComponent<BoneController>().SetOwner(this.gameObject);
                this.bone = newBone;
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
                this.stunObject = statusEffects.Stun(this.transform, Vector2.one, new Vector2(0, 0.7f));
            }
            this.isThrowing = false;
            this.animator.SetBool("Stun", true);
            yield return new WaitForSeconds(stunDuration);
            this.animator.SetBool("Stun", false);
            this.elapsedThrowTime = 0;
            this.attackCD = Mathf.Min(this.attackCD, attackSpeed * 0.8f);
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
                        this.freezeObject = this.statusEffects.Freeze(this.transform, new Vector2(2, 1.6f), Vector2.zero);
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