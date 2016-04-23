using UnityEngine;
using System.Collections;

namespace entity
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_BulletPrefab;

        [SerializeField]
        private float m_FireRate = 1.0f;

        private float m_ElapsedTime = 0.0f;

        void Start()
        {
            m_ElapsedTime = m_FireRate;
        }

        void Update()
        {
            m_ElapsedTime += Time.deltaTime;
        }

        virtual public void Fire()
        {
            if (m_ElapsedTime >= m_FireRate)
            {
                m_ElapsedTime = 0.0f;

                GameObject bullet = GameObject.Instantiate(m_BulletPrefab) as GameObject;
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
            }
        }
    }
}