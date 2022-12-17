using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class PoolInfo
    {
        public GameObject prefabs;
        public GameObject collection;
        public int initialSize;
        public string tagName;

        public Queue<GameObject> CreatePool()
        {
            Queue<GameObject> pools = new Queue<GameObject>();
            collection = new GameObject(tagName + "Collection");
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefabs, collection.transform);
                obj.SetActive(false);
                pools.Enqueue(obj);
            }
            return pools;
        }
    }

    public List<PoolInfo> poolsInfo;
    private Dictionary<string, Queue<GameObject>> pools;
    void Start()
    {
        pools = new Dictionary<string, Queue<GameObject>>();
        foreach (PoolInfo poolInfo in poolsInfo)
        {
            pools.Add(poolInfo.tagName, poolInfo.CreatePool());
        }
    }

    void Update()
    {

    }
}
