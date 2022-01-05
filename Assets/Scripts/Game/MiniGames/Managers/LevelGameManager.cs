using Data;
using UnityEngine;

namespace Game.MiniGames.Managers
{
    public class LevelGameManager
    {
        public void StartLevelGame(LevelData levelData)
        {
            levelData.GetCurrentLevelGame().Init(1);
        }
    }
}
