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

        #endregion

        public virtual void Init()
        {
            PlayerHealth = GameConfig.CHARACTER_DEFAULT_HEALTH;

            var mainRigidbody = GetComponent<Rigidbody>();

            CharacterEventManager = new CharacterEventManager();
            CharacterMovementController = gameObject.AddComponent<CharacterMovementController>();
            
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
            Debug.Log("OnStart method called");
            PlayerHealth = GameConfig.CHARACTER_DEFAULT_HEALTH;
        }
        
        private void OnFinish()
        {
            //TODO: Finishing process
            Debug.Log("OnFinish method called");
        }
        
        private void OnDeath()
        {
            //TODO: Dying process
            Debug.Log("OnDeath method called");
        }
        
        private void OnRestart()
        {
            //TODO: Restart process
            Debug.Log("OnRestart method called");
            
            Timer.Instance.TimerWait(1f, () =>
            {
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
