using UnityEngine;
using System.Collections.Generic;

namespace spawn
{
    /// <summary>
    /// Base spawner class
    /// Spawns entities
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        /// <summary>
        /// The lsit of prefabs to spawn
        /// </summary>
        [SerializeField]
        private List<GameObject> m_Prefabs = new List<GameObject>();

        /// <summary>
        /// Should we spawn a random prefab from the list or all of the rpefabs in the list?
        /// </summary>
        [SerializeField]
        private bool m_RandomPrefab = true;

        /// <summary>
        /// The spawner range
        /// </summary>
        [SerializeField]
        private Vector2 SpawnRange = new Vector2(500.0f, 500.0f);

        /// <summary>
        /// The number of entities to spawn
        /// </summary>
        [SerializeField]
        protected int m_NumberToSpawn = 3;

        /// <summary>
        /// Delegate to execute over each spawned entity
        /// </summary>
        /// <param name="go">The spawned entity</param>
        public delegate void DoForEach(GameObject go);

        /// <summary>
        /// Execute a delegate over each spawned entity
        /// </summary>
        /// <param name="forEach">The delegate to run</param>
        public void ForEachSpawn(DoForEach forEach)
        {
            int i = transform.childCount - 1;
            for(; i > 0; --i)
            {
                Transform child = transform.GetChild(i);
                forEach(child.gameObject);
            }
        }

        /// <summary>
        /// Kill all entities spawned by this spawner
        /// </summary>
        public void KillAll()
        {
            while (transform.childCount > 0)
            {
                int i = transform.childCount - 1;
                GameObject go = transform.GetChild(i).gameObject;

                entity.Entity entityGO = go.GetComponent<entity.Entity>();
                if(entityGO)
                {
                    entityGO.GameOverDeath();
                }

                utils.Pool.Instance.Destroy(go);
            }

            enabled = false;    
        }

        /// <summary>
        /// Clean up when we are destroyed
        /// </summary>
        void OnDestroy()
        {
            while(transform.childCount > 0)
            {
                int i = transform.childCount - 1;
                GameObject go = transform.GetChild(i).gameObject;

                if (utils.Pool.IsValid)
                {
                    utils.Pool.Instance.Destroy(go);
                }
            }
        }

        
        /// <summary>
        /// Update the spawner, spawning new entites when required
        /// </summary>
        void Update()
        {
            int count = transform.childCount;
            if (count != 0)
            {
                return;
            }

            if(CanSpawn() == false)
            {
                return;
            }

            Spawn(GetSpawnCount(), m_Prefabs);
        }

        /// <summary>
        /// Spawn new entity/entities
        /// </summary>
        /// <param name="count">The number of entities to spawn</param>
        /// <param name="prefabs">The prafabs to spawn</param>
        protected void Spawn(int count, List<GameObject> prefabs)
        {
            for (int i = 0; i < count; ++i)
            {
                if(m_RandomPrefab)
                {
                    GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
                    Spawn(prefab);
                }
                else
                {
                    foreach (GameObject prefab in prefabs)
                    {
                        Spawn(prefab);
                    }
                }
            }
        }

        /// <summary>
        /// Spawn a new entity based on the provided prefab
        /// </summary>
        /// <param name="prefab">The prefab to spawn</param>
        private void Spawn(GameObject prefab)
        {
            Camera cam = Camera.main;
            Vector3 position = Vector3.zero;

            position.x = Random.Range(-SpawnRange.x, SpawnRange.x);
            position.y = Random.Range(-SpawnRange.y, SpawnRange.y);

            Quaternion rotation = Quaternion.identity;
            GameObject spawn = utils.Pool.Instance.Create(prefab, position, transform);
            spawn.transform.rotation = rotation;

            OnSpawn(spawn);
        }

        /// <summary>
        /// Called when a new entity is spawned
        /// </summary>
        /// <param name="spawn">The spawned entity</param>
        virtual protected void OnSpawn(GameObject spawn) { }

        /// <summary>
        /// Can the spawner currently spawn new entities?
        /// </summary>
        /// <returns>Ready to spawn?</returns>
        virtual protected bool CanSpawn()
        {
            return false;
        }

        /// <summary>
        /// Get the number of entities to spawn
        /// </summary>
        /// <returns>The number of entities to spawn</returns>
        virtual protected int GetSpawnCount()
        {
            return m_NumberToSpawn;
        }
    }
}