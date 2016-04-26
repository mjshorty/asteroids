using UnityEngine;
using System.Collections;

namespace entity
{
    public class PowerUp : Entity
    {
        [SerializeField]
        private SpecialWeapon m_PowerUpPrefab = null;

        [SerializeField]
        private float m_RotateSpeed = 1.0f;

        [SerializeField]
        private float m_TimeAlive = 5.0f;

        private float m_Elapsedtime = 0.0f;

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