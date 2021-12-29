using System;
using System.Collections.Generic;
using Config;
using Game.CharacterSystem.Controllers;
using Game.CharacterSystem.Events;
using Game.ChipSystem.Base;
using Game.ChipSystem.Managers;
using Game.GateSystem.Base;
using Game.GateSystem.Controllers;
using UnityEngine;
using Utils;
using Zenject;

namespace Game.CharacterSystem.Base
{
    public class PlayerCharacter : CharacterBase
    {
        private CharacterInputController _characterInputController;
        private ChipManager _chipManager;

        private Camera _characterCamera;
        private Vector3 _cameraOffset;

        [Inject]
        private void OnInitialize(ChipManager chipManager)
        {
            _chipManager = chipManager;
        }

        public override void Init()
        {
            base.Init();

            _characterCamera = Camera.main;
            _cameraOffset = _characterCamera.transform.position - transform.position;

            _characterInputController = gameObject.AddComponent<CharacterInputController>();
            _characterInputController.Init();

            SubscribeControllerEvents();

            GetEventManager().SubscribeEvent(CharacterEventType.ON_START,
                () => { Timer.Instance.TimerWait(1f, () => _characterInputController.ActivateController()); });
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

            if (other.CompareTag("Extractor"))
            {
                _chipManager.SubtractChip(20);
            }

            if (other.GetComponent<GateBase>() != null)
            {
                Foo(other.GetComponent<GateBase>());
            }
        }

        public void Foo(GateBase gate)
        {
            var type = gate.EffectType;

            if (type == EffectType.ADDITION || type == EffectType.MULTIPLICATION)
            {
                _chipManager.AddChipFromGate(gate.EffectValue);
            }
            else if (type == EffectType.SUBTRACTION || type == EffectType.DIVISION)
            {
                _chipManager.SubtractChip(gate.EffectValue);
            }
        }

        #region Camera

        private void LateUpdate()
        {
            if (_characterCamera == null)
                return;

            _characterCamera.transform.position = transform.position + _cameraOffset;
        }

        #endregion
    }
}