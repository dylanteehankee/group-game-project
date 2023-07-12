using UnityEngine;

namespace McDungeon
{
    public class BlizzardMaker : MonoBehaviour, ISpellMaker
    {
        [SerializeField] private GameObject prefab_blizzard;
        [SerializeField] private GameObject blizzardPosIndicator;
        private float range = 4f;
        private Vector3 spellPos;
        private float timer = 0.02f;

        void Update()
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }

            if (timer < 0f)
            {
                blizzardPosIndicator.transform.position = new Vector3(0f, 0f, -30f);
            }

        }


        public void ChangeRange(float radius)
        {
            this.range = radius;
        }

        public void Activate()
        {
            blizzardPosIndicator.SetActive(true);
        }

        public void ShowRange(Vector3 posistion, Vector3 mousePos)
        {
            timer = 0.02f; // Refreash Timer
            // Recalculate Spell position.
            Vector3 distanceVec = (mousePos - posistion);
            distanceVec.z = 0f;
            Vector3 spellDir = distanceVec.normalized;
            float distance = distanceVec.magnitude;

            if (distance > range)
            {
                distance = range;
            }

            spellPos = posistion + spellDir * distance;

            // move indicator
            blizzardPosIndicator.transform.position = spellPos;
        }


        public GameObject Execute(Vector3 posistion, Vector3 mousePos)
        {
            // Turn off indicator.
            blizzardPosIndicator.SetActive(false);

            // Instantiate spell
            Vector3 distanceVec = (mousePos - posistion);
            distanceVec.z = 0f;
            Vector3 spellDir = distanceVec.normalized;
            float distance = distanceVec.magnitude;
            
            if (distance > range)
            {
                distance = range;
            }

            spellPos = posistion + spellDir * distance;

            GameObject blizzard = Instantiate(prefab_blizzard, spellPos, Quaternion.identity);
            blizzard.GetComponent<BlizzardController>().Config(3, 4f, .75f, false, spellDir, 0f);

            return blizzard;
        }
    }
}
