using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject Weapon;
        [SerializeField] private GameObject spellHome;
        private ISpellMaker spell_1;

        [SerializeField] private GameObject prefab_fireball;

        private float atkProgress; // 0 - 1

        void Start()
        {
            spellHome = GameObject.Find("SpellMakerHome");
            spell_1 = spellHome.GetComponent<BlizzardMaker>();
        }


        void FixedUpdate()
        {
            // move player
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            direction = direction.normalized;

            this.gameObject.transform.position = this.gameObject.transform.position + direction * speed * Time.deltaTime;
        }

        void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                spell_1.Activate();
            }
            if (Input.GetKey(KeyCode.Q))
            {
                spell_1.ShowRange(this.transform.position, mousePos);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                GameObject spellInstance = spell_1.Execute(this.transform.position, mousePos);
            }

        }

        void Shoot()
        {
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
