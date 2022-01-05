using Game.LevelSystem.Base;
using UnityEngine;

namespace Game.MiniGames.Base
{
    public abstract class LevelGame : MonoBehaviour
    {
        public abstract LevelGameType levelGameType { get; }

        public abstract void Init(int winnerID);
    }
}
