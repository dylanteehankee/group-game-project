using UnityEngine;

namespace McDungeon
{
    public class FireBallMaker : MonoBehaviour, ISpellMaker
    {
        [SerializeField] private GameObject prefab_fireball;
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
            // Draw fireball direction.
            Debug.Log("Spell Aiming");
        }


        public GameObject Execute(Vector3 posistion, Vector3 mousePos)
        {
            // Instantiate spell
            Vector3 spellDir = (mousePos - posistion).normalized;
            GameObject fireBall = Instantiate(prefab_fireball, posistion, Quaternion.identity);
            fireBall.GetComponent<FireBallController>().Config(3f, 10f, 3, spellDir);

            // Debug.Log("spellDir: " + spellDir);

            return fireBall;
        }
    }
}
