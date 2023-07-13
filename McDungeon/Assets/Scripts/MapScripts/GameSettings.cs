using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public enum GameMode
    {
        Normal,
        Hard,
        Special
    }

    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        [SerializeField]
        private GameMode difficulty = GameMode.Normal;

        public GameMode GetDifficulty()
        {
            return this.difficulty;
        }

        public void SetDifficulty(GameMode newDifficulty)
        {
            this.difficulty = newDifficulty;
        }
    }
}
