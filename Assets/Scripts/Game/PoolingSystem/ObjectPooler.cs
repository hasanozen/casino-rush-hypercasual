using System.Collections.Generic;
using System.Linq;
using Game.LevelSystem.Base;
using Game.LevelSystem.Managers;
using Game.MiniGames.Base;
using UnityEngine;
using Zenject;

namespace Game.PoolingSystem
{
    public class ObjectPooler : MonoBehaviour
    {
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        public class LevelGamePool
        {
            public string tag;
            public List<LevelGame> levelGames;
        }

        public List<Pool> pools;
        public LevelGamePool levelGamePool;
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        public Dictionary<string, Queue<GameObject>> levelPoolDictionary;

        private AssetManager _assetManager;

        [Inject]
        private void OnInitalize(AssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        public void Init()
        {
            pools = new List<Pool>();
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            levelPoolDictionary = new Dictionary<string, Queue<GameObject>>();
            
            levelGamePool = new LevelGamePool
            {
                tag = "LevelGames",
                levelGames = new List<LevelGame>()
            };
        }

        public void CreatePool(string tag, GameObject gameObject, int size)
        {
            Pool pool = new Pool
            {
                tag = tag,
                prefab = gameObject,
                size = size
            };
            
            pools.Add(pool);
        }

        public Pool GetPool(string tag)
        {
            return pools?.FirstOrDefault(x => x.tag == tag);
        }

        public Queue<GameObject> GetObjectsFromDictionary(string tag)
        {
            return poolDictionary?.FirstOrDefault(x => x.Key == tag).Value;
        }

        public void SpawnObjectsWithTag(string tag)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            Pool pool = pools?.FirstOrDefault(x => x.tag == tag);

            GameObject parent = new GameObject(tag + "Parent");
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, parent.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.tag, objectPool);
        }

        public GameObject SpawnFromPool(string tag, Vector3 position)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.Log("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            //objectToSpawn.transform.rotation = rotation;

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public void AddLevelGame(LevelGame levelGame)
        {
            levelGamePool.levelGames.Add(levelGame);
        }

        public LevelGame GetLevelGame(LevelGameType levelGameType)
        {
            LevelGame game = levelPoolDictionary[levelGamePool.tag]
                .Where(x => x.GetComponent<LevelGame>().levelGameType == levelGameType).FirstOrDefault().GetComponent<LevelGame>();

            return game;
        }

        public void SpawnLevelGames()
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < levelGamePool.levelGames.Count; i++)
            {
                GameObject game = Instantiate(levelGamePool.levelGames[i].gameObject);
                game.SetActive(false);
                
                objectPool.Enqueue(game);
            }
            
            levelPoolDictionary.Add(levelGamePool.tag, objectPool);
        }

        public void DeactivatePool(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.Log("Pool with tag " + tag + " doesn't exist.");
                return;
            }

            foreach (var obj in poolDictionary[tag])
            {
                obj.SetActive(false);
                obj.transform.position = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
            }
        }

        public void DeactivateWholePool()
        {
            var keys = poolDictionary.Keys.ToArray();
            
            for (int i = 0; i < keys.Length; i++)
                DeactivatePool(keys[i]);
            
            DeactivateLevelGames();
        }

        public void ActivateLevelGame(LevelGameType levelGameType)
        {
            LevelGame game = GetLevelGame(levelGameType);
            game.gameObject.SetActive(true);
        }

        public void DeactivateLevelGames()
        {
            if (!levelPoolDictionary.ContainsKey(levelGamePool.tag))
            {
                Debug.Log("Pool with tag " + levelGamePool.tag + " doesn't exist.");
                return;
            }
            
            foreach (var obj in levelPoolDictionary[levelGamePool.tag])
            {
                obj.SetActive(false);
                obj.transform.position = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
            }
        }
    }
}