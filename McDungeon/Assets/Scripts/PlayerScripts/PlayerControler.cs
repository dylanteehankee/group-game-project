using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject Weapon;
        [SerializeField] private GameObject spellHome;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float attackSpeedFactor;
        [SerializeField] private float attackAngle;
        private ISpellMaker spell_1;
        private bool attacking;
        private GameObject closeRangeWeapon;
        private MeshRenderer hitBoxRender;
        private CapsuleCollider2D hitBoxCllider;

        [SerializeField] private GameObject prefab_fireball;

        private float atkProgress; // 0 - 1

        void Start()
        {
            spellHome = GameObject.Find("SpellMakerHome");
            closeRangeWeapon = Weapon.transform.GetChild(1).gameObject;
            hitBoxRender = closeRangeWeapon.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
            hitBoxCllider = closeRangeWeapon.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider2D>();

            attacking = false;
            hitBoxRender.enabled = false;
            hitBoxCllider.enabled = false;

            spell_1 = spellHome.GetComponent<BlizzardMaker>();
        }


        void FixedUpdate()
        {
            // move player
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            direction = direction.normalized;

            this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * Time.deltaTime;

            if (!attacking)
            {
                // change weapon direction
                ChnagWeaponDirection();
            }
        }

        void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetButtonUp("Fire1"))
            {
                attacking = true;
                hitBoxRender.enabled = true;
                hitBoxCllider.enabled = true;
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                Shoot();
            }

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


            if (attacking)
            {
                Attack();
            }
        }

        void Shoot()
        {
        }

        private void ChnagWeaponDirection()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lookDir = mousePos - Weapon.transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            Weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void Attack()
        {
            if (atkProgress > 1f)
            {
                atkProgress = 1f;
            }


            float angle = -attackAngle / 2f + attackAngle * atkProgress;
            closeRangeWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

            if (atkProgress == 1f)
            {
                attacking = false;
                hitBoxRender.enabled = false;
                hitBoxCllider.enabled = false;
                atkProgress = 0f;
                closeRangeWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                atkProgress += attackSpeed * attackSpeedFactor * Time.deltaTime;
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
    }
}
