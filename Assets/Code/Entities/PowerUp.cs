using UnityEngine;
using System.Collections;

namespace entity
{
    /// <summary>
    /// A special powerup that will appear in the game from time to time
    /// to help the player
    /// </summary>
    public class PowerUp : Entity
    {
        /// <summary>
        /// The prefab we will assign to the player
        /// </summary>
        [SerializeField]    
        protected SpecialWeapon m_PowerUpPrefab = null;

        /// <summary>
        /// The speed at which this powerup will rotate
        /// </summary>
        [SerializeField]
        private float m_RotateSpeed = 1.0f;

        /// <summary>
        /// How long the powerup will be alive for
        /// </summary>
        [SerializeField]
        private float m_TimeAlive = 5.0f;

        /// <summary>
        /// The elapsed time since spawning
        /// </summary>
        private float m_Elapsedtime = 0.0f;

        /// <summary>
        /// Award the game object a power up 
        /// </summary>
        /// <param name="player">The player to give the powerup to</param>
        virtual protected void AwardPowerUp(Player player) { }

        /// <summary>
        /// Handle collision with the player
        /// </summary>
        /// <param name="collision">The collision data</param>
        void OnTriggerEnter(Collider collision)
        {
            GameObject collider = collision.gameObject;
            if (collider.tag == "Player")
            {
                Player player = collider.GetComponent<Player>();
                if(player != null)
                {
                    AwardPowerUp(player);
                }
            }
        }

        /// <summary>
        /// Update the entity
        /// </summary>
        protected override void UpdateEntity()
        {
            transform.Rotate(Vector3.forward, m_RotateSpeed * Time.deltaTime);

            m_Elapsedtime += Time.deltaTime;
            if(m_Elapsedtime > m_TimeAlive)
            {
                m_Elapsedtime = 0.0f;
                utils.Pool.Instance.Destroy(gameObject);
            }
        }
    }
}