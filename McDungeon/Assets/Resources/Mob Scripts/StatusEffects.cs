using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    [CreateAssetMenu]
    public class StatusEffects : ScriptableObject
    {
        [SerializeField]
        private GameObject deathPrefab;
        [SerializeField]
        private GameObject stunPrefab;
        [SerializeField]
        private GameObject freezePrefab;
        [SerializeField]
        private GameObject ablazePrefab;
        [SerializeField]
        private GameObject potionDropPrefab;
        const float ABLAZEDURATION = 4.0f;
        const float ABLAZEDAMAGE = 1.0f;
        const float FREEZEDURATION = 4.0f;
        const float SLOWDURATION = 4.0f;
        const float SLOWMODIFIER = 0.5f;

        public void Death(Vector3 mobLocation, Vector2 size)
        {
            var deathCloud = Instantiate(this.deathPrefab);
            deathCloud.transform.position = mobLocation;
            deathCloud.transform.localScale = size;
            Destroy(deathCloud, 1);
        }
        
        public GameObject Stun(Transform mobTransform, Vector2 size, Vector2 displacement)
        {
            var stun = Instantiate(this.stunPrefab, mobTransform);
            stun.transform.position += (Vector3)displacement;
            stun.transform.localScale = size;
            return stun;
        }
        public GameObject Ablaze(Transform mobTransform, Vector2 size, Vector2 displacement)
        {
            var ablaze = Instantiate(this.ablazePrefab, mobTransform);
            ablaze.transform.position = mobTransform.position + Vector3.back;
            ablaze.transform.localScale = size;
            return ablaze;
        }

        public float GetAblazeDuration()
        {
            return ABLAZEDURATION;
        }

        public float GetAblazeDamage()
        {
            return ABLAZEDAMAGE;
        }

        public GameObject Freeze(Transform mobTransform, Vector2 size, Vector2 displacement)
        {
            var freeze = Instantiate(this.freezePrefab, mobTransform);
            freeze.transform.position = mobTransform.position + Vector3.back + (Vector3)displacement;
            freeze.transform.localScale *= size;
            return freeze;
        }

        public float GetFreezeDuration()
        {
            return FREEZEDURATION;
        }

        public float GetSlowDuration()
        {
            return SLOWDURATION;
        }

        public float GetSlowModifier()
        {
            return SLOWMODIFIER;
        }
    }
}
