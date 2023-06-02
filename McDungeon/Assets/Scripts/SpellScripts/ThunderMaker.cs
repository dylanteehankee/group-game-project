using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class ThunderdMaker : MonoBehaviour, ISpellMaker
    {
        [SerializeField] private GameObject prefab_thunder;
        [SerializeField] private GameObject player;
        [SerializeField] private MobManager mobManager;
        [SerializeField] private int amount;
        private List<GameObject> targets;
        int targetedAmount = 0;
        int untargetedAmount = 0;
        private float interval = 0.2f;
        private float timer = 0f;

        void Update()
        {
            if (targetedAmount > 0 || untargetedAmount > 0)
            {
                timer += Time.deltaTime;

                if (timer > interval)
                {
                    if (targetedAmount > 0 && targets.Count > 0)
                    {
                        GameObject thunder = Instantiate(prefab_thunder, player.transform.position, Quaternion.identity);
                        thunder.GetComponent<ThunderController>().Config(3f, targets[0]);
                        targets.RemoveAt(0);

                        targetedAmount--;
                        timer = 0f;
                    }
                    else
                    {
                        GameObject thunder = Instantiate(prefab_thunder, player.transform.position, Quaternion.identity);
                        thunder.GetComponent<ThunderController>().Config(3f, this.gameObject, false);

                        untargetedAmount--;
                        timer = 0f;
                    }
                }
            }
        }

        public GameObject Execute(Vector3 useless1, Vector3 useless2)
        {
            List<GameObject> mobsList = mobManager.GetMobs();
            mobsList<int> mobIndex;

            for (int i = 0; i < mobsList.Count; i++)
            {
                mobIndex.Add(i);
            }

            if (amount <= mobsList.Count)
            {
                targetedAmount = amount;
            }
            else
            {
                targetedAmount = mobsList.Count;
                untargetedAmount = amount - targetedAmount;
            }

            targets.Clear();
            for (int i = 0; i < targetedAmount; i++)
            {
                int index = Random.Range(0, mobIndex.Count);
                targets.Add(mobsList[mobIndex[index]]);
                mobIndex.RemoveAt(index);
            }
        }


        public void ChangeRange(float radius)
        {
            // Empty, Interface Placeholder
        }

        public void Activate()
        {
            // Empty, Interface Placeholder
        }

        public void ShowRange(Vector3 posistion, Vector3 mousePos)
        {
            // Empty, Interface Placeholder
        }
    }
}
