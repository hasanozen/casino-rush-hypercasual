using Config;
using Game.CharacterSystem.Controllers;
using Game.CharacterSystem.Events;
using UnityEngine;
using Utils;

namespace Game.CharacterSystem.Base
{
    public class CharacterBase : MonoBehaviour
    {
        public int PlayerHealth;

        #region Managers

        protected CharacterEventManager CharacterEventManager;

        #endregion

        #region Controllers

        protected CharacterMovementController CharacterMovementController;
        protected CharacterAnimatorController CharacterAnimatorController;

        #endregion

        public virtual void Init()
        {
            PlayerHealth = GameConfig.CHARACTER_DEFAULT_HEALTH;

            var mainRigidbody = GetComponent<Rigidbody>();
            var animator = transform.Find("WomenModel").GetComponent<Animator>();

            CharacterEventManager = new CharacterEventManager();
            CharacterMovementController = gameObject.AddComponent<CharacterMovementController>();

            CharacterAnimatorController = new CharacterAnimatorController(animator);
            
            CharacterMovementController.Init(transform, GameConfig.CHARACTER_FORWARD_SPEED, GameConfig.CHARACTER_SWIPE_SPEED);

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CharacterEventManager.SubscribeEvent(CharacterEventType.ON_START, OnStart);
            CharacterEventManager.SubscribeEvent(CharacterEventType.ON_FINISH, OnFinish);
            CharacterEventManager.SubscribeEvent(CharacterEventType.ON_DEATH, OnDeath);
            CharacterEventManager.SubscribeEvent(CharacterEventType.ON_RESTART, OnRestart);
        }

        public CharacterEventManager GetEventManager()
        {
            return CharacterEventManager;
        }

        #region Event Methods

        private void OnStart()
        {
            PlayerHealth = GameConfig.CHARACTER_DEFAULT_HEALTH;
        }
        
        private void OnFinish()
        {
            CharacterAnimatorController.PerformIdleAnimation();
        }
        
        private void OnDeath()
        {
            CharacterAnimatorController.Deactivate();
        }
        
        private void OnRestart()
        {
            Timer.Instance.TimerWait(1f, () =>
            {
                CharacterAnimatorController.Activate();
                if (PlayerHealth < 1)
                {
                    CharacterEventManager.InvokeEvent(CharacterEventType.ON_RESTART);
                }
                else
                {
                    //TODO: Manual restart process
                }
            });
        }

        #endregion
    }
}
