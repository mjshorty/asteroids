using UnityEngine;
using System.Collections.Generic;

namespace entity
{
    public class Player : Entity
    {
        [SerializeField]
        private float m_RotationSpeed = 10.0f;

        [SerializeField]
        private List<spawn.Spawner> m_Spawners = new List<spawn.Spawner>();

        private SpecialWeapon m_Bomb = null;

        public SpecialWeapon Bomb { get { return m_Bomb; } set { m_Bomb = value; } }

        void OnTriggerEnter(Collider collision)
        {
            GameObject collider = collision.gameObject;
            if(collider.tag == "Asteroid" || collider.tag == "Enemy")
            {
                Lives = Lives - 1;
            }
        }

        void OnDestroy()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

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

        // Update is called once per frame
        override protected void UpdateEntity()
        {
            UpdateInput();
        }

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