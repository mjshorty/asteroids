using UnityEngine;
using System.Collections.Generic;

namespace grid
{
    public class GridRenderer : MonoBehaviour
    {
        private LineRenderer m_Renderer = null;
        private Grid m_Grid = null;

        [SerializeField]
        private float m_LineWidth = 2.0f;

        private List<Vector3> m_Vertices = new List<Vector3>();

        [SerializeField]
        private Color m_LineColour = Color.blue;

        [SerializeField]
        private Material m_Material = null;

        /// <summary>
        /// Initialise the mesh renderer
        /// </summary>
        void Start()
        {
            // Get or create our renderer
            m_Renderer = GetComponent<LineRenderer>();
            if (m_Renderer == null)
            {
                m_Renderer = gameObject.AddComponent<LineRenderer>();
            }

            m_Material.renderQueue = 0;

            m_Renderer.material = m_Material;
            m_Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            m_Renderer.receiveShadows = false;

            m_Grid = GetComponent<Grid>();
            if(m_Grid == null)
            {
                Debug.Assert(m_Grid != null, "The grid can not be null, disabling grid renderer.");
                enabled = false;
            }
        }

        /// <summary>
        /// Update the mesh renderer
        /// </summary>
        void Update()
        {
            UpdateVertices();

            m_Renderer.SetWidth(m_LineWidth, m_LineWidth);
            m_Renderer.SetVertexCount(m_Vertices.Count);
            m_Renderer.SetPositions(m_Vertices.ToArray());

            if(m_Material)
            {
                m_Material.color = m_LineColour;
            }
            else
            {
                m_Renderer.SetColors(m_LineColour, m_LineColour);
            }
        }

        /// <summary>
        /// Update the vertices for the line renderer
        /// </summary>
        private void UpdateVertices()
        {
            if(m_Grid == null)
            {
                return;
            }

            m_Vertices.Clear();

            List<Spring> springs = m_Grid.Springs;
            int count = springs.Count;

            for(int i = 0; i < count; ++i)
            {
                m_Vertices.Add(springs[i].ConnectionOne.Position);
                m_Vertices.Add(springs[i].ConnectionTwo.Position);
            }
        }
    }
}