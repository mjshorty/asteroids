using UnityEngine;
using System.Collections;

namespace spawn
{
    /// <summary>
    /// Spawner for pwoerup entities
    /// </summary>
    public class PowerupSpawner : Spawner
    {
        /// <summary>
        /// The time between powerup spawns
        /// </summary>
        [SerializeField]
        private float m_SpawnWhenTimeElapsed = 20.0f;

        /// <summary>
        /// The elasped time since we last spawned a powerup
        /// </summary>
        private float m_ElapsedTime = 0.0f;

        /// <summary>
        /// Check to see if we can spawn a new powerup
        /// </summary>
        /// <returns>Ready to spawn?</returns>
        protected override bool CanSpawn()
        {
            m_ElapsedTime += Time.deltaTime;
            if(m_ElapsedTime >= m_SpawnWhenTimeElapsed)
            {
                m_ElapsedTime = 0.0f;
                return true;
            }

            return false;
        }
    }
}