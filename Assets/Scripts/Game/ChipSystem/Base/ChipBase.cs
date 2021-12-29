using System.Linq;
using Config;
using DG.Tweening;
using Game.ChipSystem.Events;
using Game.LevelSystem.Managers;
using TMPro;
using UnityEngine;
using Utils;
using Zenject;

namespace Game.ChipSystem.Base
{
    public class ChipBase : MonoBehaviour
    {
        #region Managers

        protected ChipEventManager ChipEventManager;

        #endregion

        public int Value { get; set; }
        public bool Active { get; private set; }
        protected MeshRenderer MeshRenderer { get; set; }
        private Material Material { get; set; }
        private TextMeshPro _valueText;
        
        public virtual void Init()
        {
            ChipEventManager = new ChipEventManager();
            _valueText = transform.Find("ValueText").GetComponent<TextMeshPro>();
            SubscribeEvents();
        }

        public void ActivateChip()
        {
            Active = true;
            gameObject.SetActive(true);
        }

        public void DeactivateChip()
        {
            Active = false;
            gameObject.SetActive(false);
        }

        public void SetActive(bool active)
        {
            Active = active;
        }

        private void SubscribeEvents()
        {
            ChipEventManager.SubscribeEvent(ChipEventType.ON_VALUE_CHANGE, OnValueChange);
        }

        public ChipEventManager GetEventManager()
        {
            return ChipEventManager;
        }

        public void SetChipMaterial(Material material)
        {
            Material = material;
        }

        #region Event Methods

        private void OnValueChange()
        {
            MeshRenderer.material = Material;
            _valueText.text = "$" + Value;
        }

        #endregion
    }
}
