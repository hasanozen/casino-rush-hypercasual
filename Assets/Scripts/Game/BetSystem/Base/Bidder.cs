using UnityEngine;

namespace Game.BetSystem.Base
{
    public class Bidder : MonoBehaviour
    {
        private int _id;
        private int _odd;
        private int _bet;
        private bool _isWinner;
        
        public int ID
        {
            get => _id;
            set => _id = value;
        }

        public int Odd
        {
            get => _odd;
            set => _odd = value;
        }

        public int Bet
        {
            get => _bet;
            set => _bet = value;
        }
        
        public bool IsWinner
        {
            get => _isWinner;
            set => _isWinner = value;
        }

        public int GetResult()
        {
            Debug.Log("Result: " + (_odd * _bet));
            return _odd * _bet;
        }
    }
}
