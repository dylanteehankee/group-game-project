using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace McDungeon
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        protected StatusEffects statusEffects;
        private PlayerInventory playerInventory;
        private Weapon myWeapon;
        [SerializeField] private GameObject spellHome;
        [SerializeField] private GameObject Weapon;
        [SerializeField] private CRWeaponController closeRangeWeapon;
        [SerializeField] private float speed;
        [SerializeField] private CapsuleCollider2D bodyCollider;
        [SerializeField] private StartRoomLightController roomLightControl;
        [SerializeField] private CarpetKonamiController carpetLightControl;
        [SerializeField] protected int playerMaxHealth = 10;
        [SerializeField] protected float playerHealth = 10f;
        [SerializeField] private GameSettings gameSettings;
        private PlayerHealthController healthController;
        private float hitCD = 0.2f;
        private float hitTimer;

        private float speedModifier = 1.0f;

        private ISpellMaker spell_fire;
        private ISpellMaker spell_ice;
        private ISpellMaker spell_water;
        private ISpellMaker spell_lightning;
        private char spell;

        private float attackElapsedCD = 0f;
        private float attackCD = 0.5f;

        private float fireCD;
        private float firePuzzleCD = 2f;
        private float fireCombatCD = 8f;
        private float waterCD = 12f;
        private float iceCD = 12f;
        private float lightningCD = 12f;
        private bool[] spellReadyArray = {true, true, true, true};
        private bool castingSpell = false;
        private ParticleSystem spellReadyIndicator;
        private ParticleSystem spellCastIndicator;


        private bool usingPortal = false;
        private bool reachedFront = false;
        private bool reachedInside = false;
        private float oldIntensity = 0.1f;
        private float lightIntensity = 0.1f;
        private Vector3 mirrorPos;
        [SerializeField] private bool isMcMode = false;

        private Light2D globalLight;
        private Light2D torchLight;
        private float roomLightIntensity = 1f;

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

        private AudioSource[] audioSource;
        private AudioSource[] bgAudioSource;
        private AudioSource[] specialAudioSource;

        private bool playerDead = false;

        private SpriteRenderer[] spellReadyIcon;

        private UIManager uiManager;

        public GameObject gameManager;

        private bool finishedStart = false;
        

        [SerializeField] public RuntimeAnimatorController playerController;
        [SerializeField] public RuntimeAnimatorController mcController;
        private Animator playerAnimator;

        [SerializeField] private GameObject prefabTA;

        void Start()
        {
            playerInventory = new PlayerInventory(this);
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.animator = this.GetComponent<Animator>();

            fireCD = fireCombatCD;
            spellHome = GameObject.Find("SpellMakerHome");
            healthController = GameObject.Find("PlayerHealth").GetComponent<PlayerHealthController>();
            spell_fire = spellHome.GetComponent<FireBallMaker>();
            spell_ice = spellHome.GetComponent<BlizzardMaker>();
            spell_water = spellHome.GetComponent<WaterSurgeMaker>();
            spell_lightning = spellHome.GetComponent<ThunderdMaker>();

            spellReadyIndicator = this.transform.GetChild(3).GetComponent<ParticleSystem>();
            spellCastIndicator = this.transform.GetChild(4).GetComponent<ParticleSystem>();

            spellReadyIndicator.Stop();
            spellReadyIndicator.Clear();
            spellCastIndicator.Stop();
            spellCastIndicator.Clear();

            closeRangeWeapon = Weapon.transform.GetChild(0).gameObject.GetComponent<CRWeaponController>();
            closeRangeWeapon.Config(3f, 10f, 120f, 800f, true);
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

            healthController.ChangeMaxHealth(playerMaxHealth);
            healthController.SetNewHealth(playerHealth);

            spellColor = new Color[4];
            spellColor[0] = new Color(243f / 255f, 119f / 255f, 61f / 255f, 116f / 255f);
            spellColor[1] = new Color(86f / 255f, 126f / 255f, 210f / 255f, 116f / 255f);
            spellColor[2] = new Color(0.678f, 0.847f, 0.902f, 116f / 255f);
            spellColor[3] = new Color(166f / 255f, 50f / 255f, 215f / 255f, 116f / 255f);


            var mobSoundManager = GameObject.FindWithTag("MobSoundManager");
            audioSource = mobSoundManager.GetComponents<AudioSource>();
            var backgroundSoundManager = GameObject.FindWithTag("BGSoundManager");
            bgAudioSource = backgroundSoundManager.GetComponents<AudioSource>();
            var specialSoundManager = GameObject.FindWithTag("SpecialSoundManager");
            specialAudioSource = specialSoundManager.GetComponents<AudioSource>();

            uiManager = gameManager.GetComponent<UIManager>();

            spellReadyIcon = new SpriteRenderer[4];
            spellReadyIcon[0] = GameObject.Find("CoolDownReady").transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            spellReadyIcon[1] = GameObject.Find("CoolDownReady").transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            spellReadyIcon[2] = GameObject.Find("CoolDownReady").transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            spellReadyIcon[3] = GameObject.Find("CoolDownReady").transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>();
            finishedStart = true;

            playerAnimator = this.gameObject.GetComponent<Animator>();

            this.gameSettings.SetDifficulty(GameMode.Normal);
        }

        void FixedUpdate()
        {
            if (!finishedStart || playerDead)
            {
                return;
            }
            else if (usingPortal)
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
            else if (!isFreeze)
            {
                Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
                direction = direction.normalized;

                this.gameObject.transform.Translate(direction * speed * speedModifier * Time.fixedDeltaTime);
                if (isMcMode)
                {
                    this.mcSpriteController(direction);
                }
                else
                {
                    this.spriteController(direction);
                }
            }
        }

        void Update()
        {
            if (!finishedStart || GlobalStates.isPaused || playerDead)
            {
                return;
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if (attackElapsedCD >= 0f)
            {
                attackElapsedCD -= Time.deltaTime;
            }
            // Hit timer management.
            if (hitTimer <= hitCD)
            {
                hitTimer += Time.deltaTime;
            } 

            // Manage Action
            if (attackElapsedCD <= 0f && !castingSpell && !usingPortal && !isFreeze)
            {
                // Read input to determine next action.
                
                if (Input.GetButtonDown("Fire1") && myWeapon != null)
                {
                    closeRangeWeapon.SetActive(true);
                    attackElapsedCD = attackCD;
                    audioSource[5].Play();
                }
                else if (Input.GetKey(KeyCode.Alpha1) && spellReadyArray[0])
                {
                    spell_fire.Activate();
                    ParticleSystem.MainModule readyModule = spellCastIndicator.main;
                    readyModule.startColor = spellColor[0];
                    spellCastIndicator.Play();

                    spell = 'F';
                    castingSpell = true;
                    audioSource[6].Play();
                }
                else if (Input.GetKey(KeyCode.Alpha3) && spellReadyArray[1])
                {
                    spell_water.Activate();
                    ParticleSystem.MainModule readyModule = spellCastIndicator.main;
                    readyModule.startColor = spellColor[1];
                    spellCastIndicator.Play();

                    spell = 'W';
                    castingSpell = true;
                    audioSource[10].Play();
                }
                else if (Input.GetKey(KeyCode.Alpha4) && spellReadyArray[2])
                {
                    spell_ice.Activate();
                    ParticleSystem.MainModule readyModule = spellCastIndicator.main;
                    readyModule.startColor = spellColor[2];
                    spellCastIndicator.Play();

                    spell = 'I';
                    castingSpell = true;
                    audioSource[7].Play();
                }
                else if (Input.GetKey(KeyCode.Alpha2) && spellReadyArray[3])
                {
                    spell_lightning.Activate();
                    ParticleSystem.MainModule readyModule = spellCastIndicator.main;
                    readyModule.startColor = spellColor[3];
                    spellCastIndicator.Play();

                    spell = 'L';
                    castingSpell = true;
                    audioSource[11].Play();
                }
            }

            if (castingSpell)
            {
                castSpell(mousePos);
            }
        }

        public void RestoreHealth(float hpToRestore)
        {
            playerHealth = Mathf.Min(hpToRestore + playerHealth, playerMaxHealth);
            healthController.SetNewHealth(playerHealth);
        }

        public void SyncWeaponWithInventory()
        {
            myWeapon = playerInventory.GetWeaponItem();
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
            else 
            {
                closeRangeWeapon.NoWeapon();
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

        public void TakeDamage(int damage, EffectTypes type)
        {
            if (hitTimer > hitCD)
            {
                this.playerHealth -= damage;
                this.status(type);
                StartCoroutine("hitConfirm");
                healthController.SetNewHealth(this.playerHealth);

                hitTimer = 0f;
                audioSource[8].Play();
            }

            checkDeath();
        }

        public void checkDeath()
        {
            if (playerHealth <= 0f)
            {
                // Dead
                statusEffects.Death(this.gameObject.transform.position, Vector2.one * 2f);
                playerDead = true;
                audioSource[9].Play();
                uiManager.GameOver();
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

        private void mcSpriteController(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                this.animator.SetBool("Idle", false);
                if (direction.x < 0)
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

        private void castSpell(Vector3 mousePos)
        {
            ISpellMaker currSpell;
            KeyCode currKey;

            switch (spell)
                {
                    case 'F':
                        currSpell = spell_fire;
                        currKey = KeyCode.Alpha1;
                        break;
                    case 'W':
                        currSpell = spell_water;
                        currKey = KeyCode.Alpha3;
                        break;
                    case 'I':
                        currSpell = spell_ice;
                        currKey = KeyCode.Alpha4;
                        break;
                    case 'L':
                        currSpell = spell_lightning;
                        currKey = KeyCode.Alpha2;
                        break;
                    default:
                        currSpell = spell_fire;
                        currKey = KeyCode.Alpha1;
                        break;
                }

            // Spell aiming
            if (Input.GetKey(currKey))
            {
                currSpell.ShowRange(this.transform.position, mousePos);
            }

            else if (Input.GetKeyUp(currKey))
            {
                if (isMcMode && spell == 'I')
                {
                    GameObject taSpell = Instantiate(prefabTA);
                    Vector3 taPos = this.transform.position;

                    Vector3 distanceVec = (mousePos - this.transform.position);
                    distanceVec.z = 0f;
                    Vector3 spellDir = distanceVec.normalized;
                    float distance = distanceVec.magnitude;

                    if (distance > 4f)
                    {
                        distance = 4f;
                    }

                    taPos = this.transform.position + spellDir * distance;

                    taSpell.transform.position = taPos;

                    specialAudioSource[Random.Range(0,2)].Play();
                }
                else
                {
                    currSpell.Execute(this.transform.position, mousePos);
                }
                setCD(spell);
                spellCastIndicator.Stop();
                spellCastIndicator.Clear();

                attackElapsedCD = 0.5f;
                castingSpell = false;
            }
        }

        private void setCD(char spell)
        {
            switch (spell)
            {
                case 'F':
                    StartCoroutine(spellCD(fireCD, 0));
                    break;
                case 'W':
                    StartCoroutine(spellCD(waterCD, 1));
                    break;
                case 'I':
                    StartCoroutine(spellCD(iceCD, 2));
                    break;
                case 'L':
                    StartCoroutine(spellCD(lightningCD, 3));
                    break;
            }
        }

        private IEnumerator spellCD(float CD, int spellID)
        {
            spellReadyArray[spellID] = false;
            yield return new WaitForSeconds(CD - 0.5f);
            ParticleSystem.MainModule spellReady = spellReadyIndicator.main;
            spellReady.startColor = spellColor[spellID];
            spellReadyIndicator.Play();
            yield return new WaitForSeconds(0.5f);
            spellReadyIndicator.Stop();
            spellReadyIndicator.Clear();
            spellReadyArray[spellID] = true;
            spellReadyIcon[spellID].enabled = true;
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
                }
                else
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * speedModifier * Time.deltaTime;
                    if (isMcMode)
                    {
                        this.mcSpriteController(direction);
                    }
                    else
                    {
                        this.spriteController(direction);
                    }
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
                    if (isMcMode)
                    {
                        this.mcSpriteController(direction);
                    }
                    else
                    {
                        this.spriteController(direction);
                    }

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
                    attackElapsedCD = 0f;
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
                    if (isMcMode)
                    {
                        this.mcSpriteController(direction);
                    }
                    else
                    {
                        this.spriteController(direction);
                    }

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

                playerAnimator.runtimeAnimatorController = mcController;

            }
            else
            {
                bgAudioSource[0].enabled = true;
                bgAudioSource[2].enabled = false;
                if (!bgAudioSource[0].isPlaying)
                {
                    bgAudioSource[0].Play();
                }

                playerAnimator.runtimeAnimatorController = playerController;
            }
        }

        public void StartUsePortal(Vector3 mirrorPos, GameMode mode = GameMode.Normal)
        {
            this.mirrorPos = mirrorPos;
            bodyCollider.isTrigger = true;
            if (mode == GameMode.Special){
                isMcMode = !isMcMode;
            }
            else
            {
                this.gameSettings.SetDifficulty(mode);
                if (mode == GameMode.Normal)
                {
                    lightIntensity = 0.1f;
                }
                else if (mode == GameMode.Hard)
                {
                    lightIntensity = 0f;
                }
            }
            

            oldIntensity = globalLight.intensity;

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

        public void PlayerEnterPuzzle()
        {
            fireCD = firePuzzleCD;
            if ( globalLight.intensity == 0f)
            {
                globalLight.intensity = 0.1f;
            }
        }

        public void PlayerLeavePuzzle()
        {
            fireCD = fireCombatCD;
            globalLight.intensity = lightIntensity;
        }
    }
}
