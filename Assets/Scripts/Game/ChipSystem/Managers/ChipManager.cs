using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Game.CharacterSystem.Base;
using Game.ChipSystem.Base;
using Game.ChipSystem.Events;
using Game.GateSystem.Base;
using Game.GateSystem.Controllers;
using Game.LevelSystem.Managers;
using Game.PoolingSystem;
using NUnit.Framework;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.ChipSystem.Managers
{
    public class ChipManager : MonoBehaviour
    {
        #region Managers

        private AssetManager _assetManager;
        private ObjectPooler _objectPooler;

        #endregion

        private List<GameObject> _chipObjects;
        private List<GameObject> _chipMiniObjects;
        private ChipContainer container;
        private Transform player;
        public Material tempMat;

        private int currentActivatedChipCount = 0;

        //TEMP
        private GateController _gateController;

        [Inject]
        private void OnInitialize(AssetManager assetManager, ObjectPooler objectPooler)
        {
            _assetManager = assetManager;
            _objectPooler = objectPooler;
        }

        public void Init()
        {
            _gateController = new GateController();
            _gateController.Init(_assetManager, _objectPooler);
            _gateController.InitializeGates();

            _objectPooler.Init();
            _chipObjects = new List<GameObject>();
            player = FindObjectOfType<PlayerCharacter>().transform;

            CreateContainer();

            CreateChips();
            FillChipValues();
            ReplaceChips();
        }

        public void ProcessGate(EffectType effectType, int value)
        {
            switch (effectType)
            {
                case EffectType.ADDITION:
                    container.ChipAddition(value);
                    break;
                case EffectType.SUBTRACTION:
                    container.ChipSubtraction(value);
                    break;
                case EffectType.MULTIPLICATION:
                    container.ChipMultiplication(value);
                    break;
                case EffectType.DIVISION:
                    container.ChipDivision(value);
                    break;
                default:
                    Debug.Log("Any effect type found with specified type");
                    break;
            }
        }

        private void CreateContainer()
        {
            var player = FindObjectOfType<PlayerCharacter>().transform;
            container = Instantiate(_assetManager.GetPrefabObject("Container"), player).AddComponent<ChipContainer>();

            container.transform.position = new Vector3(
                player.position.x,
                player.localScale.y,
                player.position.z - player.localScale.z
            );
            
            container.Init(_objectPooler, _assetManager);
        }

        private void CreateChips()
        {
            _objectPooler.CreatePool(
                NameFields.DEFAULT_CHIP_NAME,
                _assetManager.GetPrefabObject(NameFields.DEFAULT_CHIP_NAME),
                GameConfig.DEFAULT_CHIP_POOL_SIZE);

            _objectPooler.SpawnObjectsWithTag(NameFields.DEFAULT_CHIP_NAME);

            _chipObjects = _objectPooler.GetObjectsFromDictionary(NameFields.DEFAULT_CHIP_NAME).ToList();
        }

        public void FillChipValues()
        {
            foreach (var _chipObject in _chipObjects)
            {
                Chip component = _chipObject.GetComponent<Chip>();
                component.Init();
                component.Value = GetRandomValue();
                component.SetChipMaterial(GetChipMaterial(component.Value));
                component.GetEventManager().InvokeEvent(ChipEventType.ON_VALUE_CHANGE);
            }
        }

        private void ReplaceChips()
        {
            float xMax = 2f;
            float zMax = GameObject.Find("TempPath").GetComponent<Transform>().localScale.z;

            Vector3 firstPos = _chipObjects[0].transform.position;
            foreach (var chip in _chipObjects)
            {
                Vector3 newPos = new Vector3(
                    Random.Range(-xMax, xMax),
                    1.2f,
                    Random.Range(firstPos.z, zMax));

                _objectPooler.SpawnFromPool(NameFields.DEFAULT_CHIP_NAME, newPos);
            }
        }

        public void AddChip(int value)
        {
            container.ChipAddition(value);
        }

        public void SubtractChip(int value)
        {
            container.ChipSubtraction(value);
        }

        private int GetRandomValue()
        {
            return GameConfig.CHIP_VALUES[Random.Range(0, GameConfig.CHIP_VALUES.Length)];
        }

        public List<GameObject> GetChipObjects()
        {
            return _chipObjects;
        }

        private Material GetChipMaterial(int Value)
        {
            string key = GameConfig.CHIP_FEATURES
                .FirstOrDefault(x => x.Value == Value)
                .Key;

            return _assetManager.GetChipMaterial(key);
        }
        
        private Dictionary<int, int> FindExistingValues(int[] searchingValues, int value)
        {
            Array.Sort(searchingValues);
            Array.Reverse(searchingValues);
            
            int remaining = value;

            Dictionary<int, int> counts = new Dictionary<int, int>();

            for (int i = 0; i < searchingValues.Length; i++)
            {
                int v = searchingValues[i];

                if (v <= remaining)
                {
                    remaining -= v;
                    if (!counts.ContainsKey(v))
                        counts[v] = 0;

                    counts[v]++;
                    i--;
                }
            }

            return counts;
        }

    }
}