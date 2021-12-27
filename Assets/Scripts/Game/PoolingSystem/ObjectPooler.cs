using System.Collections.Generic;
using System.Linq;
using Game.LevelSystem.Managers;
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

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

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

            GameObject parent = new GameObject("ChipParent");
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

        public void DeactivatePool(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.Log("Pool with tag " + tag + " doesn't exist.");
                return;
            }

            foreach (var obj in poolDictionary[tag])
            {
                obj.SetActive(true);
                obj.transform.position = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
            }
        }
    }
}