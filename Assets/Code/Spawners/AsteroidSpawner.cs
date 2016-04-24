using UnityEngine;
using System.Collections;

namespace spawn
{
    public class AsteroidSpawner : Spawner
    {
        [SerializeField]
        private float m_SpawnWaitTime = 1.0f;

        private float m_ElapsedTime = 0.0f;

        [SerializeField]
        private int m_MaxSpawnCount = 10;

        [SerializeField]
        private int m_IncreaseSpawnCountByScore = 10000;

        [SerializeField]
        private GameObject m_MiniPrefab = null;

        [SerializeField]
        private float m_MinimumVelocityScalar = 0.65f;

        [SerializeField]
        private float m_MaximumVelocityScalar = 1.0f;

        override protected bool CanSpawn()
        {
            if (m_ElapsedTime < m_SpawnWaitTime)
            {
                m_ElapsedTime += Time.deltaTime;
                return false;
            }

            m_ElapsedTime = 0.0f;

            return true;
        }

        override protected int GetSpawnCount()
        {
            int numToSpawn = m_NumberToSpawn;
            numToSpawn += entity.Player.Score / m_IncreaseSpawnCountByScore;
            numToSpawn = Mathf.Min(numToSpawn, m_MaxSpawnCount);

            return numToSpawn;
        }

        public void SpawnMiniAsteroid(Vector3 position, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                Camera cam = Camera.main;

                Quaternion rotation = Quaternion.identity;
                GameObject spawn = GameObject.Instantiate(m_MiniPrefab, position, rotation) as GameObject;
                spawn.transform.parent = transform;

                entity.Asteroid asteroid = spawn.GetComponent<entity.Asteroid>();
                asteroid.ConstantVelocity = asteroid.ConstantVelocity * GetVelocityScalar();
                asteroid.Spawner = null; // set the spawner to null so we dont spawn more asteroids on death
            }
        }

        override protected void OnSpawn(GameObject spawn)
        {
            entity.Asteroid asteroid = spawn.GetComponent<entity.Asteroid>();
            asteroid.ConstantVelocity = asteroid.ConstantVelocity * GetVelocityScalar();
            asteroid.Spawner = this;
        }

        private float GetVelocityScalar()
        {
            float scalar = 0.0f;
            if (Random.Range(0, 2) == 0)
            {
                scalar = Random.Range(m_MinimumVelocityScalar, m_MaximumVelocityScalar);
            }
            else
            {
                scalar = Random.Range(-m_MaximumVelocityScalar, -m_MinimumVelocityScalar);
            }

            return scalar;
        }
    }
}