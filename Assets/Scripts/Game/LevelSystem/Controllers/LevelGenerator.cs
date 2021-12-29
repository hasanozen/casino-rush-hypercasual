using System;
using System.Collections;
using System.Collections.Generic;
using Game.CharacterSystem.Base;
using Game.LevelSystem.Base;
using Game.PoolingSystem;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Action<Level> OnLevelGenerated;
    
    private static int _counter;
    private Level _currentLevel;
    private CharacterBase _mainCharacter;
    private ObjectPooler _objectPooler;
    private Vector3 _startPosition;

    private void OnInitialize()
    {
        
    }
}
