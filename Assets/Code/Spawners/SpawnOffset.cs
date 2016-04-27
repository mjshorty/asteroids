using UnityEngine;
using System.Collections;

namespace spawn
{
    /// <summary>
    /// Offset a newly spawned object from its parent
    /// </summary>
    public class SpawnOffset : MonoBehaviour
    {
        /// <summary>
        /// The positional offset
        /// </summary>
        [SerializeField]
        private Vector3 m_Offset = Vector3.zero;

        /// <summary>
        /// The rotational offset
        /// </summary>
        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;

        /// <summary>
        /// Apply the offset to the newly spawned entity
        /// </summary>
        /// <param name="xOffsetMultiplier">The number of times to apply the x positional offset</param>
        /// <param name="yOffsetMultiplier">The number of times to apply the y positional offset</param>
        /// <param name="rotationMultiplier">The number of times to apply the rotational offset</param>
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