using UnityEngine;
using System.Collections;

namespace spawn
{
    /// <summary>
    /// Spawner used to create asteroids
    /// </summary>
    public class AsteroidSpawner : Spawner
    {
        /// <summary>
        /// The time to wait between spawns
        /// </summary>
        [SerializeField]
        private float m_SpawnWaitTime = 1.0f;

        /// <summary>
        /// The elapsed spawn time
        /// </summary>
        private float m_ElapsedTime = 0.0f;

        /// <summary>
        /// The maximum number of asteroids we can spawn
        /// </summary>
        [SerializeField]
        private int m_MaxSpawnCount = 10;

        /// <summary>
        /// Increase the spawn count by one every time the players score exceeds this value
        /// </summary>
        [SerializeField]
        private int m_IncreaseSpawnCountByScore = 10000;

        /// <summary>
        /// The minimum veocity of a newly spawned asteroid
        /// </summary>
        [SerializeField]
        private float m_MinimumVelocityScalar = 0.65f;

        /// <summary>
        /// The maximum veocity of a newly spawned asteroid
        /// </summary>
        [SerializeField]
        private float m_MaximumVelocityScalar = 1.0f;

        /// <summary>
        /// Can we spawn a new asteroid?
        /// </summary>
        /// <returns>Ready to spawn?</returns>
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

        /// <summary>
        /// Get the number of asteroids to spawn
        /// </summary>
        /// <returns>The number of asteroids to spawn</returns>
        override protected int GetSpawnCount()
        {
            int numToSpawn = m_NumberToSpawn;
            numToSpawn += game.Score.Instance.CurrentScore / m_IncreaseSpawnCountByScore;
            numToSpawn = Mathf.Min(numToSpawn, m_MaxSpawnCount);

            return numToSpawn;
        }

        /// <summary>
        /// Spawn a mini asteroid on te death of a bit asteroid
        /// </summary>
        /// <param name="prefab">The mini asteroid prefab</param>
        /// <param name="position">The position of the new asteroid</param>
        /// <param name="count">The number of mini asteroids to spawn</param>
        public void SpawnMiniAsteroid(GameObject prefab, Vector3 position, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                Camera cam = Camera.main;

                Quaternion rotation = Quaternion.identity;
                GameObject spawn = utils.Pool.Instance.Create(prefab, position, transform);
                spawn.transform.rotation = rotation;

                entity.Asteroid asteroid = spawn.GetComponent<entity.Asteroid>();
                asteroid.ConstantVelocity = asteroid.ConstantVelocity * GetVelocityScalar();
                asteroid.Spawner = null; // set the spawner to null so we dont spawn more asteroids on death

                spawn.SpawnOffset offset = spawn.GetComponent<spawn.SpawnOffset>();
                if(offset)
                {
                    float xOffset = i == 0 || i == 1 ? -1.0f : 1.0f;
                    float yOffset = i == 1 || i == 2 ? -1.0f : 1.0f;

                    offset.ApplyOffset(xOffset, yOffset, (float)i);
                }
            }
        }

        /// <summary>
        /// Called when a new asteroid is spawned
        /// </summary>
        /// <param name="spawn">The spawned asteroid</param>
        override protected void OnSpawn(GameObject spawn)
        {
            entity.Asteroid asteroid = spawn.GetComponent<entity.Asteroid>();
            asteroid.ConstantVelocity = asteroid.ConstantVelocity * GetVelocityScalar();
            asteroid.Spawner = this;
        }

        /// <summary>
        /// Get the velocity scalar when spawning a enw asteroid
        /// </summary>
        /// <returns>The velocity scalar</returns>
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