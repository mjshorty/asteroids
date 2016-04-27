using UnityEngine;
using System.Collections;

namespace spawn
{
    /// <summary>
    /// Spawn a new spaceship
    /// </summary>
    public class SpaceshipSpawner : Spawner
    {
        /// <summary>
        /// When the players score hits a multiple of this, spawn a new spaceship
        /// </summary>
        [SerializeField]
        private int m_SpawnWhenScoreIsMultipleOf = 10000;

        /// <summary>
        /// The number of times we have spawned a spaceship
        /// </summary>
        private int m_SpawnCount = 0;

        /// <summary>
        /// Check to see if we are ready to spawn a new spaceship
        /// </summary>
        /// <returns>Ready to spawn?</returns>
        protected override bool CanSpawn()
        {
            int spawnModulus = game.Score.Instance.CurrentScore / m_SpawnWhenScoreIsMultipleOf;

            if (spawnModulus > m_SpawnCount)
            {
                ++m_SpawnCount;
                return true;
            }

            return false;
        }
    }
}