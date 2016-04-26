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

        void OnTriggerEnter(Collider collision)
        {
            GameObject collider = collision.gameObject;
            if (collider.tag == "Player")
            {
                Player player = collider.GetComponent<Player>();
                if(player != null)
                {
                    GameObject powerUp = utils.Pool.Instance.Create(m_PowerUpPrefab.gameObject) as GameObject;
                    powerUp.transform.parent = player.transform;
                    powerUp.transform.position = player.transform.position;

                    player.Bomb = powerUp.GetComponent<SpecialWeapon>();

                    Lives = 0;
                }
            }
        }

        protected override void UpdateEntity()
        {
            transform.Rotate(Vector3.forward, m_RotateSpeed * Time.deltaTime);
        }
    }
}