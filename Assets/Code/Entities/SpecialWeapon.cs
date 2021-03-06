﻿using UnityEngine;
using System.Collections;

namespace entity
{
    /// <summary>
    /// A special weapon that the player can use only once
    /// </summary>
    public class SpecialWeapon : MonoBehaviour
    {
        /// <summary>
        /// The ammount of damage that this weapon will inflict
        /// Note that this damage is reduced the further you are
        /// from the initial trigger position
        /// </summary>
        [SerializeField]
        private int m_Damage = 200;

        /// <summary>
        /// The radius of the special weapon
        /// </summary>
        [SerializeField]
        private float m_Radius = 300.0f;
        
        /// <summary>
        /// The effect prefab we will spawn when we trigger the special weapon
        /// </summary>
        [SerializeField]
        private GameObject m_EffectPrefab = null;
        
        /// <summary>
        /// The asteroid spawner
        /// </summary>
        spawn.AsteroidSpawner m_AsteroidSpawner = null;

        /// <summary>
        /// The spacesip spawner
        /// </summary>
        spawn.SpaceshipSpawner m_SpaceshipSpawner = null;

        void Start()
        {
            // Slow calls but we have no other way of aqcuiring handles to these spawners
            // maybe change them to singletons in future
            m_AsteroidSpawner = FindObjectOfType<spawn.AsteroidSpawner>();
            m_SpaceshipSpawner = FindObjectOfType<spawn.SpaceshipSpawner>();
        }

        /// <summary>
        /// Kill all asteroids in range
        /// </summary>
        /// <param name="go">The asteroid game object</param>
        private void KillEntity(GameObject go)
        {
            Entity entity = go.GetComponent<Asteroid>() as Entity;
            if (entity == null)
            {
                entity = go.GetComponent<Enemy>() as Entity;
            }

            if (entity != null)
            {
                float sqrDst = Vector3.SqrMagnitude(transform.position - go.transform.position);
                if (sqrDst < (m_Radius * m_Radius))
                {
                    entity.Lives = 0;
                }
            }
        }

        /// <summary>
        /// Fire the weapon
        /// </summary>
        public void Fire()
        {
            if(m_AsteroidSpawner)
            {
                m_AsteroidSpawner.ForEachSpawn(KillEntity);
            }

            if(m_SpaceshipSpawner)
            {
                m_SpaceshipSpawner.ForEachSpawn(KillEntity);
            }

            grid.Grid.Instance.ApplyExplosiveForce(m_Damage, transform.position, m_Radius);

            if (m_EffectPrefab)
            {
                GameObject effectGO = utils.Pool.Instance.Create(m_EffectPrefab, transform.position);
            }

            utils.Pool.Instance.Destroy(gameObject);
        }
    }
}