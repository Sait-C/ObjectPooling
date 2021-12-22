using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion 
    public List<Pool> pools; //it store our all pools data
    public Dictionary<string, Queue<GameObject>> poolDictionary; //it store all pools with a tag

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>(); //initialize poolDictionary

        foreach (var pool in pools) //iterating for all pools
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); //it is a temp pool

            for(int i = 0; i < pool.size; i++) //help spawn prefab the size of the pool
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool); //add to dictionary to our pool and store in this dictionary
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag)) //Error checking
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue(); //it takes a object from our pool 

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>(); //This allows the corresponding function on each object inherited from this interface to be called.
        if(pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
            
        poolDictionary[tag].Enqueue(objectToSpawn); //add back to pool

        return objectToSpawn;
    }
}
