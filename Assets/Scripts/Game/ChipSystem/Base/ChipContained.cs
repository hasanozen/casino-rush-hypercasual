using DG.Tweening;
using Game.ChipSystem.Events;
using UnityEngine;

namespace Game.ChipSystem.Base
{
    public class ChipContained : ChipBase
    {
        public override void Init()
        {
            base.Init();
            MeshRenderer = transform.GetComponent<MeshRenderer>();
            
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            GetEventManager().SubscribeEvent(ChipEventType.ON_STACKED, OnStacked);
            GetEventManager().SubscribeEvent(ChipEventType.ON_DESTACKED, OnDestacked);
        }

        #region Event Methods
        
        private void OnDestacked()
        {
            transform.parent = null;

            Vector3 randomPosition = new Vector3(
                Random.Range(transform.position.x - 2, transform.position.x + 2),
                Random.Range(5, 10), 
                Random.Range(transform.position.z - 1, transform.position.z - 10));

            Vector3 randomRotation = new Vector3(
                Random.Range(0, 360),
                Random.Range(0, 360),
                Random.Range(0, 360)
            );
            
            transform.DORotate(randomRotation, .3f);
            transform.DOMove(randomPosition, .3f).OnComplete(() =>
            {
                DeactivateChip();
            });
        }

        public void OnStacked()
        {
            Vector3 rotationMove = new Vector3(90, Random.Range(0, 180), 0);
            transform.DOLocalRotate(rotationMove, .3f);
        }

        #endregion
        
    }
}
