using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Data;
using Game.ChipSystem.Events;
using Game.LevelSystem.Managers;
using Game.PoolingSystem;
using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.ChipSystem.Base
{
    public class ChipContainer : MonoBehaviour
    {
        private ObjectPooler _objectPooler;
        private AssetManager _assetManager;
        private LevelData _levelData;

        private bool _active;
        private List<ChipContained> _chips;
        private List<ChipContained> _deactivatedChips;

        private int totalValue;
        public int TotalValue
        {
            get => totalValue;
            set => totalValue = value;
        }

        private float _containedChipScaleAxisY;

        public void Init(ObjectPooler objectPooler, AssetManager assetManager, LevelData levelData)
        {
            _objectPooler = objectPooler;
            _assetManager = assetManager;
            _levelData = levelData;
            
            _chips = new List<ChipContained>();
            _deactivatedChips = new List<ChipContained>();
            //_containedChipScaleAxisY = .6f;
            _containedChipScaleAxisY = .3f;
        }

        public void SetLevelBalance()
        {
            Debug.Log("SetLevelBalance");
            _levelData.SetBalance(GetTotalChipValues());
        }

        public void RefreshContainer()
        {
            Debug.Log("Refresh Container");
            for (int i = 0; i < _deactivatedChips.Count; i++)
            {
                Destroy(_deactivatedChips[i].gameObject);
            }

            for (int i = 0; i < _chips.Count; i++)
            {
                Destroy(_chips[i].gameObject);
            }
            
            _deactivatedChips.Clear();
            _chips.Clear();
        }
        
        public void ChipAddition(int value)
        {
            if (_chips.Count < 1)
            {
                if (value <= GameConfig.CHIP_VALUES.Max())
                {
                    CreateChip(value);
                    return;
                }
                
                AddMultipleChips(value);
            }

            ChipContained lastContainedChip = _chips.LastOrDefault();

            if (lastContainedChip.Value < GameConfig.CHIP_VALUES.Max())
                value = CompleteLastChip(value, lastContainedChip);
            
            AddMultipleChips(value);
        }

        public void ChipSubtraction(int value)
        {
            if (_chips.Count < 1)
                return;

            //Check if value to subtract greater than current total value
            value = value > TotalValue ? TotalValue : value;
            
            //Subtract last chip
            ChipContained lastChip = _chips.LastOrDefault();
            value -= lastChip.Value;
            RemoveChip(lastChip);

            if (value <= 0)
                return;
            
            //Subtract other chips
            Dictionary<int, int> existingValues = FindExistingValues(GameConfig.CHIP_VALUES, value);

            foreach (var keyValuePair in existingValues)
            {
                if (keyValuePair.Key == GameConfig.CHIP_VALUES.Max())
                {
                    for (int i = 0; i < keyValuePair.Value; i++)
                    {
                        lastChip = _chips.LastOrDefault();
                        value -= lastChip.Value;
                        RemoveChip(lastChip);
                    }
                }

                if (value <= 0)
                    return;
                
                //Set the chip on top
                lastChip = _chips.LastOrDefault();
                UpdateChipValues(lastChip, value);
            }
        }

        public void ChipMultiplication(int value)
        {
            int currentTotalChipValue = GetTotalChipValues();
            int valueToAdd = (currentTotalChipValue * value) - currentTotalChipValue;

            if (valueToAdd <= 0)
                return;
            
            ChipAddition(valueToAdd);
        }

        public void ChipDivision(int value)
        {
            if (value == 1)
                return;
            
            //Calculate the value for subtraction that needs to be multiplier of min chip value
            int currentTotalValue = GetTotalChipValues(); 
            int valueWillBeLeft, valueToSubtract, complementToDivision;

            valueWillBeLeft = currentTotalValue / value;
            valueToSubtract = currentTotalValue - valueWillBeLeft;
            complementToDivision = valueToSubtract % GameConfig.CHIP_VALUES.Min();

            valueWillBeLeft += complementToDivision;
            valueToSubtract = currentTotalValue - valueWillBeLeft;

            if (valueToSubtract <= 0)
                return;
            
            ChipSubtraction(valueToSubtract);
        }

        private void AddMultipleChips(int value)
        {
            if (value <= 0)
                return;
            
            Dictionary<int, int> existingValues = FindExistingValues(GameConfig.CHIP_VALUES, value);

            foreach (var keyValuePair in existingValues)
                for (int i = 0; i < keyValuePair.Value; i++)
                    CreateChip(keyValuePair.Key);
        }
        
        private void CreateChip(int value)
        {
            var objectToSpawn = _assetManager.GetPrefabObject(NameFields.DEFAULT_CONTAINED_CHIP_NAME);
            ChipContained contained = Instantiate(objectToSpawn).GetComponent<ChipContained>();
            
            _chips.Add(contained);
            contained.transform.SetParent(transform);
            
            contained.Init();
            contained.Value = value;
            contained.SetChipMaterial(GetChipMaterial(value));
            contained.GetEventManager().InvokeEvent(ChipEventType.ON_VALUE_CHANGE);
            contained.GetEventManager().InvokeEvent(ChipEventType.ON_STACKED);
            
            UpdateTotalChipValues();
            SetChipPosition(contained);
        }

        private void RemoveChip(ChipContained chip)
        {
            chip.GetEventManager().InvokeEvent(ChipEventType.ON_DESTACKED);
            _chips.Remove(chip);
            _deactivatedChips.Add(chip);
            
            UpdateTotalChipValues();
        }

        private int CompleteLastChip(int value, ChipContained lastContainedChip)
        {
            int maxChipValue = GameConfig.CHIP_VALUES.Max();

            int valueToCompleteLastChip = lastContainedChip.Value + value > maxChipValue 
                ? maxChipValue - lastContainedChip.Value 
                : value;

            UpdateChipValues(lastContainedChip, lastContainedChip.Value + valueToCompleteLastChip);
            value -= valueToCompleteLastChip;

            return value;
        }

        private void UpdateChipValues(ChipContained chip, int value)
        {
            chip.Value = value;
            chip.SetChipMaterial(GetChipMaterial(value));
            chip.GetEventManager().InvokeEvent(ChipEventType.ON_VALUE_CHANGE);
            
            UpdateTotalChipValues();
        }
        
        private void SetChipPosition(ChipContained chip)
        {
            float posX = 0;
            float posY = transform.localScale.y + _containedChipScaleAxisY * _chips.Count;
            float posZ = 0;
            
            chip.transform.localPosition = new Vector3(posX, posY, posZ);
        }

        private void UpdateTotalChipValues()
        {
            TotalValue = GetTotalChipValues();
        }

        private int GetTotalChipValues()
        {
            return _chips.Sum(x => x.Value);
        }

        public void ActivateContainer()
        {
            _active = true;
        }

        public void DeactivateContainer()
        {
            _active = false;
        }
        
        private Material GetChipMaterial(int Value)
        {
            string key = GameConfig.CHIP_FEATURES
                .FirstOrDefault(x => x.Value == Value)
                .Key;

            return _assetManager.GetChipMaterial(key);
        }
        
        private int GetRandomValue()
        {
            return GameConfig.CHIP_VALUES[Random.Range(0, GameConfig.CHIP_VALUES.Length)];
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