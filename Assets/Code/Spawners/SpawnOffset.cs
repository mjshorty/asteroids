using UnityEngine;
using System.Collections;

namespace spawn
{
    public class SpawnOffset : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_Offset = Vector3.zero;

        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;

        public void ApplyOffset(float xOffsetMultiplier, float yOffsetMultiplier, float rotationMultiplier)
        {
            Vector3 offset = m_Offset;
            offset.x *= xOffsetMultiplier;
            offset.y *= yOffsetMultiplier;

            Quaternion rot = Quaternion.identity;
            rot.eulerAngles = m_Rotation * rotationMultiplier;

            transform.position += offset;
            transform.rotation = rot;
        }
    }
}