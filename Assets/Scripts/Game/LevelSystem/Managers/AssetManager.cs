using System.Collections.Generic;
using System.Linq;
using Config;
using Game.ChipSystem.Base;
using Game.LevelSystem.Base;
using Game.MiniGames.Base;
using UnityEngine;

namespace Game.LevelSystem.Managers
{
    public class AssetManager
    {
        private const string OBSTACLE_PATH = "Obstacles/";
        private const string PLATFORM_PATH = "Platforms/";
        private const string OBJECT_PATH = "Objects/";
        private const string CHIP_MATERIAL_PATH = "Materials/Chip";
        private const string BASIC_MATERIAL_PATH = "Materials/Basic";
        private const string GATE_IMAGES_POSITIVE = "Sprites/Images/Positive";
        private const string GATE_IMAGES_NEGATIVE = "Sprites/Images/Negative";
        private const string LEVEL_GAMES_PATH = "Objects/";

        private List<Material> _chipMaterials;
        private List<Material> _basicMaterials;
        private List<GameObject> _prefabObjects;
        private List<Sprite> _gateImagesPositive;
        private List<Sprite> _gateImagesNegative;
        private List<LevelGame> _levelGames;

        public AssetManager()
        {
            _chipMaterials = Resources.LoadAll<Material>(CHIP_MATERIAL_PATH).ToList();
            _basicMaterials = Resources.LoadAll<Material>(BASIC_MATERIAL_PATH).ToList();
            _prefabObjects = Resources.LoadAll<GameObject>(OBJECT_PATH).ToList();
            _gateImagesPositive = Resources.LoadAll<Sprite>(GATE_IMAGES_POSITIVE).ToList();
            _gateImagesNegative = Resources.LoadAll<Sprite>(GATE_IMAGES_NEGATIVE).ToList();
            _levelGames = Resources.LoadAll<LevelGame>(LEVEL_GAMES_PATH).ToList();
        }

        public Sprite GetGatePositiveImage(string name)
        {
            return _gateImagesPositive?.FirstOrDefault(x => x.name == name);
        }
        
        public Sprite GetGateNegativeImage(string name)
        {
            return _gateImagesNegative?.FirstOrDefault(x => x.name == name);
        }

        public Material GetBasicMaterial(string name)
        {
            return _basicMaterials?.FirstOrDefault(x => x.name == name);
        }

        public Material GetChipMaterial(string name)
        {
            return _chipMaterials?.FirstOrDefault(x => x.name == name);
        }

        public GameObject GetPrefabObject(string name)
        {
            return _prefabObjects?.FirstOrDefault(x => x.name == name).gameObject;
        }

        public LevelGame GetLevelGame(LevelGameType levelGameType)
        {
            return _levelGames?.FirstOrDefault(x => x.levelGameType == levelGameType);
        }
    }
}
