using UnityEngine;

namespace McDungeon
{
    public interface ISpellMaker
    {
        void ChangeRange(float radius);
        void Activate();
        void ShowRange(Vector3 posistion, Vector3 mousePos);
        GameObject Execute(Vector3 posistion, Vector3 mousePos);
    }
}
