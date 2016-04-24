using UnityEngine;
using System.Collections;

namespace entity
{
    public class Asteroid : Entity
    {
        [SerializeField]
        private Vector3 m_ConstantVelocity = Vector3.one;

        [SerializeField]
        private int m_AwardOnKill = 1000;

        [SerializeField]
        private GameObject m_MiniPrefab = null;

        public spawn.AsteroidSpawner Spawner { get; set; }

        public Vector3 ConstantVelocity { get { return m_ConstantVelocity; } set { m_ConstantVelocity = value; } }

        override protected void UpdateEntity()
        {
            m_Velocity = m_ConstantVelocity;
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            Player.Score += m_AwardOnKill;

            if (Spawner)
            {
                Spawner.SpawnMiniAsteroid(m_MiniPrefab, transform.position, 4);
            }
        }
    }
}