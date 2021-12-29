﻿using System.Collections.Generic;
using System.Linq;
using Config;
using Game.GateSystem.Base;
using Game.LevelSystem.Managers;
using Game.PoolingSystem;
using UnityEngine;
using Utils;
using Zenject;

namespace Game.GateSystem.Controllers
{
    public class GateController
    {
        private AssetManager _assetManager;
        private ObjectPooler _objectPooler;
        
        private List<GateBase> _gates;

        private int _positiveGateCount = 5, _negativeGateCount = 5;
        
        public void Init(AssetManager assetManager, ObjectPooler objectPooler)
        {
            _assetManager = assetManager;
            _objectPooler = objectPooler;
        }

        public void InitializeGates()
        {
            _gates = new List<GateBase>();

            _objectPooler.CreatePool(NameFields.GATE_OBJECT,
                _assetManager.GetPrefabObject(NameFields.GATE_OBJECT),
                _positiveGateCount + _negativeGateCount);

            _objectPooler.SpawnObjectsWithTag(NameFields.GATE_OBJECT);

            List<GameObject> objects = _objectPooler.GetObjectsFromDictionary(NameFields.GATE_OBJECT).ToList();
            for (int i = 0; i < objects.Count; i++)
            {
                GateType type = i < _positiveGateCount ? GateType.POSITIVE : GateType.NEGATIVE;
                GateBase gate = objects[i].GetComponent<GateBase>();
                _gates.Add(gate);
                CreateGate(gate, type);
            }

            _gates.Shuffle();
            SetGatePosition();
        }

        public void SetGatePosition()
        {
            float limitX = 1.5f;
            float minPoint = 20f;
            float minDistBetweenGates = 10f;
            float spectrum = 20f;
            float maxPoint = 100;

            for (int i = 0; i < _gates.Count; i+=2)
            {
                float zPos = Mathf.Clamp(Random.Range(minPoint, minPoint + spectrum), minPoint, maxPoint);
                
                _gates[i].transform.position =
                    new Vector3(limitX, _gates[i].transform.position.y, zPos);
                
                _gates[i + 1].transform.position =
                    new Vector3(-limitX, _gates[i + 1].transform.position.y, zPos);
                
                _gates[i].Activate();
                _gates[i + 1].Activate();

                minPoint += minDistBetweenGates;
            }
        }

        private void CreateGate(GateBase gate, GateType gateType)
        {
            EffectType effectType = gateType == GateType.POSITIVE
                ? (EffectType)Random.Range(0, 2)
                : (EffectType)Random.Range(3, 4);
            
            gate.Init(gateType, effectType);

            var material = gateType == GateType.POSITIVE
                ? _assetManager.GetBasicMaterial(NameFields.GATE_POSITIVE_MATERIAL)
                : _assetManager.GetBasicMaterial(NameFields.GATE_NEGATIVE_MATERIAL);
            gate.SetGateMaterial(material);

            var color = gateType == GateType.POSITIVE
                ? GameConfig.DEFAULT_GATE_POSITIVE_COLOR
                : GameConfig.DEFAULT_GATE_NEGATIVE_COLOR;

            var img = gateType == GateType.POSITIVE
                ? _assetManager.GetGatePositiveImage(NameFields.IMG_POSITIVE_SOCCER)
                : _assetManager.GetGateNegativeImage(NameFields.IMG_NEGATIVE_WATCH);

            string sign;
            int limit;
            int value = 1;
            switch (effectType)
            {
                case EffectType.ADDITION:
                    sign = "+";
                    limit = GameConfig.MAX_ADDITION_NUMBER;
                    value = GameConfig.CHIP_VALUES[Random.Range(0, GameConfig.CHIP_VALUES.Length)] *
                            Random.Range(2, limit);
                    break;
                case EffectType.SUBTRACTION:
                    sign = "-";
                    limit = GameConfig.MAX_SUBTRACTION_NUMBER;
                    value = GameConfig.CHIP_VALUES[Random.Range(0, GameConfig.CHIP_VALUES.Length)] *
                            Random.Range(2, limit);
                    break;
                case EffectType.MULTIPLICATION:
                    sign = "x";
                    limit = GameConfig.MAX_MULTIPLICATION_NUMBER;
                    value = Random.Range(1, limit);
                    break;
                case EffectType.DIVISION:
                    sign = "÷";
                    limit = GameConfig.MAX_DIVISION_NUMBER;
                    Random.Range(1, limit);
                    break;
                default:
                    sign = "";
                    limit = 5;
                    break;
            }
            string text = sign + "$" + value;
            
            gate.SetGateValues(value, color, img, text);
        }
    }
}
