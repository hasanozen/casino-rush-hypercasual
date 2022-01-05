using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Data;
using Game.BetSystem.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.BetSystem.Controllers
{
    public class BetController
    {
        private LevelData _levelData;
        private List<Bidder> _bidders;

        public void Init(LevelData levelData)
        {
            _levelData = levelData;
            _bidders = new List<Bidder>();
            _bidders = Transform.FindObjectsOfType<Bidder>().ToList();
        }

        public void ParticipateBets()
        {
            int value = _levelData.GetBalance();
            int[] bets = new int[_bidders.Count];
            
            Debug.Log("Value: " + value);

            int left = value % _bidders.Count;
            value -= left;
            value /= _bidders.Count;

            for (int i = 0; i < _bidders.Count; i++)
            {
                bets[i] = value;
                if (i == 0)
                    bets[i] += left;
            }

            for (int i = 0; i < _bidders.Count; i++)
            {
                _bidders[i].Bet = bets[i];
                _bidders[i].Odd = 2;
                
                Debug.Log("Bidder ID: " + i + " | Odd: " + _bidders[i].Odd + " | Bet: " + _bidders[i].Bet);
            }
        }

        public void SetWinner()
        {
            int winner = Random.Range(0, _bidders.Count);
            _bidders[winner].IsWinner = true;

            GetResult();
        }

        public void AddDatasToPlayerData()
        {
            _levelData.SetEndGameBonus(GetResult());
            _levelData.SetTotalGain();
        }

        public int GetWinnerID()
        {
            return (int)_bidders?.FirstOrDefault(x => x.IsWinner).ID;
        }

        public int GetResult()
        {
            return (int)_bidders?.FirstOrDefault(x => x.IsWinner).GetResult();
        }
        
        private Dictionary<int, int> FindExistingValues(int[] searchingValues, int value)
        {
            Array.Sort(searchingValues);
            Array.Reverse(searchingValues);
            
            int remaining = value;

            Dictionary<int, int> counts = new Dictionary<int, int>();

            for (int i = 0; i < searchingValues.Length; i++)
            {
                int v = searchingValues[i];

                if (v <= remaining)
                {
                    remaining -= v;
                    if (!counts.ContainsKey(v))
                        counts[v] = 0;

                    counts[v]++;
                    i--;
                }
            }

            return counts;
        }
    }
}
