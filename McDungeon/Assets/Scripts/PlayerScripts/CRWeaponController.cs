using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobs;

namespace McDungeon
{
    public class CRWeaponController : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float attackSpeedFactor;
        [SerializeField] private float attackAngle;
        [SerializeField] private float knockBack;
        [SerializeField] private bool active;
        private SpriteRenderer hitBoxRender;
        private CapsuleCollider2D hitBoxCllider;
        private CRWweaponHitBox hitBoxController;
        private float weaponDir;   // angle
        private bool attacking;
        private float atkProgress; // 0 - 1

        private Animator attackAnimator;
        private GameObject hitbox;

        void Awake()
        {
            hitbox = this.transform.GetChild(0).gameObject;
            hitBoxRender = this.hitbox.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            hitBoxCllider = this.hitbox.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider2D>();
            hitBoxController = this.hitbox.transform.GetChild(0).gameObject.GetComponent<CRWweaponHitBox>();
            attackAnimator = this.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Animator>();

            attackSpeedFactor = 0.6f;
            knockBack = 0f;
            attacking = false;
            hitBoxRender.enabled = false;
            hitBoxCllider.enabled = false;
            Debug.Log("hitbox name: ===========[" + hitbox.name + "]=======================");
        }

        void LateUpdate()
        {
            if (active)
            {
                WeaponUpdate();
            }
            else
            {
                // change weapon direction
                ChangeWeaponDirection();
            }
        }

        public void Config(float attackDamage, float attackSpeed, float attackAngle, float knockBack, bool active, float attackSpeedFactor = 0.4f)
        {
            this.attackDamage = attackDamage;
            hitBoxController.setDamage(attackDamage, knockBack);
            this.attackSpeed = attackSpeed;
            this.attackAngle = attackAngle;
            this.knockBack = knockBack;
            this.active = active;
            this.attackSpeedFactor = attackSpeedFactor;
            Debug.Log("configed weapon + " + attackAngle);
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
                ChangeWeaponDirection();
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetButtonUp("Fire1"))
            {
                attacking = true;
                hitBoxRender.enabled = true;
                hitBoxCllider.enabled = true;
                attackAnimator.Rebind();
            }


            if (attacking)
            {
                Attack();
            }
        }

        private void ChangeWeaponDirection()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lookDir = mousePos - this.transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            weaponDir = angle;
            this.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void Attack()
        {
            float angle = attackAngle / 2f - attackAngle * atkProgress;
            this.hitbox.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

            if (atkProgress == 1f)
            {
                attacking = false;
                hitBoxRender.enabled = false;
                hitBoxCllider.enabled = false;
                atkProgress = 0f;
                this.hitbox.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                active = false;
            }
            else
            {
                atkProgress += attackSpeed * attackSpeedFactor * Time.deltaTime;
            }

            if (atkProgress > 1f)
            {
                atkProgress = 1f;
            }
        }
    }
}
