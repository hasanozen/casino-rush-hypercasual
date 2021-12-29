using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.GateSystem.Base
{
    public enum GateType
    {
        POSITIVE,
        NEGATIVE
    }

    public enum EffectType
    {
        ADDITION,
        MULTIPLICATION,
        SUBTRACTION,
        DIVISION
    }
    
    public class GateBase : MonoBehaviour
    {
        private List<MeshRenderer> _gateBorders;
        private SpriteRenderer _background, _image;
        private TextMeshPro _text;
        
        public GateType GateType { get; }
        public EffectType EffectType;

        public int EffectValue;

        public virtual void Init(GateType gateType, EffectType effectType)
        {
            EffectType = effectType;
            
            GetBorders();
            GetMiddleObjects();
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void GetMiddleObjects()
        {
            var middle = transform.Find("Middle");

            _background = middle.Find("Background").GetComponent<SpriteRenderer>();
            _image = middle.Find("Image").GetComponent<SpriteRenderer>();
            _text = middle.Find("Text").GetComponent<TextMeshPro>();
        }

        private void GetBorders()
        {
            var frame = transform.Find("Frame");

            _gateBorders = new List<MeshRenderer>
            {
                frame.Find("Column_1").GetComponent<MeshRenderer>(),
                frame.Find("Column_2").GetComponent<MeshRenderer>(),
                frame.Find("Header").GetComponent<MeshRenderer>()
            };
        }

        public void SetGateMaterial(Material material)
        {
            foreach (var border in _gateBorders)
            {
                border.material = material;
            }
        }

        public void SetGateValues(int value, Color backgroundColor, Sprite image, string text)
        {
            EffectValue = value;
            
            _background.color = backgroundColor;
            _image.sprite = image;
            _text.text = text;
        }

        public GateType GetGateType()
        {
            return GateType;
        }

        public EffectType GetEffectType()
        {
            return EffectType;
        }
    }
}
