using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public interface IMobController
    {
        void GetPlayer(GameObject player);
        void TakeDamage(float damage);
    }
}