using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class MageController : Mob
    {
        [SerializeField]
        private GameObject fireballPrefab;
        [SerializeField]
        private GameObject frostboltPrefab;
        [SerializeField]
        private float[] castSpeed = {5.0f, 3.5f};
        private float castCD = -1.0f;
        [SerializeField]
        private float[] castTime = {2.0f, 1.5f};
        private float elapsedCastTime = 0.0f;
        private bool isCasting = false;
        private EffectTypes spellType = EffectTypes.None;
        private AudioSource[] audioSource;

        void Start()
        {
            this.difficulty = (int)gameSettings.GetDifficulty();
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.animator = this.GetComponent<Animator>();

            var mobSoundManager = GameObject.FindWithTag("MobSoundManager");
            audioSource = mobSoundManager.GetComponents<AudioSource>();
        }

        void Update()
        {
            if (!this.stunned && !this.isFreeze)
            {
                Vector2 location = this.transform.position;
                Vector2 playerLocation = this.playerObject.transform.position;
                if (this.castCD < this.castSpeed[difficulty])
                {
                    this.castCD += Time.deltaTime;
                }

                if ((Vector2.Distance(location, playerLocation) < this.attackRange && this.castCD > this.castSpeed[difficulty]) || isCasting)
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
            if (!isCasting)
            {
                Debug.Log("CASTING");
                this.animator.SetBool("Cast", true);
                audioSource[1].Play();
                if (Random.Range(0, 2) == 1)
                {
                    this.animator.SetTrigger("CastFire");
                    this.spellType = EffectTypes.Ablaze;
                    audioSource[6].Play();
                }
                else
                {
                    this.animator.SetTrigger("CastFrost");
                    this.spellType = EffectTypes.Freeze;
                    audioSource[7].Play();
                }
                isCasting = true;
            }
            else if (this.elapsedCastTime > this.castTime[difficulty])
            {
                Vector2 location = this.transform.position;
                GameObject spell;
                switch (this.spellType)
                {
                    case EffectTypes.Ablaze:
                        spell = (GameObject)Instantiate(this.fireballPrefab);
                        break;
                    case EffectTypes.Freeze:
                        spell = (GameObject)Instantiate(this.frostboltPrefab);
                        break;
                    default:
                        spell = (GameObject)Instantiate(this.fireballPrefab);
                        break;
                }
                spell.transform.position = location;
                spell.GetComponent<MageSpellController>().Cast(this.playerObject.transform.position, this.spellType, this.difficulty);
                this.isCasting = false;
                this.elapsedCastTime = 0;
                this.castCD = 0;
                this.animator.SetBool("Cast", false);
                return;
            }
            this.elapsedCastTime += Time.deltaTime;
        }

        protected override void spriteControl(Vector2 deltaLocation)
        {
            if (Mathf.Abs(deltaLocation.x) > Mathf.Abs(deltaLocation.y))
            {
                this.animator.SetInteger("Direction", 0);
                if (deltaLocation.x < 0)
                {
                    this.spriteRenderer.flipX = false;
                }
                else
                {
                    this.spriteRenderer.flipX = true;
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

        protected override IEnumerator stunStatus()
        {
            if (!this.stunObject)
            {
                this.stunObject = statusEffects.Stun(this.transform, Vector2.one, new Vector2(0, 0.5f));
            }
            this.animator.SetBool("Stun", true);
            yield return new WaitForSeconds(stunDuration);
            this.animator.SetBool("Stun", false);
            this.stunned = false;
            Destroy(this.stunObject);
        }

        protected override void status(EffectTypes type)
        {
            this.isCasting = false;
            this.animator.SetBool("Cast", false);
            this.elapsedCastTime = 0;
            this.castCD = 0;

            switch (type)
            {
                case EffectTypes.None:
                    this.stunned = true;
                    StopCoroutine("stunStatus");
                    StartCoroutine("stunStatus");
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