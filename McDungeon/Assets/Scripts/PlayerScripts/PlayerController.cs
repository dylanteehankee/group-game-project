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
        private PlayerInventory playerInventory;
        [SerializeField] private GameObject spellHome;
        [SerializeField] private GameObject Weapon;
        [SerializeField] private CRWeaponController closeRangeWeapon;
        [SerializeField] private float speed;
        [SerializeField] private CapsuleCollider2D bodyCollider;
        [SerializeField] private StartRoomLightController roomLightControl;
        [SerializeField] private CarpetKonamiController carpetLightControl;
        [SerializeField] protected int playerMaxHealth;
        [SerializeField] protected float playerHealth;
        private PlayerHealthController healthController;
        private float hitTakenInterverl;
        private float hitTimer;
        private bool readyForAction = true;

        private float speedModifier = 1.0f;

        private ISpellMaker spell_fire;
        private ISpellMaker spell_ice;
        private ISpellMaker spell_water;
        private ISpellMaker spell_lightning;
        private ISpellMaker spell_special;
        private char spell;

        private float actionCoolDown = 0f;
        private float atkCoolDown = 0.5f;

        private float fireCoolDown = 3f;
        private float waterCoolDown = 6f;
        private float iceCoolDown = 6f;
        private float lightningCoolDown = 6f;
        private float fireCoolDowntimer = 0f;
        private float waterCoolDowntimer = 0f;
        private float iceCoolDowntimer = 0f;
        private float lightningCoolDowntimer = 0f;

        private float fireCastDuration = 0.5f;
        private float waterCastDuration = 1f;
        private float iceCastDuration = 1f;
        private float lightningCastDuration = 1f;
        private float spellCastTimer = 0f;
        private bool castingSpell = false;
        private bool spellReady = false;
        private ParticleSystem spellChargeIndicator;
        private ParticleSystem spellReadyIndicator;


        private bool usingPortal = false;
        private bool reachedFront = false;
        private bool reachedInside = false;
        private float oldIntensity = 0.1f;
        private float lightIntensity = 0.1f;
        private Vector3 mirrorPos;
        private GameMode mode = GameMode.Normal;
        private bool isMcMode = false;

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
        private Color[] spellColor;

        private bool unlockingMcMirror = false;
        private bool unlockedMcCurtain = false;
        private bool resetedCamera = false;
        private float unlockMcMirrorTimer = 5f;

        private AudioSource[] bgAudioSource;


        void Awake()
        {
            playerInventory = new PlayerInventory(this);
        }

        void Start()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.animator = this.GetComponent<Animator>();

            spellHome = GameObject.Find("SpellMakerHome");
            healthController = GameObject.Find("PlayerHealth").GetComponent<PlayerHealthController>();
            spell_fire = spellHome.GetComponent<FireBallMaker>();
            spell_ice = spellHome.GetComponent<BlizzardMaker>();
            spell_water = spellHome.GetComponent<WaterSurgeMaker>();
            spell_lightning = spellHome.GetComponent<ThunderdMaker>();

            spell_special = spell_fire;
            spellChargeIndicator = this.transform.GetChild(3).GetComponent<ParticleSystem>();
            spellReadyIndicator = this.transform.GetChild(4).GetComponent<ParticleSystem>();
            spellReady = false;

            spellChargeIndicator.Stop();
            spellChargeIndicator.Clear();
            spellReadyIndicator.Stop();
            spellReadyIndicator.Clear();

            closeRangeWeapon = Weapon.transform.GetChild(0).gameObject.GetComponent<CRWeaponController>();
            closeRangeWeapon.Config(3f, 10f, 120f, 800f, true);

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
            carpetLightControl = GameObject.Find("Carpet").GetComponent<CarpetKonamiController>();

            playerMaxHealth = 10;
            playerHealth = 10f;
            healthController.ChangeMaxHealth(playerMaxHealth);
            healthController.SetNewHealth(playerHealth);

            spellColor = new Color[4];
            spellColor[0] = new Color(243f / 255f, 119f / 255f, 61f / 255f, 116f / 255f);
            spellColor[1] = new Color(86f / 255f, 126f / 255f, 210f / 255f, 116f / 255f);
            spellColor[2] = new Color(0.678f, 0.847f, 0.902f, 116f / 255f);
            spellColor[3] = new Color(166f / 255f, 50f / 255f, 215f / 255f, 116f / 255f);

            closeRangeWeapon.ChangeWeapon(0);

            var backgroundSoundManager = GameObject.FindWithTag("BGSoundManager");
            bgAudioSource = backgroundSoundManager.GetComponents<AudioSource>();
        }

        void FixedUpdate()
        {
            if (usingPortal)
            {
                usePortal();
            }
            else if (unlockingMcMirror)
            {
                // Time the effect
                if (unlockMcMirrorTimer > 0f)
                {
                    unlockMcMirrorTimer -= Time.deltaTime;
                }

                if (unlockMcMirrorTimer <= 3f && !unlockedMcCurtain)
                {
                    GameObject.Find("McMirror").transform.GetChild(2).GetComponent<McCurtainController>().Unlock();
                    unlockedMcCurtain = true;
                }

                if (unlockMcMirrorTimer <= 1f && !resetedCamera)
                {
                    GameObject.Find("Main Camera").GetComponent<PositionLockCamera>().changeCameraMode(CameraMode.LockOnPlayer, new Vector2(0f, 0f));
                    resetedCamera = true;
                }

                if (unlockMcMirrorTimer <= 0f)
                {
                    GameObject.Find("Main Camera").GetComponent<PositionLockCamera>().LockOnPlayer();
                    unlockingMcMirror = false;
                }
            }
            else
            {
                // move player
                if (!stunned && !isFreeze)
                {
                    Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
                    direction = direction.normalized;

                    this.gameObject.transform.Translate(direction * speed * speedModifier * Time.fixedDeltaTime);
                    this.spriteController(direction);
                }
            }
        }

        void Update()
        {
            if (GlobalStates.isPaused)
            {
                return;
            }
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
                else if (Input.GetButtonDown("Fire2") && fireCoolDowntimer <= 0f)
                {
                    spell_fire.Activate();
                    spellCastTimer = fireCastDuration;
                    ParticleSystem.MainModule chargeModule = spellChargeIndicator.main;
                    ParticleSystem.MainModule readyModule = spellReadyIndicator.main;
                    chargeModule.startColor = spellColor[0];
                    readyModule.startColor = spellColor[0];
                    spellChargeIndicator.Play();

                    spell = 'F';
                    castingSpell = true;
                    actionCoolDown = 100f; // Prevent other action
                }
                else if (Input.GetKey(KeyCode.E) && waterCoolDowntimer <= 0f)
                {
                    spell_water.Activate();
                    spellCastTimer = waterCastDuration;
                    ParticleSystem.MainModule chargeModule = spellChargeIndicator.main;
                    ParticleSystem.MainModule readyModule = spellReadyIndicator.main;
                    chargeModule.startColor = spellColor[1];
                    readyModule.startColor = spellColor[1];
                    spellChargeIndicator.Play();

                    spell = 'W';
                    castingSpell = true;
                    actionCoolDown = 100f; // Prevent other action
                }
                else if (Input.GetKey(KeyCode.Space) && iceCoolDowntimer <= 0f)
                {
                    spell_ice.Activate();
                    spellCastTimer = iceCastDuration;
                    ParticleSystem.MainModule chargeModule = spellChargeIndicator.main;
                    ParticleSystem.MainModule readyModule = spellReadyIndicator.main;
                    chargeModule.startColor = spellColor[2];
                    readyModule.startColor = spellColor[2];
                    spellChargeIndicator.Play();

                    spell = 'I';
                    castingSpell = true;
                    actionCoolDown = 100f; // Prevent other action
                }
                else if (Input.GetKey(KeyCode.Q) && lightningCoolDowntimer <= 0f)
                {
                    spell_lightning.Activate();
                    spellCastTimer = lightningCastDuration;
                    ParticleSystem.MainModule chargeModule = spellChargeIndicator.main;
                    ParticleSystem.MainModule readyModule = spellReadyIndicator.main;
                    chargeModule.startColor = spellColor[3];
                    readyModule.startColor = spellColor[3];
                    spellChargeIndicator.Play();

                    spell = 'L';
                    castingSpell = true;
                    actionCoolDown = 100f; // Prevent other action
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

        public void RestoreHealth(float hpToRestore)
        {
            playerHealth = Mathf.Min(hpToRestore + playerHealth, playerMaxHealth);
            healthController.SetNewHealth(playerHealth);
        }

        public void SyncWeaponWithInventory()
        {
            Weapon myWeapon = playerInventory.GetWeaponItem();
            if (myWeapon != null)
            {
                closeRangeWeapon.Config(
                    attackDamage: myWeapon.damage,
                    attackSpeed: myWeapon.attackSpeed * 10f, // How does this scale exactly? // == I made base attackspeed = 10f as "normal" and fair speed, 20f will be 2 times faster, 5f will be slower
                    attackAngle: myWeapon.attackAngle,
                    knockBack: myWeapon.knockBack * 100f,
                    true
                );
                closeRangeWeapon.ChangeWeapon(myWeapon.weaponSpriteID);
            }
            /*
            closeRangeWeapon.Config(
                attackDamage:3f, 
                attackSpeed:10f, 
                attackAngle: 120f,
                knockBack: 800f,
                true
            );
            */
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
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
                this.playerHealth -= damage;
                this.status(type);
                healthController.SetNewHealth(this.playerHealth);

                hitTimer = 0f;
            }

            checkDeath();
        }

        public void checkDeath()
        {
            if (playerHealth < 0f)
            {
                // Dead
                statusEffects.Death(this.gameObject.transform.position, Vector2.one);
            }
        }

        public PlayerInventory GetPlayerInventory()
        {
            return playerInventory;
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
                this.playerHealth -= (float)this.statusEffects.GetAblazeDamage();
                healthController.SetNewHealth(this.playerHealth);
                checkDeath();
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

            if (fireCoolDown > 0f)
            {
                fireCoolDowntimer -= Time.deltaTime;
            }

            if (waterCoolDown > 0f)
            {
                waterCoolDowntimer -= Time.deltaTime;
            }

            if (iceCoolDown > 0f)
            {
                iceCoolDowntimer -= Time.deltaTime;
            }

            if (lightningCoolDown > 0f)
            {
                lightningCoolDowntimer -= Time.deltaTime;
            }

        }

        private void castSpell(Vector3 mousePos)
        {
            if (spell == 'W' || spell == 'I' || spell == 'L')
            {
                ISpellMaker currentSpell = spell_water;
                KeyCode currentSpellKey = KeyCode.E;

                switch (spell)
                {
                    case 'W':
                        currentSpell = spell_water;
                        currentSpellKey = KeyCode.E;
                        break;
                    case 'I':
                        currentSpell = spell_ice;
                        currentSpellKey = KeyCode.Space;
                        break;
                    case 'L':
                        currentSpell = spell_lightning;
                        currentSpellKey = KeyCode.Q;
                        break;
                }


                // Spell Casting progress monitor.
                if (Input.GetKey(currentSpellKey) && spellCastTimer > 0f)
                {
                    currentSpell.ShowRange(this.transform.position, mousePos);
                    spellCastTimer -= Time.deltaTime;
                    Debug.Log("Spell casting");
                }
                else if (Input.GetKey(currentSpellKey))
                {
                    // Still aiming
                    currentSpell.ShowRange(this.transform.position, mousePos);
                }

                if (spellCastTimer <= 0f)
                {
                    // Show ready
                    if (!spellReady)
                    {
                        spellChargeIndicator.Stop();
                        spellChargeIndicator.Clear();
                        spellReadyIndicator.Play();
                        spellReady = true;
                        Debug.Log("Spell ready");
                    }
                }

                if (Input.GetKeyUp(currentSpellKey))
                {
                    if (spellCastTimer <= 0f)
                    {
                        currentSpell.Execute(this.transform.position, mousePos);
                        setCD(spell);
                        Debug.Log("Spell Casted");
                    }
                    else
                    {
                        Debug.Log("Spell Cancelled");
                    }

                    spellChargeIndicator.Stop();
                    spellChargeIndicator.Clear();
                    spellReadyIndicator.Stop();
                    spellReadyIndicator.Clear();

                    spellReady = false;
                    actionCoolDown = 0.1f;
                    castingSpell = false;
                }
            }
            else
            {
                // Spell Casting progress monitor.
                if (Input.GetButton("Fire2") && spellCastTimer > 0f)
                {
                    spell_special.ShowRange(this.transform.position, mousePos);
                    spellCastTimer -= Time.deltaTime;
                    Debug.Log("Spell casting");
                }
                else if (Input.GetButton("Fire2"))
                {
                    // Still aiming
                    spell_special.ShowRange(this.transform.position, mousePos);
                }

                if (spellCastTimer <= 0f)
                {
                    // Show ready
                    if (!spellReady)
                    {
                        spellChargeIndicator.Stop();
                        spellChargeIndicator.Clear();
                        spellReadyIndicator.Play();
                        spellReady = true;
                        Debug.Log("Spell ready");
                    }
                }

                if (Input.GetButtonUp("Fire2"))
                {
                    if (spellCastTimer <= 0f)
                    {
                        spell_special.Execute(this.transform.position, mousePos);
                        setCD(spell);
                        Debug.Log("Spell Casted");
                    }
                    else
                    {
                        Debug.Log("Spell Cancelled");
                    }

                    spellChargeIndicator.Stop();
                    spellChargeIndicator.Clear();
                    spellReadyIndicator.Stop();
                    spellReadyIndicator.Clear();

                    spellReady = false;
                    actionCoolDown = 0.1f;
                    castingSpell = false;
                }

            }
        }

        private void setCD(char spell)
        {
            switch (spell)
            {
                case 'F':
                    fireCoolDowntimer = fireCoolDown;
                    break;
                case 'W':
                    waterCoolDowntimer = waterCoolDown;
                    break;
                case 'I':
                    iceCoolDowntimer = iceCoolDown;
                    break;
                case 'L':
                    lightningCoolDowntimer = lightningCoolDown;
                    break;
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
                    carpetLightControl.ChangeLight(roomLightIntensity);
                    changeToMcMode();
                }
                else
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * speedModifier * Time.deltaTime;
                    this.spriteController(direction);

                    globalLight.intensity = globalLight.intensity - speed * speedModifier * Time.deltaTime * oldIntensity;
                    torchLight.intensity = torchLight.intensity - speed * speedModifier * Time.deltaTime * 0.5f;
                    roomLightIntensity = roomLightIntensity - speed * speedModifier * Time.deltaTime * 1f;
                    roomLightControl.UpdateLight(roomLightIntensity);
                    carpetLightControl.ChangeLight(roomLightIntensity);
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
                    carpetLightControl.ChangeLight(roomLightIntensity);
                }
                else
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * speedModifier * Time.deltaTime;
                    this.spriteController(direction);

                    globalLight.intensity = globalLight.intensity + speed * speedModifier * Time.deltaTime * lightIntensity / 2.5f;
                    torchLight.intensity = torchLight.intensity + speed * speedModifier * Time.deltaTime * 0.5f / 2.5f;
                    roomLightIntensity = roomLightIntensity + speed * speedModifier * Time.deltaTime * 1f / 2.5f;
                    roomLightControl.UpdateLight(roomLightIntensity);
                    carpetLightControl.ChangeLight(roomLightIntensity);

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

        private void changeToMcMode()
        {
            // Change Back Ground sound
            if (isMcMode)
            {
                bgAudioSource[2].enabled = true;
                bgAudioSource[0].enabled = false;
                if (!bgAudioSource[2].isPlaying)
                {
                    bgAudioSource[2].Play();
                }
            }
            else
            {
                bgAudioSource[0].enabled = true;
                bgAudioSource[2].enabled = false;
                if (!bgAudioSource[0].isPlaying)
                {
                    bgAudioSource[0].Play();
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
                isMcMode = !isMcMode;
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

        public void UnlockingMcMirror()
        {
            unlockingMcMirror = true;

            GameObject.Find("Main Camera").GetComponent<PositionLockCamera>().changeCameraMode(CameraMode.MoveToTarget, new Vector2(4.4f, 3.7f));
        }
    
        public void ResetPlayer()
        {
            
        }
    }
}
