// Used below website as reference
// http://gamedevelopment.tutsplus.com/tutorials/make-a-neon-vector-shooter-for-ios-the-warping-grid--gamedev-14637

using UnityEngine;

namespace grid
{
    public class Mass
    {
        private Vector3 m_Position;
        private Vector3 m_Velocity;
        private Vector3 m_Acceleration;
        private float m_Dampening;
        private float m_InverseMass; // 1 / mass

        public Vector3 Position { get { return m_Position; } }
        public Vector3 Velocity { get { return m_Velocity; } }

        public Mass()
        {
            m_Position = Vector3.zero;
            m_Velocity = Vector3.zero;
            m_Acceleration = Vector3.zero;
            m_Dampening = 0.98f;
            m_InverseMass = 0.0f;
        }

        public Mass(Vector3 position, float inverseMass)
        {
            m_Position = position;
            m_Velocity = Vector3.zero;
            m_Acceleration = Vector3.zero;
            m_Dampening = 0.98f;
            m_InverseMass = inverseMass;
        }

        public void ApplyForce(Vector3 force)
        {
            m_Acceleration += force * m_InverseMass;
        }

        public void ModifyDampening(float scalar)
        {
            m_Dampening *= scalar;
        }

        public void Update()
        {
            m_Velocity += m_Acceleration;
            m_Position += m_Velocity;
            m_Acceleration = Vector3.zero;

            if (Vector3.SqrMagnitude(m_Velocity) < 0.001f * 0.001f)
            {
                m_Velocity = Vector3.zero;
            }

            m_Velocity *= m_Dampening;
            m_Damping = 0.98f;
        }
    }

}