using Data;
using Game.BetSystem.Base;
using UnityEngine;

namespace Game.MiniGames.Managers
{
    public class LevelGameManager
    {
        public void StartLevelGame(LevelData levelData)
        {
            levelData.GetCurrentLevelGame().Init(GetRandomWinner());
        }

        private int GetRandomWinner()
        {
            int bidderCount = Transform.FindObjectsOfType<Bidder>().Length;

            return Random.Range(0, bidderCount);
        }
    }
}
