using UnityEngine;
using System.Collections;

namespace spawn
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Prefab = null;

        [SerializeField]
        private Vector2 SpawnRange = new Vector2(500.0f, 500.0f);

        [SerializeField]
        private int m_NumberToSpawn = 3;

        [SerializeField]
        private int m_MaxSpawnCount = 10;

        [SerializeField]
        private int m_IncreaseSpawnCountByScore = 1000;

        [SerializeField]
        private float m_SpawnWaitTime = 1.0f;

        private float m_ElapsedTime = 0.0f;

        // Update is called once per frame
        void Update()
        {
            int count = transform.childCount;
            if (count != 0)
            {
                return;
            }

            if (m_ElapsedTime < m_SpawnWaitTime)
            {
                m_ElapsedTime += Time.deltaTime;
                return;
            }

            m_ElapsedTime = 0.0f;

            int numToSpawn = m_NumberToSpawn;
            //numToSpawn += currentScore % m_IncreaseSpawnCountByScore;
            numToSpawn = Mathf.Min(numToSpawn, m_MaxSpawnCount);

            Spawn(numToSpawn, m_Prefab);
        }

        protected void Spawn(int count, GameObject prefab)
        {
            for (int i = 0; i < count; ++i)
            {
                Camera cam = Camera.main;
                Vector3 position = Vector3.zero;

                position.x = Random.Range(-SpawnRange.x, SpawnRange.x);
                position.y = Random.Range(-SpawnRange.y, SpawnRange.y);

                Quaternion rotation = Quaternion.identity;
                GameObject spawn = GameObject.Instantiate(m_Prefab, position, rotation) as GameObject;
                spawn.transform.parent = transform;

                OnSpawn(spawn);
            }
        }

        virtual protected void OnSpawn(GameObject spawn) { }
    }
}