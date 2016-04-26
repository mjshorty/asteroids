using UnityEngine;
using System.Collections;

namespace entity
{
    /// <summary>
    /// A weapon that fires Bullets
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        /// <summary>
        /// The bullet prefab we want to fire
        /// </summary>
        [SerializeField]
        private GameObject m_BulletPrefab = null;

        /// <summary>
        /// The rate of fire of the weapon
        /// </summary>
        [SerializeField]
        private float m_FireRate = 1.0f;

        /// <summary>
        /// The elapsed time since we last fired the weapon
        /// </summary>
        private float m_ElapsedTime = 0.0f;

        /// <summary>
        /// Fixup the weapon
        /// </summary>
        void Start()
        {
            m_ElapsedTime = m_FireRate;
        }

        /// <summary>
        /// Update the weapon
        /// </summary>
        void Update()
        {
            m_ElapsedTime += Time.deltaTime;
        }

        /// <summary>
        /// Fire the weapon
        /// </summary>
        virtual public void Fire()
        {
            if (m_ElapsedTime >= m_FireRate)
            {
                m_ElapsedTime = 0.0f;

                GameObject bullet = utils.Pool.Instance.Create(m_BulletPrefab, transform.position);
                bullet.transform.rotation = transform.rotation;
            }
        }
    }
}