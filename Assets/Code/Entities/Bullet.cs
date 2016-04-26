using UnityEngine;
using System.Collections;

namespace entity
{
    /// <summary>
    /// Control the motion and collision of entity released bullets
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        /// <summary>
        /// The damage inflicted by the bullet
        /// </summary>
        [SerializeField]
        private int m_Damage = 10;

        /// <summary>
        /// The velocity of the bullet
        /// </summary>
        [SerializeField]
        private float m_Velocity = 10.0f;

        /// <summary>
        /// The dampening of the bullet
        /// </summary>
        [SerializeField]
        private float m_Dampening = 1.0f;

        /// <summary>
        /// Handle the bullet colliding with another entity
        /// </summary>
        /// <param name="collision">the collision data</param>
        void OnTriggerEnter(Collider collision)
        {
            GameObject collider = collision.gameObject;
            if (tag == "PlayerBullet")
            {
                if (collider.tag == "Enemy")
                {
                    utils.Pool.Instance.Destroy(gameObject);

                    Enemy enemy = collider.GetComponent<Enemy>();
                    enemy.ApplyDamage(m_Damage);
                }
                else if (collider.tag == "Asteroid")
                {
                    utils.Pool.Instance.Destroy(gameObject);

                    Asteroid asteroid = collider.GetComponent<Asteroid>();
                    asteroid.ApplyDamage(m_Damage);
                }
            }
            else if (tag == "EnemyBullet")
            {
                if (collider.tag == "Player")
                {
                    utils.Pool.Instance.Destroy(gameObject);

                    Player player = collider.GetComponent<Player>();
                    player.ApplyDamage(m_Damage);
                }
            }
        }

        /// <summary>
        /// Update the bullet
        /// </summary>
        void Update()
        {
            KillWhenOffScreen();
            CalculatePosition();
        }

        /// <summary>
        /// Calculate the postion of the bullet based on its velocity
        /// </summary>
        private void CalculatePosition()
        {
            float x = -Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
            float y = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);

            Vector3 vel = Vector3.zero;
            vel.x += m_Velocity * Time.deltaTime * x;
            vel.y += m_Velocity * Time.deltaTime * y;

            transform.position += vel;

            m_Velocity *= m_Dampening;
        }

        /// <summary>
        /// Destroy the bullet when it is offscreen
        /// </summary>
        private void KillWhenOffScreen()
        {
            Renderer renderer = GetComponent<Renderer>();
            if(!renderer.isVisible)
            {
                utils.Pool.Instance.Destroy(gameObject);
            }
        }
    }
}