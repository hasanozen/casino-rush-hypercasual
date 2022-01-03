using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using Game.CharacterSystem.Base;
using Game.CharacterSystem.Events;
using Game.ChipSystem.Managers;
using Game.GateSystem.Controllers;
using Game.LevelSystem.Base;
using Game.LevelSystem.Managers;
using Game.PoolingSystem;
using UnityEngine;
using Utils;
using Zenject;

public class LevelGenerator : MonoBehaviour
{
    public Action<Level> OnLevelGenerated;
    
    private static int _counter;
    private Level _currentLevel;
    private CharacterBase _mainCharacter;
    private ObjectPooler _objectPooler;
    private AssetManager _assetManager;
    private ChipManager _chipManager;

    private GateController _gateController;
    
    private Vector3 _startPosition;

    private List<GameObject> _platforms;
    private List<GameObject> _finish;
    private List<GameObject> _endGames;

    [Inject]
    private void OnInitialize(ObjectPooler objectPooler, AssetManager assetManager, ChipManager chipManager, CharacterBase characterBase)
    {
        _mainCharacter = characterBase;
        _objectPooler = objectPooler;
        _assetManager = assetManager;
        _chipManager = chipManager;

        _startPosition = new Vector3(0, 0, 0);
        _currentLevel = null;
        _counter = 0;

        _platforms = new List<GameObject>();
        _finish = new List<GameObject>();
        _endGames = new List<GameObject>();
    }

    public void Init()
    {
        _gateController = new GateController();
        _gateController.Init(_assetManager, _objectPooler);
        _gateController.InitializeGates();

        _mainCharacter.GetEventManager().SubscribeEvent(CharacterEventType.ON_FINISH, GeneratePlatforms);
        _mainCharacter.GetEventManager().SubscribeEvent(CharacterEventType.ON_FINISH, GenerateChips);
        _mainCharacter.GetEventManager().SubscribeEvent(CharacterEventType.ON_FINISH, GenerateGates);
        CreatePlatformPool();
        GeneratePlatforms();
        
        _chipManager.Init();
        
        GenerateChips();
        GenerateGates();
    }

    private void SelectLevelSettings()
    {
        LevelLength levelLength = GameConfig.GetLevelLength(_counter);
        LevelDifficulty levelDifficulty = GameConfig.GetLevelDifficulty(_counter);

        _currentLevel = new Level(_counter++, levelLength, levelDifficulty);
    }

    private void CreatePlatformPool()
    {
        _objectPooler.CreatePool("Path",
            _assetManager.GetPrefabObject("Path"),
            15);
        
        _objectPooler.SpawnObjectsWithTag("Path");
        _platforms = _objectPooler.GetObjectsFromDictionary("Path").ToList();
        
        _objectPooler.CreatePool("Finish",
            _assetManager.GetPrefabObject("Finish"),
            1);
        
        _objectPooler.SpawnObjectsWithTag("Finish");
        _finish = _objectPooler.GetObjectsFromDictionary("Finish").ToList();
        
        _objectPooler.CreatePool("EndGame_HorseRace",
            _assetManager.GetPrefabObject("HorseRacePlatform"),
            1);
        
        _objectPooler.SpawnObjectsWithTag("EndGame_HorseRace");
        _endGames = _objectPooler.GetObjectsFromDictionary("EndGame_HorseRace").ToList();
    }

    private void GeneratePlatforms()
    {
        SelectLevelSettings();
        _objectPooler.DeactivatePool("Path");
        _objectPooler.DeactivatePool("Finish");

        Debug.Log("Level Lenght: " + _currentLevel.LevelLength);
        
        var platform = _platforms[0];
        platform.SetActive(true);
        platform.transform.position = _startPosition;
        _mainCharacter.transform.position =
            new Vector3(platform.transform.position.x, 1, platform.transform.position.z);
        Vector3 lastPosition = _startPosition;
        float increaseAmount = platform.transform.localScale.z / 2;

        for (int i = 1; i < (int)_currentLevel.LevelLength; i++)
        {
            platform = _platforms[i];
            platform.SetActive(true);
            UpdateObjectPosition(platform.transform, ref lastPosition, ref increaseAmount);
        }

        platform = _finish[0];
        platform.SetActive(true);
        UpdateObjectPosition(platform.transform, ref lastPosition, ref increaseAmount);

        platform = _endGames[0];
        platform.SetActive(true);
        UpdateObjectPosition(platform.transform, ref lastPosition, ref increaseAmount);
        
        OnLevelGenerated.SafeInvoke(_currentLevel);
        _mainCharacter.GetEventManager().InvokeEvent(CharacterEventType.ON_START);
    }

    private void GenerateChips()
    {
        _chipManager.DeactivateChips();
        _chipManager.RefreshChips();
    }
    
    private void GenerateGates()
    {
        _gateController.DeactivateGates();
        _gateController.RefreshGates();
    }
    
    private void UpdateObjectPosition(Transform objTransform, ref Vector3 lastPosition, ref float increaseAmount)
    {
        var cachedObjectTransform = objTransform.transform;
        cachedObjectTransform.position = lastPosition;
        cachedObjectTransform.Translate(
            0,
            0,
            increaseAmount + cachedObjectTransform.localScale.z / 2);

        lastPosition = cachedObjectTransform.transform.position;
        increaseAmount = cachedObjectTransform.localScale.z / 2;
    }

    private void ClearPlatforms()
    {
        foreach (var platform in _platforms)
        {
            platform.SetActive(false);
        }
    }
}
