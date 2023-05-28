using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class EnemyController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("loaded");

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Trigger Enter: " + other.gameObject.name);


            if (other.gameObject.tag == "PlayerWeapon")
            {
                Debug.Log("Trigger Enter: " + other.gameObject.name);
            }

            // Perform actions or logic when the collision occurs
        }
    }
}