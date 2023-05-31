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

        private ISpellMaker spell_1;
        [SerializeField] private GameObject prefab_fireball;


        void Start()
        {
            spellHome = GameObject.Find("SpellMakerHome");
            spell_1 = spellHome.GetComponent<BlizzardMaker>();

            closeRangeWeapon = Weapon.transform.GetChild(0).gameObject.GetComponent<CRWeaponController>();
            closeRangeWeapon.Config(10f, 120f, true);

            hitTakenInterverl = 0.2f; // 0.2 sec
        }


        void FixedUpdate()
        {
            // move player
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            direction = direction.normalized;

            this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * effectSlowRate * Time.deltaTime;
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
