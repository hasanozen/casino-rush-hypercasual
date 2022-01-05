using Game.MiniGames.Base;
using UnityEngine;

namespace Data
{
    public class LevelData
    {
        private int _balance;
        private int _endGameBonus;
        private int _totalGain;
        private int _levelIndex;
        private LevelGame _levelGame;

        public LevelData()
        {
            _levelIndex = PlayerData.Instance.LastLevelIndex;
        }

        public void SetBalance(int value)
        {
            Debug.Log("Balance Set: " + value);
            _balance = value;
        }

        public void SetEndGameBonus(int value)
        {
            _endGameBonus = value;
        }

        public void SetTotalGain()
        {
            _totalGain = _balance + _endGameBonus;
        }

        public void SetLevelIndex(int index)
        {
            _levelIndex = index;
        }

        public void SetLevelGame(LevelGame levelGame)
        {
            _levelGame = levelGame;
        }
        
        public int GetBalance()
        {
            return _balance;
        }

        public int GetEndGameBonus()
        {
            return _endGameBonus;
        }

        public int GetTotalGain()
        {
            return _totalGain;
        }
        
        public int GetLevelIndex()
        {
            return _levelIndex;
        }

        public LevelGame GetCurrentLevelGame()
        {
            return _levelGame;
        }

        public void AddDatasToPlayerData(string type)
        {
            switch (type)
            {
                case "OnlyBalance":
                    PlayerData.Instance.AddBalance(_balance);
                    return;
                case "BalanceWithBonus":
                    PlayerData.Instance.AddBalance(_totalGain);
                    return;
            }
            
            PlayerData.Instance.SetLevelIndex(_levelIndex);
        }

        public void Reset()
        {
            _balance = 0;
            _endGameBonus = 0;
            _totalGain = 0;
            _levelGame = null;
        }

    }
}
