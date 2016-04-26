// Used below website as reference
// http://gamedevelopment.tutsplus.com/tutorials/make-a-neon-vector-shooter-for-ios-the-warping-grid--gamedev-14637

using UnityEngine;

namespace grid
{
    /// <summary>
    /// Represents a point of mass. Each point
    /// has a position along with its current velocity
    /// and acceleration.
    /// </summary>
    public class Mass
    {
        /// <summary>
        /// The position of the point of mass
        /// </summary>
        private Vector3 m_Position;

        /// <summary>
        /// The velocity of the point of mass
        /// </summary>
        private Vector3 m_Velocity;

        /// <summary>
        /// The acceleration of the point of mass
        /// </summary>
        private Vector3 m_Acceleration;

        /// <summary>
        /// The dampening of the point of mass
        /// </summary>
        private float m_Dampening;
        /// <summary>
        /// The ingverse mass (1/mass) of the point of mass
        /// </summary>
        private float m_InverseMass;

        /// <summary>
        /// Get the poisition of the mass
        /// </summary>
        public Vector3 Position { get { return m_Position; } }

        /// <summary>
        /// Get the velocity of the mass
        /// </summary>
        public Vector3 Velocity { get { return m_Velocity; } }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Mass()
        {
            m_Position = Vector3.zero;
            m_Velocity = Vector3.zero;
            m_Acceleration = Vector3.zero;
            m_Dampening = 0.98f;
            m_InverseMass = 0.0f;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">The position of the point of mass</param>
        /// <param name="inverseMass">The inverse mass (1 / mass)</param>
        public Mass(Vector3 position, float inverseMass)
        {
            m_Position = position;
            m_Velocity = Vector3.zero;
            m_Acceleration = Vector3.zero;
            m_Dampening = 0.98f;
            m_InverseMass = inverseMass;
        }

        /// <summary>
        /// Apply a force to the point of mass
        /// </summary>
        /// <param name="force">The force to apply</param>
        public void ApplyForce(Vector3 force)
        {
            m_Acceleration += force * m_InverseMass;
        }

        /// <summary>
        /// Scale the dampening parameter
        /// </summary>
        /// <param name="scalar">The value to scale the dampening value by</param>
        public void ModifyDampening(float scalar)
        {
            m_Dampening *= scalar;
        }

        /// <summary>
        /// Update the point of mass calculating the
        /// new position from the acceleration and velocity
        /// </summary>
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
            m_Dampening = 0.98f;
        }
    }

}