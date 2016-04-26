using UnityEngine;
using System.Collections;

namespace spawn
{
    public class PowerupSpawner : Spawner
    {
        [SerializeField]
        private float m_SpawnWhenTimeElapsed = 20.0f;

        private float m_ElapsedTime = 0.0f;

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

        protected override void OnSpawn(GameObject spawn)
        {

        }
    }
}