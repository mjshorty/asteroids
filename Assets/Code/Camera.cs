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

        // Use this for initialization
        void Start()
        {
            m_Camera = GetComponent<Camera>();
            if (m_Camera == null)
            {
                m_Camera = gameObject.AddComponent<Camera>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            Transform gridTransform = m_Grid.transform;

            Vector3 cameraPos = gridTransform.position;
            cameraPos.z -= m_CameraOffset;
            transform.position = cameraPos;
        }
    }
}