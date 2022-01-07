using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.BetSystem.Base
{
    public class BetInfo : MonoBehaviour
    {
        private int _id;
        private Color _color;
        private Image _horseImg;
        private Transform _winnerBg;
        private TextMeshProUGUI _oddText;
        private TextMeshProUGUI _bidText;
        
        public int ID
        {
            get => _id;
            set => _id = value;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public Image HorseImg
        {
            get => _horseImg;
            set => _horseImg = value;
        }

        public TextMeshProUGUI OddText
        {
            get => _oddText;
            set => _oddText = value;
        }

        public TextMeshProUGUI BidText
        {
            get => _bidText;
            set => _bidText = value;
        }

        private void Awake()
        {
            _horseImg = transform.Find("HorseImage").GetComponent<Image>();
            _oddText = transform.Find("OddText").GetComponent<TextMeshProUGUI>();
            _bidText = transform.Find("BidText").GetComponent<TextMeshProUGUI>();
            
            _winnerBg = transform.Find("WinnerHorseBG");
        }

        public void ApplyInformations(Color color, string oddText, string bidText)
        {
            _horseImg.color = color;
            _oddText.text = "X" + oddText;
            _bidText.text = "$" + bidText;
        }

        public void ActivateWinnerBg()
        {
            _winnerBg.gameObject.SetActive(true);
        }
        
        public void DeactivateWinnerBg()
        {
            _winnerBg.gameObject.SetActive(false);
        }
    }
}
