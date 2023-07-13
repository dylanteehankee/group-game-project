using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class SkeletonController : Mob
    {
        [SerializeField]
        private GameObject bonePrefab;
        [SerializeField]
        private float attackSpeed = 1.0f;
        private float attackCD = -1.0f;
        private const float THROWDURATION = 1.0f;
        private float elapsedThrowTime = 0.0f;
        private bool isThrowing = false;
        private bool hasBone = true;
        private bool threwBone = false;
        private GameObject bone;

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

        protected override void attackPlayer(Vector2 deltaLocation)
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
                this.hasBone = false;
                this.threwBone = false;
                return;
            }
            else if (this.elapsedThrowTime > (THROWDURATION / 2) && !threwBone)
            {
                deltaLocation.Normalize();
                Vector2 location = this.transform.position;
                this.bone = (GameObject)Instantiate(this.bonePrefab);
                this.bone.transform.position = location + deltaLocation;
                this.bone.GetComponent<BoneController>().Throw(this.playerObject.transform.position, this.gameObject);
                this.threwBone = true;
                Debug.Log("THROWING BONE");
            }
            this.elapsedThrowTime += Time.deltaTime;
        }

        public virtual void TakeDamage(float damage, EffectTypes type)
        {
            this.mobHealth -= damage;
            if (this.mobHealth >= 0)
            {
                Destroy(this.bone);
            }
            this.death();
            this.stunned = true;
            this.status(type);
            StopCoroutine(stunStatus());
            StartCoroutine(stunStatus());
            StartCoroutine("hitConfirm");
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

        protected override IEnumerator stunStatus()
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
    }
}