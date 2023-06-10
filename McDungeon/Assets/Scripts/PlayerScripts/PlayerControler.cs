using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

enum Effect
{
    None,
    Slow,
    Frezze,
    Burn
}

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
        [SerializeField] private GameObject spellHome;
        [SerializeField] private GameObject weapon;
        [SerializeField] private CRWeaponController closeRangeWeapon;
        [SerializeField] private float speed;
        [SerializeField] private BoxCollider2D bodyCollider;
        private float hitTakenInterverl;
        private float hitTimer;
        private bool readyForAction = true;

        private Effect effectType;
        private bool hasEffect;
        private float effectDuration;
        private float effectTimer;
        private float effectDamegePerSec;
        private float effectCount;
        private float effectSlowRate = 1.0f;

        private ISpellMaker spell_fire;
        private ISpellMaker spell_ice;
        private ISpellMaker spell_water;
        private ISpellMaker spell_lightning;
        private ISpellMaker spell_Q;
        private ISpellMaker spell_E;
        private char spell;

        private float actionCoolDown = 0f;
        private float atkCoolDown = 0.5f;

        private float spellQCoolDown = 5f;
        private float spellQCoolDowntimer = 0f;
        private float spellECoolDown = 5f;
        private float spellECoolDowntimer = 0f;

        private float spellCastDuration = 1f;
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

        void Start()
        {
            spellHome = GameObject.Find("SpellMakerHome");
            spell_fire = spellHome.GetComponent<FireBallMaker>();
            spell_ice = spellHome.GetComponent<BlizzardMaker>();
            spell_water = spellHome.GetComponent<WaterSurgeMaker>();
            spell_lightning = spellHome.GetComponent<ThunderdMaker>();

            spell_Q = spell_water;
            spell_E = spell_lightning;

            closeRangeWeapon = weapon.transform.GetChild(0).gameObject.GetComponent<CRWeaponController>();
            closeRangeWeapon.Config(10f, 10f, 120f, 1f, true);

            hitTakenInterverl = 0.2f; // 0.2 sec

            bodyCollider = this.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();

            globalLight = GameObject.Find("GlobalLight2D").GetComponent<Light2D>();
            torchLight = this.transform.GetChild(0).gameObject.GetComponent<Light2D>();
        }

        void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // move player

            if (!usingPortal)
            {
                Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
                direction = direction.normalized;

                this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * effectSlowRate * Time.deltaTime;
            }
            else
            {
                UsePortal();
            }

            // Reduce all coll down count.
            UpdateCoolDowns();

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
                CastSpell(mousePos);
            }

            // Hit timer management.
            if (hitTimer < hitTakenInterverl)
            {
                hitTimer += Time.deltaTime;
            }

            // Effect timer management.
            if (hasEffect)
            {
                if (effectTimer < effectDuration)
                {
                    effectTimer += Time.deltaTime;
                }
                else
                {
                    effectTimer = 0f;
                    effectDuration = 0f;
                    effectDamegePerSec = 0f;
                    effectSlowRate = 1f; // no slow
                    effectCount = 0;

                    if (effectType == Effect.Frezze)
                    {
                        readyForAction = true;
                    }
                    effectType = Effect.None;
                    hasEffect = false;
                }

                const float burnInterveal = 0.5f;
                // Burn effect management.
                if (effectType == Effect.Burn && effectTimer > (burnInterveal * (effectCount + 1)))
                {
                    // Take next Burn Damage
                    TakeDamage(effectDamegePerSec * burnInterveal);
                    effectCount++;
                }

                // Frezze
                if (effectType == Effect.Frezze && effectSlowRate != 0.111f)
                {
                    readyForAction = false;
                    effectSlowRate = 0.111f;
                    // Might change player color
                }
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

        void SetEffect(Effect effect, float time, float damegePerSec = 0f, float slowRate = 0f)
        {
            effectType = effect;
            effectDuration = time;
            effectDamegePerSec = damegePerSec;
            effectCount = 0;
            effectSlowRate = slowRate;
            effectTimer = 0f;
            hasEffect = true;
        }

        void TakeDamage(float damage)
        {
            if (hitTimer > hitTakenInterverl)
            {
                // Able to take another Hit

                // Take Damage and manage heealth


                // Reset timer
                hitTimer = 0f;
            }

            if (hasEffect && effectType == Effect.Frezze)
            {
                // Clear Frezze effect
                effectTimer = 0f;
                effectDuration = 0f;
                effectDamegePerSec = 0f;
                effectSlowRate = 1f; // no slow
                effectCount = 0;
                readyForAction = true;
                effectType = Effect.None;
                hasEffect = false;
            }
        }

        void UpdateCoolDowns()
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

        void CastSpell(Vector3 mousePos)
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

        void UsePortal()
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
                    this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * effectSlowRate * Time.deltaTime;
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
                    Debug.Log("Done reached inside");
                }
                else
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * effectSlowRate * Time.deltaTime;
                    globalLight.intensity = globalLight.intensity - speed * effectSlowRate  * Time.deltaTime * oldIntensity;
                    torchLight.intensity = torchLight.intensity - speed * effectSlowRate * Time.deltaTime * 0.5f;
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
                    effectSlowRate = 1f;
                    globalLight.intensity = lightIntensity;
                    torchLight.intensity = 0.5f;
                    Debug.Log("Done use Portal");
                }
                else
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * effectSlowRate * Time.deltaTime;
                    globalLight.intensity = globalLight.intensity + speed * effectSlowRate * Time.deltaTime * lightIntensity / 2.5f;
                    torchLight.intensity = torchLight.intensity + speed * effectSlowRate * Time.deltaTime * 0.5f / 2.5f;

                    if (globalLight.intensity > lightIntensity)
                    {
                        globalLight.intensity = lightIntensity;
                    }

                    if (torchLight.intensity > 0.5f)
                    {
                        torchLight.intensity = 0.5f;
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
            effectSlowRate = 0.7f;
        }


    }
}
