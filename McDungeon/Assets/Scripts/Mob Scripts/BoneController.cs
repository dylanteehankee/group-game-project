using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class BoneController : MonoBehaviour
    {
        [SerializeField]
        private int[] boneSpeed = {600, 800};
        [SerializeField]
        private int[] damage = {1, 2};
        private float knockbackDuration = 1.0f;
        private bool active = true;
        private GameObject ownerSkeleton;
        private bool pickup = true;
        private int difficulty;

        // Fix later to disable collision after collision
        // Add no collision if not moving
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (this.active && collision.gameObject.tag == "PlayerHitbox")
            {
                Vector2 location = this.transform.position;
                Vector2 playerLocation = collision.transform.position;
                var deltaLocation = playerLocation - location;
                deltaLocation.Normalize();
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * boneSpeed[difficulty]);
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage[difficulty], EffectTypes.None);
                this.GetComponent<Animator>().SetTrigger("BoneIdle");
                this.active = false;
            }
            else if (pickup && collision.gameObject.tag == "MobHitbox" && !collision.gameObject.GetComponent<SkeletonController>().HasBone())
            {
                pickup = false;
                collision.gameObject.GetComponent<SkeletonController>().GrabBone();
                if (collision.gameObject != ownerSkeleton)
                {
                    var newBone = collision.gameObject.GetComponent<SkeletonController>().GetBone();
                    ownerSkeleton.GetComponent<SkeletonController>().Reassign(newBone);
                }
                Destroy(this.gameObject);
            }
        }

        public void Throw(Vector2 playerLocation, GameObject skeleton, int difficulty)
        {
            this.difficulty = difficulty;
            ownerSkeleton = skeleton;
            Vector2 location = this.transform.position;
            var deltaLocation = playerLocation - location;
            deltaLocation.Normalize();
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(deltaLocation * boneSpeed[difficulty]);
            Destroy(this.gameObject, 40);
        }

        public void SetOwner(GameObject newOwner)
        {
            this.ownerSkeleton = newOwner;
        }
    }
}