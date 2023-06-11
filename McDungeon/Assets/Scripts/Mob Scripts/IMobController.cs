using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public interface IMobController
    {
        void GetPlayer(GameObject player);
        void TakeDamage(float damage, EffectTypes type);
    }
}