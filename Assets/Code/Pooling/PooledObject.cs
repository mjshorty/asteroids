using UnityEngine;
using System.Collections;

namespace utils
{
    /// <summary>
    /// A pooled game object
    /// </summary>
    public class PooledObject : MonoBehaviour
    {
        /// <summary>
        /// The ID of the pool we are assigned to
        /// </summary>
        public int PoolID { get; set; }
    }
}