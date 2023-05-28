using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class CRWeaponController : MonoBehaviour
    {
        [SerializeField] private float attackSpeed;
        [SerializeField] private float attackSpeedFactor;
        [SerializeField] private float attackAngle;
        [SerializeField] private GameObject player;
        private bool attacking;
        private SpriteRenderer hitBoxRender;
        private CapsuleCollider2D hitBoxCllider;
        private float atkProgress; // 0 - 1
        private bool active = false;

        void Start()
        {
            hitBoxRender = this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            hitBoxCllider = this.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider2D>();

            active = false;
            attacking = false;
            hitBoxRender.enabled = false;
            hitBoxCllider.enabled = false;
        }

        void LateUpdate()
        {
            if (active)
            {
                WeaponUpdate();
            }
        }

        public void SetActive(bool setToState)
        {
            this.active = setToState;
        }

        private void WeaponUpdate()
        {
            // Move weapon with Player player
            this.gameObject.transform.position = player.transform.position;

            if (!attacking)
            {
                // change weapon direction
                ChnagWeaponDirection();
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetButtonUp("Fire1"))
            {
                attacking = true;
                hitBoxRender.enabled = true;
                hitBoxCllider.enabled = true;
            }


            if (attacking)
            {
                Attack();
            }
        }

        private void ChnagWeaponDirection()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lookDir = mousePos - this.transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            this.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void Attack()
        {
            if (atkProgress > 1f)
            {
                atkProgress = 1f;
            }


            float angle = -attackAngle / 2f + attackAngle * atkProgress;
            this.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

            if (atkProgress == 1f)
            {
                attacking = false;
                hitBoxRender.enabled = false;
                hitBoxCllider.enabled = false;
                atkProgress = 0f;
                this.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                atkProgress += attackSpeed * attackSpeedFactor * Time.deltaTime;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision Enter CRWeapon: " + collision.gameObject.name);
        }
    }
}
