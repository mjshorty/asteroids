using UnityEngine;
using System.Collections;

namespace entity
{
    /// <summary>
    /// The asteroid entity class
    /// </summary>
    public class Asteroid : Entity
    {
        /// <summary>
        /// The constant velocity of the asteroid
        /// </summary>
        [SerializeField]
        private Vector3 m_ConstantVelocity = Vector3.one;

        /// <summary>
        /// The number of points awarded to the player upon killing this asteroid
        /// </summary>
        [SerializeField]
        private int m_AwardOnKill = 1000;

        /// <summary>
        /// The optional mini prefab to spawn when this entity dies
        /// </summary>
        [SerializeField]
        private GameObject m_MiniPrefab = null;

        /// <summary>
        /// The asteroid spawner
        /// </summary>
        public spawn.AsteroidSpawner Spawner { get; set; }

        /// <summary>
        /// Get the constant asteroid velocity
        /// </summary>
        public Vector3 ConstantVelocity { get { return m_ConstantVelocity; } set { m_ConstantVelocity = value; } }

        /// <summary>
        /// Update the asteroids velocity
        /// </summary>
        override protected void UpdateEntity()
        {
            m_Velocity = m_ConstantVelocity;
        }

        /// <summary>
        /// On the death of the asteroid award the player points
        /// and optionally spawn minature asteroids
        /// </summary>
        /// <param name="gameOver">Due to a game over?</param>
        /// <returns>Trigger delayed death?</returns>
        protected override bool OnDeath(bool gameOver)
        {
            if (!gameOver)
            {
                game.Score.Instance.Award(m_AwardOnKill);

                if (Spawner)
                {
                    Spawner.SpawnMiniAsteroid(m_MiniPrefab, transform.position, 4);
                }
            }

            return false;
        }
    }
}