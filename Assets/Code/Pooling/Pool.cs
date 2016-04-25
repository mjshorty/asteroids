using UnityEngine;
using System.Collections.Generic;

namespace utils
{
    [System.Serializable]
    public class PoolSpec
    {
        public GameObject m_Prefab = null;
        public int m_PoolSize = 5;
    }

    public class PoolEntry
    {
        public GameObject m_GO = null;
        public bool m_InUse = false;
    }

    public class Pool : Singleton<Pool>
    {
        [SerializeField]
        private List<PoolSpec> m_Prefabs = new List<PoolSpec>();

        private Dictionary<int, List<PoolEntry>> m_Pool = new Dictionary<int, List<PoolEntry>>();

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

        public GameObject Create(GameObject prefab)
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

            poolEntry.m_GO.SetActive(true);
            SendMessage(poolEntry.m_GO, "Awake", true);
            SendMessage(poolEntry.m_GO, "Start", true);

            return poolEntry.m_GO;
        }

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
                        if(entry.m_GO == go)
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

        private void SendMessage(GameObject go, string message, bool onlyActive)
        {
            int childCount = go.transform.childCount;
            for(int i = 0; i < childCount; ++i)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                if (!onlyActive || child.activeSelf)
                {
                    child.SendMessage(message, null, SendMessageOptions.DontRequireReceiver);
                }

                SendMessage(child, message, onlyActive);
            }
        }
    }
}