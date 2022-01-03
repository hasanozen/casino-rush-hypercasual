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
            float animDuration = .5f;
            Vector3 mainScale = transform.localScale;
            transform.parent = null;

            Vector3 randomPosition = new Vector3(
                Random.Range(transform.position.x - 5, transform.position.x + 5),
                transform.position.y, 
                transform.position.z);

            Vector3 randomRotation = new Vector3(
                Random.Range(0, 360),
                Random.Range(0, 360),
                Random.Range(0, 360)
            );
            
            transform.DORotate(randomRotation, animDuration);
            transform.DOScale(new Vector3(0, 0, 0), animDuration);
            transform.DOMove(randomPosition, animDuration).OnComplete(() =>
            {
                DeactivateChip();
                transform.localScale = mainScale;
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
