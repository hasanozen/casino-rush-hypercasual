using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Game.CharacterSystem.Base;
using Game.ChipSystem.Base;
using Game.ChipSystem.Events;
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
        private GameObject container;
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

            CreateMiniChips();
            FillMiniChipValues();
        }

        #region Gate Process

        public void AddChipFromGate(int value)
        {
            var existingValues = FindExistingValues(GameConfig.CHIP_VALUES, value);

            foreach (var keyValuePair in existingValues)
            {
                for (int i = 0; i < keyValuePair.Value; i++)
                    AddChip(keyValuePair.Key);
            }
        }
        
        public void SubtractChipFromGate()
        {
        }

        public void MultiplyChipFromGate()
        {
            //TODO: Toplam chip değerlerini al ve çarparak çip ekle
        }

        public void DivideChipFromGate()
        {
        }

        #endregion

        public void AddChip(int value)
        {
            var chipMini = _chipMiniObjects[currentActivatedChipCount].GetComponent<ChipMini>();
            chipMini.Value = value;
            chipMini.SetChipMaterial(GetChipMaterial(chipMini.Value));
            chipMini.GetEventManager().InvokeEvent(ChipEventType.ON_VALUE_CHANGE);

            container.GetComponent<ChipContainer>().ActivateChips(1);

            currentActivatedChipCount++;
        }

        public void SubtractChip(int amount)
        {
            currentActivatedChipCount -= amount;

            container.GetComponent<ChipContainer>().DeactivateChips(amount);
        }

        public void SubtractChip(int valueOf, int amount)
        {
            
        }

        private void CreateContainer()
        {
            var player = FindObjectOfType<PlayerCharacter>().transform;
            container = Instantiate(_assetManager.GetPrefabObject("Container"), player);

            container.transform.position = new Vector3(
                player.position.x,
                player.localScale.y,
                player.position.z - player.localScale.z
            );

            container.AddComponent<ChipContainer>();
            container.GetComponent<ChipContainer>().Init(_objectPooler);
        }

        private void CreateMiniChips()
        {
            _objectPooler.CreatePool(
                NameFields.DEFAULT_MINI_CHIP_POOL_TAG,
                _assetManager.GetPrefabObject(NameFields.DEFAULT_MINI_CHIP_POOL_TAG),
                GameConfig.DEFAULT_MINI_CHIP_POOL_SIZE);

            _objectPooler.SpawnObjectsWithTag("Chip_Mini");
            _chipMiniObjects = _objectPooler.GetObjectsFromDictionary("Chip_Mini").ToList();
            container.GetComponent<ChipContainer>().SubscribeMembers(_chipMiniObjects);
        }

        private void FillMiniChipValues()
        {
            int i = 0;
            foreach (var _chipMiniObject in _chipMiniObjects)
            {
                ChipMini component = _chipMiniObject.GetComponent<ChipMini>();
                component.Init();
                //component.Value = _chipObjects[i].GetComponent<Chip>().Value;
                //component.SetChipMaterial(GetChipMaterial(component.Value));
                //component.GetEventManager().InvokeEvent(ChipEventType.ON_VALUE_CHANGE);

                _chipMiniObject.transform.SetParent(player);

                component.DeactivateChip();
                i++;
            }
        }

        private void CreateChips()
        {
            _objectPooler.CreatePool(
                NameFields.DEFAULT_CHIP_POOL_TAG,
                _assetManager.GetPrefabObject(NameFields.DEFAULT_CHIP_POOL_TAG),
                GameConfig.DEFAULT_CHIP_POOL_SIZE);

            _objectPooler.SpawnObjectsWithTag(NameFields.DEFAULT_CHIP_POOL_TAG);

            _chipObjects = _objectPooler.GetObjectsFromDictionary(NameFields.DEFAULT_CHIP_POOL_TAG).ToList();
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

                _objectPooler.SpawnFromPool(NameFields.DEFAULT_CHIP_POOL_TAG, newPos);
            }
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