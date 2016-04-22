// Used below website as reference
// http://gamedevelopment.tutsplus.com/tutorials/make-a-neon-vector-shooter-for-ios-the-warping-grid--gamedev-14637

using UnityEngine;

namespace grid
{
    /// <summary>
    /// A spring connecting two points of pass
    /// </summary>
    public class Spring
    {
        private Mass[] m_Connections = new Mass[2];
        private float m_TargetLength;
        private float m_Stiffness;
        private float m_Dampening;

        public Mass ConnectionOne { get { return m_Connections[0]; } }
        public Mass ConnectionTwo { get { return m_Connections[1]; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection1"></param>
        /// <param name="connection2"></param>
        /// <param name="stiffness"></param>
        /// <param name="dampening"></param>
        public Spring(Mass connection1, Mass connection2, float stiffness, float dampening)
        {
            m_Connections[0] = connection1;
            m_Connections[1] = connection2;
            m_Stiffness = stiffness;
            m_Dampening = dampening;

            m_TargetLength = Vector3.Distance(m_Connections[0].Position, m_Connections[1].Position) * 0.95f;
        }

        /// <summary>
        /// Update our spring which will in turn modify the two masses attached
        /// to the spring. We use hooks law to ensure that the two length of the spring
        /// returns back to its original length when stretched.
        /// </summary>
        public void Update()
        {
            Vector3 distanceBeyondLength = m_Connections[0].Position - m_Connections[1].Position;
            float length = Vector3.Magnitude(distanceBeyondLength);

            if (length > m_TargetLength)
            {
                distanceBeyondLength = (distanceBeyondLength / length) * (length - m_TargetLength);
                Vector3 dv = m_Connections[1].Velocity - m_Connections[0].Velocity;
                Vector3 force = m_Stiffness * distanceBeyondLength - dv * m_Dampening;

                m_Connections[0].ApplyForce(-force);
                m_Connections[1].ApplyForce(force);
            }
        }
    }
}
