using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Effect
{
    None,
    Slow,
    Frezze,
    Burn
}

namespace McDungeon
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject spellHome;
        [SerializeField] private GameObject Weapon;
        [SerializeField] private CRWeaponController closeRangeWeapon;
        [SerializeField] private float speed;
        private float hitTakenInterverl;
        private float hitTimer;

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


        void Start()
        {
            spellHome = GameObject.Find("SpellMakerHome");
            spell_fire = spellHome.GetComponent<FireBallMaker>();
            spell_ice = spellHome.GetComponent<BlizzardMaker>();
            spell_water = spellHome.GetComponent<WaterSurgeMaker>();
            spell_lightning = spellHome.GetComponent<ThunderdMaker>();

            spell_Q = spell_water;

            closeRangeWeapon = Weapon.transform.GetChild(0).gameObject.GetComponent<CRWeaponController>();
            closeRangeWeapon.Config(10f, 120f, true);

            hitTakenInterverl = 0.2f; // 0.2 sec
        }

        void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // move player
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            direction = direction.normalized;

            this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * effectSlowRate * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                spell_Q.Activate();
            }
            if (Input.GetKey(KeyCode.Q))
            {
                spell_Q.ShowRange(this.transform.position, mousePos);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                spell_Q.Execute(this.transform.position, mousePos);
            }

            // Hit timer management.
            if (hitTimer < hitTakenInterverl)
            {
                hitTimer += Time.deltaTime;
            }


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
                    effectSlowRate = 0.111f;
                    // Might change player color
                }
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
        }

    }
}
