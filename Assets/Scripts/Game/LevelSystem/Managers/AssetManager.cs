using System.Collections.Generic;
using System.Linq;
using Config;
using Game.ChipSystem.Base;
using UnityEngine;

namespace Game.LevelSystem.Managers
{
    public class AssetManager
    {
        private const string OBSTACLE_PATH = "Obstacles/";
        private const string PLATFORM_PATH = "Platforms/";
        private const string OBJECT_PATH = "Objects/";
        private const string MATERIAL_PATH = "Materials/Chip";

        private List<Material> _materials;
        private List<GameObject> _prefabObjects;

        public AssetManager()
        {
            _materials = Resources.LoadAll<Material>(MATERIAL_PATH).ToList();
            _prefabObjects = Resources.LoadAll<GameObject>(OBJECT_PATH).ToList();
        }

        public Material GetMaterial(string name)
        {
            return _materials?.FirstOrDefault(x => x.name == name);
        }

        public GameObject GetPrefabObject(string name)
        {
            return _prefabObjects?.FirstOrDefault(x => x.name == name).gameObject;
        }
    }
}
