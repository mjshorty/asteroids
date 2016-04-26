using UnityEngine;
using System.Collections.Generic;

namespace entity
{
    public class Player : Entity
    {
        /// <summary>
        /// The rotation speed of the player
        /// </summary>
        [SerializeField]
        private float m_RotationSpeed = 10.0f;

        /// <summary>
        /// A container of entity spawners
        /// </summary>
        [SerializeField]
        private List<spawn.Spawner> m_Spawners = new List<spawn.Spawner>();

        /// <summary>
        /// An optional special weapon which can be acquired in game
        /// </summary>
        private SpecialWeapon m_Bomb = null;
        
        /// <summary>
        /// Get or set the special weapon
        /// </summary>
        public SpecialWeapon Bomb { get { return m_Bomb; } set { m_Bomb = value; } }

        /// <summary>
        /// Handle collisions with enemies and asteroids
        /// </summary>
        /// <param name="collision">The collision data</param>
        void OnTriggerEnter(Collider collision)
        {
            GameObject collider = collision.gameObject;
            if(collider.tag == "Asteroid" || collider.tag == "Enemy")
            {
                Lives = Lives - 1;
            }
        }

        /// <summary>
        /// Load the game over screen when the player has been destroyed
        /// </summary>
        void OnDestroy()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        /// <summary>
        /// Handle the players death, saving the score and terminating all other entities
        /// </summary>
        /// <param name="gameOver">Due to a game over</param>
        /// <returns>Trigger a delayed deletion?</returns>
        override protected bool OnDeath(bool gameOver)
        {
            if (!gameOver)
            {
                game.Score.Instance.SaveScore();

                foreach(var s in m_Spawners)
                {
                    s.KillAll();
                }
            }

            return true;
        }

        /// <summary>
        /// Update the entity
        /// </summary>
        override protected void UpdateEntity()
        {
            UpdateInput();
        }

        /// <summary>
        /// Update the player based on the users input
        /// </summary>
        private void UpdateInput()
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                float rotation = transform.rotation.eulerAngles.z;
                Vector3 acceleration = Vector3.zero;

                acceleration.x -= Mathf.Sin(Mathf.Deg2Rad * rotation);
                acceleration.y += Mathf.Cos(Mathf.Deg2Rad * rotation);

                m_Acceleration += acceleration * 10.0f;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0.0f, 0.0f, -Time.deltaTime * m_RotationSpeed);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0.0f, 0.0f, Time.deltaTime * m_RotationSpeed);
            }

            if(Input.GetKey(KeyCode.Space))
            {
                FireWeapons();
            }

            if(Input.GetKey(KeyCode.Return))
            {
                if(m_Bomb)
                {
                    m_Bomb.Fire();
                    m_Bomb = null;
                }
            }
        }
    }
}