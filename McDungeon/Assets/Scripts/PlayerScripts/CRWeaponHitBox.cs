using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobs;

namespace McDungeon
{
    public class CRWweaponHitBox : MonoBehaviour
    {
        private float attackDamage;
        private float knockBack = 1000f;
        private GameObject center;

        void Start()
        {
            center = this.transform.parent.gameObject;
        }

        public void setDamage(float damage, float knockBack = 1000f)
        {
            attackDamage = damage;
            this.knockBack = knockBack;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Debug.Log("Collision Enter CRWeapon: " + collision.gameObject.name);
            if (other.gameObject.tag == "MobHitbox")
            {
                IMobController mobControl = other.gameObject.GetComponent<IMobController>();
                mobControl.TakeDamage(attackDamage, EffectTypes.None);
                
                Vector3 direction = other.gameObject.transform.position - center.transform.position;
                Vector2 dir2D = new Vector2(direction.x, direction.y);
                dir2D = dir2D.normalized;

                other.gameObject.GetComponent<Rigidbody2D>().AddForce(knockBack * dir2D);
            }
        }
    }
}
