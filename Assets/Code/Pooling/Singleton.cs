using UnityEngine;
using System.Collections;

namespace utils
{
    /// <summary>
    /// A simple singleton implementation used to gain
    /// access to a single instance of a class
    /// </summary>
    /// <typeparam name="T">The type of the singleton</typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance accessors
        /// </summary>
        static public T Instance { get; private set; }

        /// <summary>
        /// Is this singelton instance valid?
        /// </summary>
        static public bool IsValid { get { return (Instance != null); } }

        /// <summary>
        /// Create the singleton isntance
        /// </summary>
        protected virtual void Awake()
        {
            if (!DestroyInstance())
                CreateInstance();
        }

        /// <summary>
        /// When we enable the singleton create it (editor only)
        /// </summary>
        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            CreateInstance();
#endif
        }

        /// <summary>
        /// Destory the singleton instance
        /// </summary>
        protected virtual void OnDestroy()
        {            
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>
        /// Create the singleton instance
        /// </summary>
        private void CreateInstance()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
        }

        /// <summary>
        /// Destroy the singleton instance if it exists
        /// </summary>
        /// <returns></returns>
        private bool DestroyInstance()
        {
            if (Instance != null)
            {
                Object.Destroy(gameObject);
                return true;
            }

            return false;
        }
    }
}
