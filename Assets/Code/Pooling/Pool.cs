using UnityEngine;
using System.Collections.Generic;

namespace utils
{
    /// <summary>
    /// A prefab pool specification
    /// </summary>
    [System.Serializable]
    public class PoolSpec
    {
        /// <summary>
        /// The prefab to pool
        /// </summary>
        public GameObject m_Prefab = null;

        /// <summary>
        /// The number of times to clone the prefab
        /// </summary>
        public int m_PoolSize = 5;
    }

    /// <summary>
    /// An entry into the prefab pool
    /// </summary>
    public class PoolEntry
    {
        public GameObject m_GO = null;
        public bool m_InUse = false;
    }

    /// <summary>
    /// The pool is used to create and recycle game objects
    /// this is done to speed up creating new objects at runtime
    /// as it tends to be a very slow process
    /// </summary>
    public class Pool : Singleton<Pool>
    {
        /// <summary>
        /// The prefabs we want to pool
        /// </summary>
        [SerializeField]
        private List<PoolSpec> m_Prefabs = new List<PoolSpec>();

        /// <summary>
        /// The container of prefab pools
        /// </summary>
        private Dictionary<int, List<PoolEntry>> m_Pool = new Dictionary<int, List<PoolEntry>>();

        /// <summary>
        /// Create all of our prefab pools
        /// </summary>
        void Start()
        {
            foreach(var poolEntry in m_Prefabs)
            {
                List<PoolEntry> pool = new List<PoolEntry>(poolEntry.m_PoolSize);
                for(int i = 0; i < poolEntry.m_PoolSize; ++i)
                {
                    PoolEntry entry = CreatePoolEntry(poolEntry.m_Prefab);
                    pool.Add(entry);
                }

                m_Pool.Add(poolEntry.m_Prefab.GetInstanceID(), pool);
            }
        }

        /// <summary>
        /// Create a game object from a prefab
        /// </summary>
        /// <param name="prefab">The prefab to clone</param>
        /// <param name="position">The position to place the object</param>
        /// <param name="parent">The new parent to assign to the game object</param>
        /// <returns>The cloned game object</returns>
        public GameObject Create(GameObject prefab, Vector3 position, Transform parent = null)
        {
            Debug.Assert(prefab != null);

            List<PoolEntry> pool = null;
            PoolEntry poolEntry = null;

            int id = prefab.GetInstanceID();
            if(m_Pool.ContainsKey(id))
            {
                pool = m_Pool[id];
                foreach (var entry in pool)
                {
                    if (!entry.m_InUse)
                    {
                        poolEntry = entry;
                        break;
                    }
                }
            }
            else
            {
                pool = new List<PoolEntry>();
                m_Pool.Add(prefab.GetInstanceID(), pool);
            }

            if (poolEntry == null)
            {
                poolEntry = CreatePoolEntry(prefab);
                pool.Add(poolEntry);
            }

            poolEntry.m_InUse = true;

            if (parent != null)
            {
                poolEntry.m_GO.transform.parent = parent;
            }

            poolEntry.m_GO.transform.position = position;

            poolEntry.m_GO.SetActive(true);
            SendMessage(poolEntry.m_GO, "Awake", true);
            SendMessage(poolEntry.m_GO, "Start", true);

            return poolEntry.m_GO;
        }

        /// <summary>
        /// Destroy a game object, returning it to the pool
        /// </summary>
        /// <param name="go">The game object to destroy</param>
        public void Destroy(GameObject go)
        {
            Debug.Assert(go != null);

            PooledObject pooledObject = go.GetComponent<PooledObject>();
            if(pooledObject == null)
            {
                GameObject.Destroy(go);
            }
            else
            {
                int poolID = pooledObject.PoolID;
                var pool = m_Pool[poolID];
                if(pool == null)
                {
                    GameObject.Destroy(go);
                }
                else
                {
                    foreach(var entry in pool)
                    {
                        if(entry.m_GO.GetInstanceID() == go.GetInstanceID())
                        {
                            entry.m_InUse = false;

                            SendMessage(go, "OnDestroy", false);

                            go.SetActive(false);
                            go.transform.parent = transform;

                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create a new pool entry
        /// </summary>
        /// <param name="prefab">The prefab to clone</param>
        /// <returns>The cloned game object</returns>
        private PoolEntry CreatePoolEntry(GameObject prefab)
        {
            PoolEntry entry = new PoolEntry();
            entry.m_GO = GameObject.Instantiate(prefab);
            entry.m_GO.SetActive(false);
            entry.m_GO.transform.parent = transform;

            entry.m_InUse = false;

            PooledObject pooledObject = entry.m_GO.AddComponent<PooledObject>();
            pooledObject.PoolID = prefab.GetInstanceID();

            return entry;
        }

        /// <summary>
        /// Set a message recursivley to all game objects in the heirarchy
        /// </summary>
        /// <param name="go">The parent game object</param>
        /// <param name="message">the message (function name)</param>
        /// <param name="onlyActive">Only send to active game objects?</param>
        private void SendMessage(GameObject go, string message, bool onlyActive)
        {
            if (!onlyActive || go.activeSelf)
            {
                go.SendMessage(message, null, SendMessageOptions.DontRequireReceiver);
            }

            int childCount = go.transform.childCount;
            for(int i = 0; i < childCount; ++i)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                SendMessage(child, message, onlyActive);
            }
        }
    }
}