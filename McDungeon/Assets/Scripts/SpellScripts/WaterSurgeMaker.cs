using UnityEngine;

namespace McDungeon
{
    public class WaterSurgeMaker : MonoBehaviour, ISpellMaker
    {
        [SerializeField] private GameObject prefab_waterSurge;
        private float range;


        public void Activate()
        {

        }

        public void ChangeRange(float radius)
        {
            this.range = radius;
        }

        public void ShowRange(Vector3 posistion, Vector3 mousePos)
        {
            Debug.Log("Spell Aiming");
        }


        public GameObject Execute(Vector3 posistion, Vector3 mousePos)
        {
            // Instantiate spell
            Vector3 spellDir = (mousePos - posistion);
            spellDir.z = 0;
            spellDir = spellDir.normalized;
            GameObject waterSurge = Instantiate(prefab_waterSurge, posistion + spellDir, Quaternion.identity);
            waterSurge.GetComponent<WaterSurgeController>().Config(1f, 10f, 2f, spellDir);

            // Debug.Log("spellDir: " + spellDir);

            return waterSurge;
        }
    }
}
