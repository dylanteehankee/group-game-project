using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum GameMode
{
    Normal,
    Hard,
    Unchange
}

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
        [SerializeField] private CapsuleCollider2D bodyCollider;
        [SerializeField] private StartRoomLightController roomLightControl;
        [SerializeField] private int health = 10;
        private float hitTakenInterverl;
        private float hitTimer;
        private bool readyForAction = true;

        private float speedModifier = 1.0f;

        private ISpellMaker spell_fire;
        private ISpellMaker spell_ice;
        private ISpellMaker spell_water;
        private ISpellMaker spell_lightning;
        private ISpellMaker spell_Q;
        private ISpellMaker spell_E;
        private char spell;

        private float actionCoolDown = 0f;
        private float atkCoolDown = 0.5f;

        private float spellQCoolDown = 3f;
        private float spellQCoolDowntimer = 0f;
        private float spellECoolDown = 3f;
        private float spellECoolDowntimer = 0f;

        private float spellCastDuration = 0.5f;
        private float spellCastTimer = 0f;
        private bool castingSpell = false;


        private bool usingPortal = false;
        private bool reachedFront = false;
        private bool reachedInside = false;
        private float oldIntensity = 0.1f;
        private float lightIntensity = 0.1f;
        private Vector3 mirrorPos;
        private GameMode mode = GameMode.Normal;

        private Light2D globalLight;
        private Light2D torchLight;
        private float roomLightIntensity = 1f;

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
            spell_fire = spellHome.GetComponent<FireBallMaker>();
            spell_ice = spellHome.GetComponent<BlizzardMaker>();
            spell_water = spellHome.GetComponent<WaterSurgeMaker>();
            spell_lightning = spellHome.GetComponent<ThunderdMaker>();

            spell_Q = spell_fire;
            spell_E = spell_lightning;

            closeRangeWeapon = Weapon.transform.GetChild(0).gameObject.GetComponent<CRWeaponController>();
            closeRangeWeapon.Config(10f, 10f, 120f, 1f, true);

            hitTakenInterverl = 0.2f; // 0.2 sec
            /*
            GameObject.Find("GameManager").GetComponent<UIManager>().GenerateTextBubble(
                this.gameObject.transform,
                text:  "Lots of random text, this has gotta suck if this does not wrap around. Light up all the torches to win.",
                dimensions: new Vector3(10, 2, 0), 
                offset: new Vector3(-5, 3, 0), 
                fontSize: 3.5f,
                duration: 10.0f      
            );
            */
            bodyCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
            
            globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
            torchLight = this.transform.GetChild(0).gameObject.GetComponent<Light2D>();
            roomLightControl = GameObject.Find("StartingRoom").GetComponent<StartRoomLightController>();
        }

        void FixedUpdate()
        {
            if (!usingPortal)
            {
                if (!stunned && !isFreeze)
                {
                    Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
                    direction = direction.normalized;

                    this.gameObject.transform.Translate(direction * speed * speedModifier * Time.fixedDeltaTime);
                    this.spriteController(direction);
                }
            }
            else
            {
                usePortal();
            }
        }

        void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // move player

            // Reduce all coll down count.
            updateCoolDowns();

            // Manage Action
            if (actionCoolDown <= 0f)
            {
                // Read input to determine next action.
                if (Input.GetButtonDown("Fire1"))
                {
                    closeRangeWeapon.SetActive(true);
                    actionCoolDown = atkCoolDown;
                }
                else if (Input.GetKey(KeyCode.Q) && spellQCoolDowntimer <= 0f)
                {
                    spell_Q.Activate();
                    castingSpell = true;
                    spellCastTimer = spellCastDuration;
                    actionCoolDown = 100f;
                    spell = 'Q';
                }
                else if (Input.GetKey(KeyCode.E) && spellECoolDowntimer <= 0f)
                {
                    spell_E.Activate();
                    castingSpell = true;
                    spellCastTimer = spellCastDuration;
                    actionCoolDown = 100f;
                    spell = 'E';
                }
            }

            if (castingSpell)
            {
                castSpell(mousePos);
            }

            // Hit timer management.
            if (hitTimer < hitTakenInterverl)
            {
                hitTimer += Time.deltaTime;
            }
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

        public void TakeDamage(int damage, EffectTypes type)
        {
            if (hitTimer > hitTakenInterverl)
            {
                this.health -= damage;
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
                        this.ablazeObject = this.statusEffects.Ablaze(this.transform, new Vector2(1, 3), new Vector2(0, 1.75f));
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
                this.health -= (int)this.statusEffects.GetAblazeDamage();
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

        private void updateCoolDowns()
        {
            if (actionCoolDown > 0f)
            {
                actionCoolDown -= Time.deltaTime;
            }

            if (spellQCoolDowntimer > 0f)
            {
                spellQCoolDowntimer -= Time.deltaTime;
            }

            if (spellECoolDowntimer > 0f)
            {
                spellECoolDowntimer -= Time.deltaTime;
            }
        }

        private void castSpell(Vector3 mousePos)
        {
            if (spell == 'Q')
            {
                // Spell Casting progress monitor.
                if (Input.GetKey(KeyCode.Q) && spellCastTimer > 0f)
                {
                    spell_Q.ShowRange(this.transform.position, mousePos);
                    spellCastTimer -= Time.deltaTime;
                    Debug.Log("Spell Q casting");
                }

                if (spellCastTimer <= 0f)
                {
                    // Show ready
                    Debug.Log("Spell Q ready");
                }

                if (Input.GetKeyUp(KeyCode.Q))
                {
                    if (spellCastTimer <= 0f)
                    {
                        spell_Q.Execute(this.transform.position, mousePos);
                        actionCoolDown = 0.1f;
                        spellQCoolDowntimer = spellQCoolDown;
                        Debug.Log("Spell Q Casted");
                    }
                    else
                    {
                        actionCoolDown = 0.1f;
                        Debug.Log("Spell Q Cancelled");
                    }

                    castingSpell = false;
                }
            }
            else if (spell == 'E')
            {
                // Spell Casting progress monitor.
                if (Input.GetKey(KeyCode.E) && spellCastTimer > 0f)
                {
                    spell_E.ShowRange(this.transform.position, mousePos);
                    spellCastTimer -= Time.deltaTime;
                    Debug.Log("Spell E casting");
                }

                if (spellCastTimer <= 0f)
                {
                    // Show ready
                    Debug.Log("Spell E ready");
                }

                if (Input.GetKeyUp(KeyCode.E))
                {
                    if (spellCastTimer <= 0f)
                    {
                        spell_E.Execute(this.transform.position, mousePos);
                        actionCoolDown = 0.1f;
                        spellECoolDowntimer = spellECoolDown;
                        Debug.Log("Spell E Casted");
                    }
                    else
                    {
                        actionCoolDown = 0.1f;
                        Debug.Log("Spell E Cancelled");
                    }

                    castingSpell = false;
                }
            }
        }

        private void usePortal()
        {
            if (!reachedFront)
            {
                Vector3 direction = mirrorPos - this.gameObject.transform.position + new Vector3(0f, -0.5f, 0f);
                direction.z = 0f;
                if (direction.magnitude < 0.1f)
                {
                    this.gameObject.transform.position = mirrorPos + new Vector3(0f, -0.5f, 0f);
                    reachedFront = true;
                    Debug.Log("Done reached front");
                }
                else
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * speedModifier * Time.deltaTime;
                    this.spriteController(direction);
                }
            }
            else if (!reachedInside)
            {
                Vector3 direction = mirrorPos - this.gameObject.transform.position + new Vector3(0f, 0.5f, 0f);
                direction.z = 0f;
                if (direction.magnitude < 0.1f)
                {
                    this.gameObject.transform.position = mirrorPos + new Vector3(0f, 0.5f, 0f);
                    reachedInside = true;
                    globalLight.intensity = 0f;
                    torchLight.intensity = 0f;
                    roomLightIntensity = 0f;
                    roomLightControl.UpdateLight(roomLightIntensity);
                }
                else
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * speedModifier * Time.deltaTime;
                    this.spriteController(direction);

                    globalLight.intensity = globalLight.intensity - speed * speedModifier * Time.deltaTime * oldIntensity;
                    torchLight.intensity = torchLight.intensity - speed * speedModifier * Time.deltaTime * 0.5f;
                    roomLightIntensity = roomLightIntensity - speed * speedModifier * Time.deltaTime * 1f;
                    roomLightControl.UpdateLight(roomLightIntensity);
                }
            }
            else
            {
                Vector3 direction = mirrorPos - this.gameObject.transform.position + new Vector3(0f, -2f, 0f);
                direction.z = 0f;
                if (direction.magnitude < 0.1f)
                {
                    this.gameObject.transform.position = mirrorPos + new Vector3(0f, -2f, 0f);

                    bodyCollider.isTrigger = false;
                    actionCoolDown = 0f;
                    usingPortal = false;
                    speedModifier = 1f;
                    globalLight.intensity = lightIntensity;
                    torchLight.intensity = 0.5f;
                    roomLightIntensity = 1f;
                    roomLightControl.UpdateLight(roomLightIntensity);
                }
                else
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * speedModifier * Time.deltaTime;
                    this.spriteController(direction);

                    globalLight.intensity = globalLight.intensity + speed * speedModifier * Time.deltaTime * lightIntensity / 2.5f;
                    torchLight.intensity = torchLight.intensity + speed * speedModifier * Time.deltaTime * 0.5f / 2.5f;
                    roomLightIntensity = roomLightIntensity + speed * speedModifier * Time.deltaTime * 1f / 2.5f;
                    roomLightControl.UpdateLight(roomLightIntensity);

                    if (globalLight.intensity > lightIntensity)
                    {
                        globalLight.intensity = lightIntensity;
                    }

                    if (torchLight.intensity > 0.5f)
                    {
                        torchLight.intensity = 0.5f;
                    }

                    if (roomLightIntensity > 1f)
                    {
                        roomLightIntensity = 1f;
                        roomLightControl.UpdateLight(roomLightIntensity);
                    }
                }

            }

        }

        public void StartUsePortal(Vector3 mirrorPos, GameMode mode = GameMode.Normal)
        {
            this.mirrorPos = mirrorPos;
            this.mode = mode;
            bodyCollider.isTrigger = true;

            if (mode == GameMode.Unchange)
            {
                lightIntensity = lightIntensity;
            }
            else if (mode == GameMode.Normal)
            {
                lightIntensity = 0.1f;
            }
            else
            {
                lightIntensity = 0f;
            }

            oldIntensity = globalLight.intensity;

            actionCoolDown = 100f;
            usingPortal = true;
            reachedFront = false;
            reachedInside = false;
            speedModifier = 0.7f;
        }
    }
}
