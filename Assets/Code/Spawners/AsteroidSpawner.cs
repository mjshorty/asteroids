using UnityEngine;
using System.Collections;

namespace spawn
{
    public class AsteroidSpawner : Spawner
    {
        [SerializeField]
        private GameObject m_MiniPrefab = null;

        [SerializeField]
        private float m_MinimumVelocityScalar = 0.65f;

        [SerializeField]
        private float m_MaximumVelocityScalar = 1.0f;

        public void SpawnMiniAsteroid(Vector3 position, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                Camera cam = Camera.main;

                Quaternion rotation = Quaternion.identity;
                GameObject spawn = GameObject.Instantiate(m_MiniPrefab, position, rotation) as GameObject;
                spawn.transform.parent = transform;

                entity.Asteroid asteroid = spawn.GetComponent<entity.Asteroid>();

                float scalar = 0.0f;
                if(Random.Range(0, 2) == 0)
                {
                    scalar = Random.Range(m_MinimumVelocityScalar, m_MaximumVelocityScalar);
                }
                else
                {
                    scalar = Random.Range(-m_MaximumVelocityScalar, -m_MinimumVelocityScalar);
                }

                asteroid.ConstantVelocity = asteroid.ConstantVelocity * scalar;
            }
        }

        override protected void OnSpawn(GameObject spawn)
        {
            entity.Asteroid asteroid = spawn.GetComponent<entity.Asteroid>();
            asteroid.ConstantVelocity = asteroid.ConstantVelocity * Random.Range(-1.0f, 1.0f);
        }
    }
}