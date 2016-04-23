using UnityEngine;
using System.Collections;

namespace entity
{
    public class Entity : MonoBehaviour
    {
        [SerializeField]
        private float m_Dampening = 0.95f;

        protected Vector3 m_Acceleration = Vector3.zero;
        protected Vector3 m_Velocity = Vector3.zero;

        virtual protected void UpdateEntity()
        {
        }

        void Update()
        {
            UpdateEntity();

            m_Velocity += m_Acceleration * Time.deltaTime;
            transform.position += m_Velocity * Time.deltaTime;

            m_Acceleration *= m_Dampening;
            m_Velocity *= m_Dampening;

            if (Vector3.SqrMagnitude(m_Velocity) < 0.001f * 0.001f)
            {
                m_Velocity = Vector3.zero;
            }

            if (Vector3.SqrMagnitude(m_Acceleration) < 0.001f * 0.001f)
            {
                m_Acceleration = Vector3.zero;
            }
        }
    }
}