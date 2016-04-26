// Used below website as reference
// http://gamedevelopment.tutsplus.com/tutorials/make-a-neon-vector-shooter-for-ios-the-warping-grid--gamedev-14637

using UnityEngine;
using System.Collections.Generic;

namespace grid
{
    /// <summary>
    /// Generate a grid of points of mass interconnected with springs
    /// </summary>
    public class Grid : utils.Singleton<Grid>
    {
        /// <summary>
        /// The various forces we can apply to the grid
        /// </summary>
        public enum Force
        {
            Explosion,
            Implosion,
            Directed
        }

        /// <summary>
        /// The width of the grid
        /// </summary>
        [SerializeField]
        private int m_Width = 400;

        /// <summary>
        /// The height of the grid
        /// </summary>
        [SerializeField]
        private int m_Height = 300;

        /// <summary>
        /// The dynamic springs in the grid
        /// </summary>
        private List<Spring> m_Springs = null;

        /// <summary>
        /// The anchor springs in the grid
        /// </summary>
        private List<Spring> m_AnchorSprings = null;

        /// <summary>
        /// The dynamic points of mass in the grid
        /// </summary>
        private List<Mass> m_DynamicPoints = null;

        /// <summary>
        /// The anchor points of mass in the grid
        /// </summary>
        private List<Mass> m_AnchorPoints = null;

        /// <summary>
        /// Get the dynamic points of mass
        /// </summary>
        public List<Mass> DynamicPoints { get { return m_DynamicPoints; } }

        /// <summary>
        /// Get the dynamic springs
        /// </summary>
        public List<Spring> Springs { get { return m_Springs; } }

        /// <summary>
        /// The spacing between points of mass
        /// </summary>
        [SerializeField]
        private float m_Spacing = 100.0f;

        /// <summary>
        /// The default mass of a single point
        /// </summary>
        [SerializeField]
        private float m_Mass = 1.0f;

        /// <summary>
        /// Create our grid
        /// </summary>
        public void Start()
        {
            Resolution res = Screen.currentResolution;

            int numRows = (int)((float)m_Height / m_Spacing) + 1;
            int numColumns = (int)((float)m_Width / m_Spacing) + 1;

            m_DynamicPoints = new List<Mass>();
            m_AnchorPoints = new List<Mass>();

            m_Springs = new List<Spring>();
            m_AnchorSprings = new List<Spring>();

            Vector3 offset = Vector3.zero;
            offset.x = (numColumns * m_Spacing) * 0.5f;
            offset.y = (numRows * m_Spacing) * 0.5f;

            for (int y = 0; y < numRows; ++y)
            {
                for (int x = 0; x < numColumns; ++x)
                {
                    Vector3 position = new Vector3(x * m_Spacing, y * m_Spacing, 0.0f);

                    Mass dynamicPoint = new Mass(position - offset, 1.0f / m_Mass);
                    m_DynamicPoints.Add(dynamicPoint);

                    // anchor all of the outside points of mass
                    if (x == 0 || y == 0 || x == numColumns - 1 || y == numRows - 1)
                    {
                        Mass anchorPoint = new Mass(position - offset, 0.0f);
                        m_AnchorPoints.Add(anchorPoint);

                        Spring spring = new Spring(anchorPoint, dynamicPoint, 0.1f, 0.1f);
                        m_AnchorSprings.Add(spring);
                    }
                    // anchor every 3rd dynamic point
                    else if (x % 3 == 0 && y % 3 == 0)
                    {
                        Mass anchorPoint = new Mass(position - offset, 0.0f);
                        m_AnchorPoints.Add(anchorPoint);

                        Spring spring = new Spring(anchorPoint, dynamicPoint, 0.002f, 0.02f);
                        m_AnchorSprings.Add(spring);
                    }

                    // add a spring between each point of mass and the previous
                    if (x > 0)
                    {
                        Spring spring = new Spring(GetPointOfMass(x - 1, y, numColumns), dynamicPoint, 0.28f, 0.06f);
                        m_Springs.Add(spring);
                    }

                    if (y > 0)
                    {
                        Spring spring = new Spring(GetPointOfMass(x, y - 1, numColumns), dynamicPoint, 0.28f, 0.06f);
                        m_Springs.Add(spring);
                    }
                }
            }
        }

        /// <summary>
        /// Get a point of mass from the grid
        /// </summary>
        /// <param name="dynamic">Is the point of mass dynamic or anchored?</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="numColumns">The number of columns in the grid</param>
        /// <returns></returns>
        private Mass GetPointOfMass(int x, int y, int numColumns)
        {
            int index = y * numColumns + x;
            return m_DynamicPoints[index];
        }

        /// <summary>
        /// Update the grid
        /// </summary>
        public void Update()
        {
            int numSprings = m_Springs.Count;
            for(int i = 0; i < numSprings; ++i)
            {
                Spring spring = m_Springs[i] as Spring;
                spring.Update();
            }

            numSprings = m_AnchorSprings.Count;
            for (int i = 0; i < numSprings; ++i)
            {
                Spring spring = m_AnchorSprings[i] as Spring;
                spring.Update();
            }

            int numMasses = m_DynamicPoints.Count;
            for (int i = 0; i < numMasses; ++i)
            {
                Mass mass = m_DynamicPoints[i] as Mass;
                mass.Update();
            }
        }

        /// <summary>
        /// Apply a directed force to the grid
        /// </summary>
        /// <param name="force">The force to apply</param>
        /// <param name="position">The origin of the force</param>
        /// <param name="radius">The radius of the force</param>
        public void ApplyDirectedForce(Vector3 force, Vector3 position, float radius)
        {
            float squareRadius = radius * radius;

            int numMasses = m_DynamicPoints.Count;
            for (int i = 0; i < numMasses; ++i)
            {
                Vector3 posToMass = position - m_DynamicPoints[i].Position;
                if (posToMass.sqrMagnitude < squareRadius)
                {
                    m_DynamicPoints[i].ApplyForce(10.0f * force / (10.0f + posToMass.magnitude));
                }
            }
        }

        /// <summary>
        /// Apply an implosive force to the grid
        /// </summary>
        /// <param name="force">The force to apply</param>
        /// <param name="position">The origin of the force</param>
        /// <param name="radius">The radius of the force</param>
        public void ApplyImplosiveForce(float force, Vector3 position, float radius)
        {
            float squareRadius = radius * radius;

            int numMasses = m_DynamicPoints.Count;
            for (int i = 0; i < numMasses; ++i)
            {
                Vector3 posToMass = position - m_DynamicPoints[i].Position;
                if (posToMass.sqrMagnitude < squareRadius)
                {
                    m_DynamicPoints[i].ApplyForce(10.0f * force * posToMass / (100 + posToMass.sqrMagnitude));
                    m_DynamicPoints[i].ModifyDampening(0.6f);
                }
            }
        }

        /// <summary>
        /// Apply an explosive force to the groid
        /// </summary>
        /// <param name="force">The force to apply</param>
        /// <param name="position">The origin of the force</param>
        /// <param name="radius">The radius of the force</param>
        public void ApplyExplosiveForce(float force, Vector3 position, float radius)
        {
            float squareRadius = radius * radius;

            int numMasses = m_DynamicPoints.Count;
            for (int i = 0; i < numMasses; ++i)
            {
                Vector3 posToMass = position - m_DynamicPoints[i].Position;
                if (posToMass.sqrMagnitude < squareRadius)
                {
                    m_DynamicPoints[i].ApplyForce(100 * force * posToMass / (10000 + posToMass.sqrMagnitude));
                    m_DynamicPoints[i].ModifyDampening(0.6f);
                }
            }
        }
    }
}