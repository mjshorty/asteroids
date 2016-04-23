using UnityEngine;
using System.Collections;

namespace entity
{
    public class Asteroid : Entity
    {
        [SerializeField]
        private Vector3 m_ConstantVelocity = Vector3.one;

        public Vector3 ConstantVelocity { get { return m_ConstantVelocity; } set { m_ConstantVelocity = value; } }

        override protected void UpdateEntity()
        {
            m_Velocity = m_ConstantVelocity;
        }
    }
}