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
        private CapsuleCollider2D hitBoxCllider;
        private CRWweaponHitBox hitBoxController;
        private SpriteRenderer hitBoxRender;
        private SpriteRenderer hitBoxCounterweightRender; // Making sure the sprite are same size for center the rotation-center
        private SpriteRenderer followerWeaponRender;
        private float weaponDir;   // angle
        private bool attacking;
        private float atkProgress; // 0 - 1

        private GameObject hitbox;
        [SerializeField] private Sprite weapon_0;
        [SerializeField] private Sprite weapon_1;
        [SerializeField] private Sprite weapon_2;
        [SerializeField] private Sprite weapon_3;
        [SerializeField] private Sprite weapon_4;
        [SerializeField] private Sprite weapon_5;
        private int currentWeaponIdex = 0;
        private float weaponScaleRatio = 1f;

        void Start()
        {
            hitbox = this.transform.GetChild(0).gameObject;
            hitBoxCllider = this.hitbox.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider2D>();
            hitBoxController = this.hitbox.transform.GetChild(0).gameObject.GetComponent<CRWweaponHitBox>();
            hitBoxRender = this.hitbox.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            hitBoxCounterweightRender = this.hitbox.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>();
            followerWeaponRender = GameObject.Find("FollowerWeapon").GetComponent<SpriteRenderer>();

            attackSpeedFactor = 0.6f;
            knockBack = 0f;
            attacking = false;
            hitBoxRender.enabled = false;
            hitBoxCllider.enabled = false;
            Debug.Log("hitbox name: ===========[" + hitbox.name + "]=======================");
            Debug.Log("Awake hitbox render: ===========[" + hitBoxRender + "]=======================");
            ChangeWeapon(0);
        }

        void LateUpdate()
        {
            //Debug.Log("Update hitbox render: ===========[" + hitBoxRender + "]=======================");
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
                Debug.Log("Firing and attacking");
                attacking = true;
                hitBoxRender.enabled = true;
                hitBoxCllider.enabled = true;
                followerWeaponRender.enabled = false;
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
                followerWeaponRender.enabled = true;
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

        public void ChangeWeapon(int weaponIndex)
        {
            if(hitBoxRender == null)
            {
                Debug.Log("Bad ");
                return;
            }
            // Need resize base on scale
            Vector3 scale = new Vector3(1.7f, 1.7f, 1f);
            scale = scale * weaponScaleRatio;

            //if(hitbox == null) return;
            //hitBoxRender = this.hitbox.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            Debug.Log("hitboxRender name: ===========[" + this.hitBoxRender + "]=======================");
            Debug.Log("hitboxRender color: ===========[" + hitBoxRender.color + "]=======================");
            hitBoxRender.transform.localScale = scale;
            hitBoxCounterweightRender.transform.localScale = scale;

            if (weaponIndex == 5)
            {
                // Need resize smaller for dagger
                // Vector3 scale = new Vector3(1.7f, 1.7f, 1f);
                // scale = scale * weaponScaleRatio;

                // hitBoxRender.transform.localScale = scale;
                // hitBoxCounterweightRender.transform.localScale = scale;
            }

            currentWeaponIdex = weaponIndex;
            switch (weaponIndex)
            {
                case 0:
                    hitBoxRender.sprite = weapon_0;
                    followerWeaponRender.sprite = weapon_0;
                    hitBoxCounterweightRender.sprite = weapon_0;
                    break;
                case 1:
                    hitBoxRender.sprite = weapon_1;
                    followerWeaponRender.sprite = weapon_1;
                    hitBoxCounterweightRender.sprite = weapon_1;
                    break;
                case 2:
                    hitBoxRender.sprite = weapon_2;
                    followerWeaponRender.sprite = weapon_2;
                    hitBoxCounterweightRender.sprite = weapon_2;
                    break;
                case 3:
                    hitBoxRender.sprite = weapon_3;
                    followerWeaponRender.sprite = weapon_3;
                    hitBoxCounterweightRender.sprite = weapon_3;
                    break;
                case 4:
                    hitBoxRender.sprite = weapon_4;
                    followerWeaponRender.sprite = weapon_4;
                    hitBoxCounterweightRender.sprite = weapon_4;
                    break;
                case 5:
                    hitBoxRender.sprite = weapon_5;
                    followerWeaponRender.sprite = weapon_5;
                    hitBoxCounterweightRender.sprite = weapon_5;
                    break;
            }

        }

        public void ChangeWeaponSacle(float scaleRatio)
        {
            weaponScaleRatio = scaleRatio;
            ChangeWeapon(currentWeaponIdex);
        }
    }
}
