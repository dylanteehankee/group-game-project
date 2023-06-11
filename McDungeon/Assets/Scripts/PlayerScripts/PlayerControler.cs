using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        protected StatusEffects statusEffects;
        [SerializeField] private GameObject spellHome;
        [SerializeField] private GameObject Weapon;
        [SerializeField] private CRWeaponController closeRangeWeapon;
        [SerializeField] private float speed;
        private float hitTakenInterverl;
        private float hitTimer;

        private float speedModifier = 1.0f;

        private ISpellMaker spell_1;
        [SerializeField] private GameObject prefab_fireball;

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

            spellHome = GameObject.Find("SpellMakerHome");
            spell_1 = spellHome.GetComponent<FireBallMaker>();

            closeRangeWeapon = Weapon.transform.GetChild(0).gameObject.GetComponent<CRWeaponController>();
            closeRangeWeapon.Config(10f, 120f, true);

            hitTakenInterverl = 0.2f; // 0.2 sec

            GameObject.Find("GameManager").GetComponent<UIManager>().GenerateTextBubble(
                this.gameObject.transform,
                text:  "Lots of random text, this has gotta suck if this does not wrap around. Light up all the torches to win.",
                dimensions: new Vector3(10, 2, 0), 
                offset: new Vector3(-5, 3, 0), 
                fontSize: 3.5f,
                duration: 10.0f      
            );
        }


        void FixedUpdate()
        {
            // move player
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            direction = direction.normalized;
            this.transform.Translate(direction * Time.fixedDeltaTime * speedModifier * speed);
            
            this.spriteController(direction);
            //this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * speedModifier * Time.fixedDeltaTime;
        }

        void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                spell_1.Activate();
            }
            if (Input.GetKey(KeyCode.Q))
            {
                spell_1.ShowRange(this.transform.position, mousePos);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                GameObject spellInstance = spell_1.Execute(this.transform.position, mousePos);
            }

            // Hit timer management.
            if (hitTimer < hitTakenInterverl)
            {
                hitTimer += Time.deltaTime;
            }
        }

        void Shoot()
        {
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision Enter: " + collision.gameObject.name);


            if (collision.gameObject.tag == "None")
            {
                Debug.Log("Collision Enter: " + collision.gameObject.name);
            }

            // Perform actions or logic when the collision occurs
        }

        public void TakeDamage(float damage, EffectTypes type)
        {
            if (hitTimer > hitTakenInterverl)
            {
                this.status(type);
                hitTimer = 0f;
            }
        }

        private void spriteController(Vector2 direction)
        {
            this.animator.SetBool("Idle", false);
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                this.animator.SetInteger("Direction", 0);
                if (direction.x < 0)
                {
                    this.spriteRenderer.flipX = true;
                }
                else
                {
                    this.spriteRenderer.flipX = false;
                }
            }
            else if (direction.y < 0)
            {
                this.animator.SetInteger("Direction", -1);
            }
            else if (direction.y > 0)
            {
                this.animator.SetInteger("Direction", 1);
            }
            else
            {
                this.animator.SetBool("Idle", true);
            }
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
                // this.health -= this.statusEffects.GetAblazeDamage();
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
