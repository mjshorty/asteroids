using UnityEngine;
using System.Collections;

namespace spawn
{
    public class SpaceshipSpawner : Spawner
    {
        [SerializeField]
        private int m_SpawnWhenScoreIsMultipleOf = 10000;

        private int m_SpawnCount = 0;

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

        protected override void OnSpawn(GameObject spawn)
        {
            
        }
    }
}