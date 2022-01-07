using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Data;
using DG.Tweening;
using Game.BetSystem.Controllers;
using Game.CameraSystem;
using Game.CharacterSystem.Controllers;
using Game.CharacterSystem.Events;
using Game.ChipSystem.Base;
using Game.ChipSystem.Managers;
using Game.GateSystem.Base;
using Game.GateSystem.Controllers;
using Game.MiniGames.Managers;
using TMPro;
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
        private CameraFollow _cameraFollow;

        private Camera _characterCamera;
        private Vector3 _cameraOffset;
        private TextMeshPro _balanceText;

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

            _cameraFollow = FindObjectOfType<CameraFollow>();
            _balanceText = transform.Find("BalanceText").Find("Text").GetComponent<TextMeshPro>();

            SubscribeControllerEvents();

            GetEventManager().SubscribeEvent(CharacterEventType.ON_START,
                () =>
                {
                    _characterInputController.DeactivateController();
                    CharacterAnimatorController.PerformIdleAnimation();

                    Timer.Instance.TimerWait(1f, () =>
                    {
                        _characterInputController.ActivateController();
                        _cameraFollow.EaseSharpness();
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

                _balanceText.text = "$" + _levelData.GetCurrentBalance();
            }

            if (other.GetComponent<GateBase>() != null)
            {
                GateBase gate = other.GetComponent<GateBase>();
                _chipManager.ProcessGate(gate.EffectType, gate.EffectValue);

                _balanceText.text = "$" + _levelData.GetCurrentBalance();
            }

            if (other.CompareTag("Finish"))
            {
                Debug.Log("Finish");
                if (!_levelData.CheckIfGamePlayed())
                {
                    _levelData.ConfirmLevelGameIsPlayed();
                    GetEventManager().InvokeEvent(CharacterEventType.ON_END_GAME);
                }
            }
        }

        public async void StartLevelGame()
        {
            _chipManager.SetLevelBalance();
            _betController.Init(_levelData);
            _betController.ParticipateBets();
            _betController.SetWinner();
            
            StartCoroutine(ResetBalanceText());
            
            await _chipManager.MoveChipsToBanks(2f);
            
            _betController.SetBetInfos();

            Timer.Instance.TimerWait(2f, () =>
            {
                _levelGameManager.StartLevelGame(_levelData);
            }); 
        }

        public IEnumerator ResetBalanceText()
        {
            int balance = _levelData.GetCurrentBalance();
            int decreaseAmount = balance / 10;

            while (balance > 0)
            {
                balance -= decreaseAmount;

                if (balance < 0)
                    balance = 0;

                _balanceText.text = balance.ToString();
                
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}