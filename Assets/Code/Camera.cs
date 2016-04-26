using UnityEngine;
using System.Collections;

namespace game
{
    public class Camera : MonoBehaviour
    {
        [SerializeField]
        private grid.GridRenderer m_Grid = null;

        private Camera m_Camera = null;

        [SerializeField]
        private float m_CameraOffset = 750.0f;

        private Vector3 m_OriginalPosition = Vector3.zero;

        // Use this for initialization
        void Start()
        {
            m_Camera = GetComponent<Camera>();
            if (m_Camera == null)
            {
                m_Camera = gameObject.AddComponent<Camera>();
            }

            m_OriginalPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Transform gridTransform = m_Grid.transform;

            Vector3 cameraPos = gridTransform.position;
            cameraPos.z -= m_CameraOffset;
            transform.position = cameraPos;
        }

        public void Shake()
        {
            StopCoroutine(ShakeCamera());
            StartCoroutine(ShakeCamera());
        }

        private IEnumerator ShakeCamera()
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

                Vector3 offset = m_OriginalPosition;
                offset.x += Random.Range(-5.0f, 5.0f);
                offset.y += Random.Range(-5.0f, 5.0f);
                offset.z -= m_CameraOffset;

                transform.position = offset;

                yield return null;
            }
        }
    }
}