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
        private SpecialWeapon m_PowerUpPrefab = null;

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
                    if (player.Bomb == null)
                    {
                        GameObject powerUp = utils.Pool.Instance.Create(m_PowerUpPrefab.gameObject, player.transform.position, player.transform) as GameObject;

                        player.Bomb = powerUp.GetComponent<SpecialWeapon>();

                        Lives = 0;
                    }
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