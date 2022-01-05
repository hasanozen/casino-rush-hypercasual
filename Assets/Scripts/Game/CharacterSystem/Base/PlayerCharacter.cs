using System;
using System.Collections.Generic;
using Config;
using Data;
using Game.BetSystem.Controllers;
using Game.CharacterSystem.Controllers;
using Game.CharacterSystem.Events;
using Game.ChipSystem.Base;
using Game.ChipSystem.Managers;
using Game.GateSystem.Base;
using Game.GateSystem.Controllers;
using Game.MiniGames.Managers;
using UnityEngine;
using Utils;
using Zenject;

namespace Game.CharacterSystem.Base
{
    public class PlayerCharacter : CharacterBase
    {
        private CharacterInputController _characterInputController;
        private ChipManager _chipManager;
        private LevelGameManager _levelGameManager;
        private LevelData _levelData;
        private BetController _betController;

        private Camera _characterCamera;
        private Vector3 _cameraOffset;

        [Inject]
        private void OnInitialize(ChipManager chipManager, LevelData levelData)
        {
            _chipManager = chipManager;
            _levelData = levelData;
        }

        public override void Init()
        {
            base.Init();

            _characterCamera = Camera.main;
            _cameraOffset = _characterCamera.transform.position - transform.position;

            _characterInputController = gameObject.AddComponent<CharacterInputController>();
            _characterInputController.Init();

            _levelGameManager = new LevelGameManager();
            _betController = new BetController();

            SubscribeControllerEvents();

            GetEventManager().SubscribeEvent(CharacterEventType.ON_START,
                () =>
                {
                    Timer.Instance.TimerWait(1f, () =>
                    {
                        _characterInputController.ActivateController();
                        CharacterAnimatorController.PerformRunAnimation();
                    });
                });
            
            GetEventManager().SubscribeEvent(CharacterEventType.ON_FINISH, () =>
            {
                _characterInputController.DeactivateController();
                GetEventManager().InvokeEvent(CharacterEventType.ON_START);
            });
            
            GetEventManager().SubscribeEvent(CharacterEventType.ON_END_GAME, () =>
            {
                _characterInputController.DeactivateController();
                CharacterAnimatorController.PerformIdleAnimation();
                StartLevelGame();
            });
        }

        private void SubscribeControllerEvents()
        {
            // Controller Event Subscriptions
            _characterInputController.OnLevelStarted += () => { CharacterMovementController.MoveForward(); };

            _characterInputController.OnTapPressing += () => { CharacterMovementController.MoveSide(); };

            _characterInputController.OnTapReleasing += () =>
            {
                //TODO: Some idle animation
            };
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Chip>() != null)
            {
                _chipManager.AddChip(other.GetComponent<Chip>().Value);
                other.GetComponent<Chip>().DeactivateChip();
            }

            if (other.GetComponent<GateBase>() != null)
            {
                GateBase gate = other.GetComponent<GateBase>();
                _chipManager.ProcessGate(gate.EffectType, gate.EffectValue);
            }

            if (other.CompareTag("Finish"))
            {
                Debug.Log("Finish");
                //GetEventManager().InvokeEvent(CharacterEventType.ON_FINISH);
                GetEventManager().InvokeEvent(CharacterEventType.ON_END_GAME);
            }
        }

        public void StartLevelGame()
        {
            _chipManager.SetLevelBalance();
            _betController.Init(_levelData);
            _betController.ParticipateBets();
            _betController.SetWinner();
            _levelGameManager.StartLevelGame(_levelData);
        }

        #region Camera

        private void LateUpdate()
        {
            if (_characterCamera == null)
                return;

            Vector3 newPos = new Vector3(_characterCamera.transform.position.x, transform.position.y,
                transform.position.z);
            _characterCamera.transform.position = newPos + _cameraOffset;
        }

        #endregion
    }
}