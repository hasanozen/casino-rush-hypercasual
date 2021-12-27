using System;
using Game.CharacterSystem.Base;
using Game.ChipSystem.Managers;
using Game.PoolingSystem;
using UnityEngine;
using Zenject;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        private CharacterBase _mainCharacter;
        private ChipManager _chipManager;
        private ObjectPooler _objectPooler;

        [Inject]
        private void OnInitialize(CharacterBase characterBase, ChipManager chipManager, ObjectPooler objectPooler)
        {
            _mainCharacter = characterBase;
            _chipManager = chipManager;
            _objectPooler = objectPooler;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _mainCharacter.Init();
            _objectPooler.Init();
            _chipManager.Init();
        }
    }
}
