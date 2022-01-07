using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.BetSystem.Base
{
    public class BetInfo : MonoBehaviour
    {
        private int _id;
        private TextMeshPro _text;

        public int ID
        {
            get => _id;
            set => _id = value;
        }

        public TextMeshPro Text
        {
            get => _text;
            set => _text = value;
        }

        private void Awake()
        {
            _text = transform.Find("Info").GetComponent<TextMeshPro>();
        }

        public void SetText(string text)
        {
            _text.text = text;
            transform.DORotate(new Vector3(0, 0, 0), .5f);
            transform.DOMoveY(2, 1f).SetEase(Ease.OutBack);
        }
    }
}
