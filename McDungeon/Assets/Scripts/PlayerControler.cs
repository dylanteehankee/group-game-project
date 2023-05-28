using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject Weapon;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float attackSpeedFactor;
        [SerializeField] private float attackAngle;
        private bool attacking;
        private GameObject closeRangeWeapon;
        private MeshRenderer hitBoxRender;
        private CapsuleCollider2D hitBoxCllider;

        [SerializeField] private GameObject prefab_fireball;

        private float atkProgress; // 0 - 1

        void Start()
        {
            attacking = false;
            closeRangeWeapon = Weapon.transform.GetChild(1).gameObject;
            hitBoxRender = closeRangeWeapon.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
            hitBoxCllider = closeRangeWeapon.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider2D>();
            hitBoxRender.enabled = false;
            hitBoxCllider.enabled = false;
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


            if (attacking)
            {
                Attack();
            }
        }

        void Shoot()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 spellDir = mousePos - this.transform.position;
            spellDir = spellDir.normalized;
            GameObject fireBall = Instantiate(prefab_fireball, this.transform.position, Quaternion.identity);
            fireBall.GetComponent<FireBallController>().Config(3f, 10f, 3, spellDir);
            Debug.Log("spellDir: " + spellDir);
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
