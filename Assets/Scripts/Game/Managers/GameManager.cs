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
        private LevelGenerator _levelGenerator;

        [Inject]
        private void OnInitialize(CharacterBase characterBase, ChipManager chipManager, ObjectPooler objectPooler, LevelGenerator levelGenerator)
        {
            _mainCharacter = characterBase;
            _chipManager = chipManager;
            _objectPooler = objectPooler;
            _levelGenerator = levelGenerator;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _mainCharacter.Init();
            _objectPooler.Init();
            _levelGenerator.Init();
        }
    }
}
