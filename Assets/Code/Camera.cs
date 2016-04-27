using UnityEngine;
using System.Collections;

namespace game
{
    /// <summary>
    /// A basic game camera class
    /// </summary>
    public class Camera : MonoBehaviour
    {
        /// <summary>
        /// The in game background grid
        /// </summary>
        [SerializeField]
        private grid.GridRenderer m_Grid = null;

        /// <summary>
        /// The unity camera component
        /// </summary>
        private Camera m_Camera = null;

        /// <summary>
        /// The distance the camera is from the origin
        /// </summary>
        [SerializeField]
        private float m_CameraOffset = 750.0f;

        /// <summary>
        /// The original, cached position of the camera
        /// </summary>
        private Vector3 m_OriginalPosition = Vector3.zero;

        /// <summary>
        /// The amount of shake to apply to the camera
        /// </summary>
        [SerializeField]
        private float m_MaxCameraShake = 5.0f;

        /// <summary>
        /// Initialise the camera class
        /// </summary>
        void Start()
        {
            m_Camera = GetComponent<Camera>();
            if (m_Camera == null)
            {
                m_Camera = gameObject.AddComponent<Camera>();
            }

            m_OriginalPosition = transform.position;
        }

        /// <summary>
        /// Update the position of the camera
        /// </summary>
        void Update()
        {
            Transform gridTransform = m_Grid.transform;

            Vector3 cameraPos = gridTransform.position;
            cameraPos.z -= m_CameraOffset;
            transform.position = cameraPos;
        }

        /// <summary>
        /// Shake the camera
        /// </summary>
        /// <param name="shakeScale">The amount of shake to apply</param>
        public void Shake(float shakeScale)
        {
            if (shakeScale > 0.0f)
            {
                StopCoroutine(ShakeCamera(shakeScale));
                StartCoroutine(ShakeCamera(shakeScale));
            }
        }

        /// <summary>
        /// Shake camera coroutine
        /// </summary>
        /// <param name="shakeScale">The amount of shake to apply</param>
        private IEnumerator ShakeCamera(float shakeScale)
        {
            float elapsedTime = 0.0f;

            while (true)
            {
                elapsedTime += Time.deltaTime;
                if(elapsedTime > 0.5f)
                {
                    transform.position = m_OriginalPosition;
                    yield break;
                }

                float maxShake = m_MaxCameraShake * shakeScale;

                Vector3 offset = m_OriginalPosition;
                offset.x += Random.Range(-maxShake, maxShake);
                offset.y += Random.Range(-maxShake, maxShake);
                offset.z -= m_CameraOffset;

                transform.position = offset;

                yield return null;
            }
        }
    }
}