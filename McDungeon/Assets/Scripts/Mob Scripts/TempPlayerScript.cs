using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using McDungeon;
namespace Mobs
{
    public class TempPlayerScript : MonoBehaviour
    {
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private GameObject target;
        private Vector3 movementDirection;
        // Start is called before the first frame update
        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                GameObject[] targets = GameObject.FindGameObjectsWithTag("MobHitbox");
                if (targets.Length > 0)
                {
                    target = targets[0];
                    target.GetComponent<IMobController>().TakeDamage(1, EffectTypes.Freeze);
                }
                // GameObject[] spawner = GameObject.FindGameObjectsWithTag("MobSpawner");
                // if (spawner.Length > 0)
                // {
                //     spawner[0].GetComponent<MobManager>().SpawnMobs((MobTypes)Random.Range(0, 5));
                // }
            }
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject[] targets = GameObject.FindGameObjectsWithTag("MobHitbox");
                if (targets.Length > 0)
                {
                    target = targets[0];
                    if (target.GetComponent<KnightController>() != null)
                    {
                        target.GetComponent<KnightController>().ActivateKnight();
                    }
                    else { Debug.Log("Not Knight"); }
                }
            }
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
            gameObject.transform.Translate(movementDirection * Time.deltaTime * speed);
        }
    }
}