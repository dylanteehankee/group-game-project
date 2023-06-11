using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class GNomeController : Mob
    {
        [SerializeField]
        private GameObject knifePrefab;
        [SerializeField]
        private GameObject gNelfPrefab;
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
            if (this.elapsedThrowTime == 0)
            {
                this.spriteControl(deltaLocation);
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

        protected override IEnumerator stunStatus()
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
