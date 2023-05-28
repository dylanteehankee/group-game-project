using UnityEngine;

namespace McDungeon
{
    public interface ISpellMaker
    {
        void ChangeRange(float radius);
        void ShowRange(Vector3 MousePosistion);
        GameObject Execute(Vector3 posistion, Vector3 mousePos);
    }
}
