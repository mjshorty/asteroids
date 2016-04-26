using UnityEngine;
using System.Collections;

namespace entity
{
    public class SpecialWeapon : MonoBehaviour
    {
        [SerializeField]
        private int m_Damage = 200;

        [SerializeField]
        private float m_Radius = 300.0f;

        [SerializeField]
        private GameObject m_EffectPrefab = null;

        spawn.AsteroidSpawner m_AsteroidSpawner = null;
        spawn.SpaceshipSpawner m_SpaceshipSpawner = null;

        void Start()
        {
            m_AsteroidSpawner = FindObjectOfType<spawn.AsteroidSpawner>();
            m_SpaceshipSpawner = FindObjectOfType<spawn.SpaceshipSpawner>();
        }

        private void KillAsteroids(GameObject go)
        {
            entity.Asteroid asteroid = go.GetComponent<Asteroid>();
            if(asteroid == null)
            {
                return;
            }

            float sqrDst = Vector3.SqrMagnitude(transform.position - go.transform.position);
            if(sqrDst < (m_Radius * m_Radius))
            {
                asteroid.Lives = 0;
            }
        }

        private void DamageSpaceships(GameObject go)
        {
            entity.Enemy enemy = go.GetComponent<Enemy>();
            if (enemy == null)
            {
                return;
            }

            float sqrDst = Vector3.SqrMagnitude(transform.position - go.transform.position);
            if (sqrDst < (m_Radius * m_Radius))
            {
                float scale = 1.0f - Mathf.Clamp01(sqrDst / (m_Radius * m_Radius));
                int damage = (int)((float)m_Damage * scale);
                enemy.ApplyDamage(damage);
            }
        }

        public void Fire()
        {
            if(m_AsteroidSpawner)
            {
                m_AsteroidSpawner.ForEachSpawn(KillAsteroids);
            }

            if(m_SpaceshipSpawner)
            {
                m_SpaceshipSpawner.ForEachSpawn(DamageSpaceships);
            }

            grid.Grid.Instance.ApplyExplosiveForce(m_Damage, transform.position, m_Radius);

            if (m_EffectPrefab)
            {
                GameObject effectGO = utils.Pool.Instance.Create(m_EffectPrefab);
                effectGO.transform.position = transform.position;
            }

            utils.Pool.Instance.Destroy(gameObject);
        }
    }
}