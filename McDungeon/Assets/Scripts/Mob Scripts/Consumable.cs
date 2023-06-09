using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    [CreateAssetMenu]
    public class Consumable : ScriptableObject
    {
        [SerializeField]
        private int sellValue;
        [SerializeField]
        private int buyValue;

        public void UseEffect(GameObject playeObject)
        {

        }
    }
}