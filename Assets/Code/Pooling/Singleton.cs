using UnityEngine;
using System.Collections;

namespace utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static public T Instance { get; private set; }

        static public bool IsValid { get { return (Instance != null); } }

        protected virtual void Awake()
        {
            if (!DestroyInstance())
                CreateInstance();
        }

        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            CreateInstance();
#endif
        }

        protected virtual void OnDestroy()
        {            
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void CreateInstance()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
        }

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
