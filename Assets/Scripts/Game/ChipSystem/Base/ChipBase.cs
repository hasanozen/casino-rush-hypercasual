using System.Linq;
using Config;
using DG.Tweening;
using Game.ChipSystem.Events;
using Game.LevelSystem.Managers;
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
        
        public virtual void Init()
        {
            ChipEventManager = new ChipEventManager();
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
        }

        #endregion
    }
}
