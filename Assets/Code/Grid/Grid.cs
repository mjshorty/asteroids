using UnityEngine;
using System.Collections.Generic;

namespace grid
{
    /// <summary>
    /// Generate a grid of points of mass interconnected with springs
    /// </summary>
    public class Grid : MonoBehaviour
    {
        private List<Spring> m_Springs = null;
        private List<Mass> m_DynamicPoints = null;
        private List<Mass> m_AnchorPoints = null;

        [SerializeField]
        private float m_Spacing;

        [SerializeField]
        private float m_Mass = 1.0f;

        /// <summary>
        /// Create our grid
        /// </summary>
        public void Start()
        {
            Resolution res = Screen.currentResolution;

            int numColumns = (int)((float)res.height / m_Spacing) + 1;
            int numRows = (int)((float)res.width / m_Spacing) + 1;

            CreatePointsOfMass(numColumns, numRows);
            CreateSprings(numColumns, numRows);
        }

        /// <summary>
        /// Create and initialise all of the points of mass
        /// within our grid. The anchor points are immovable and will
        /// be used to anchor our dynamic points to their original position
        /// otherwise we'd end up with a (very) simple cloth sim that would
        /// never return to the original state.
        /// </summary>
        /// <param name="numColumns">The number of columns in the grid</param>
        /// <param name="numRows">The number of rows in the grid</param>
        private void CreatePointsOfMass(int numColumns, int numRows)
        {
            m_DynamicPoints = new List<Mass>();
            m_AnchorPoints = new List<Mass>();

            for (int y = 0; y < numRows; ++y)
            {
                for (int x = 0; x < numColumns; ++x)
                {
                    Vector3 position = new Vector3(x * m_Spacing, y * m_Spacing, 0.0f);

                    m_DynamicPoints.Add(new Mass(position, 1.0f / m_Mass));

                    // its a bit wastefull creating all these anchor points when we only need the outside ones
                    // and then every 3x3, maybe we can be a bit smarter about this.
                    m_AnchorPoints.Add(new Mass(position, 0.0f));
                }
            }
        }

        /// <summary>
        /// Create the springs between our points of mass
        /// </summary>
        /// <param name="numColumns">The number of columns in the grid</param>
        /// <param name="numRows">The number of rows in the grid</param>
        private void CreateSprings(int numColumns, int numRows)
        {
            m_Springs = new List<Spring>();

            for (int y = 0; y < numRows; ++y)
            {
                for (int x = 0; x < numColumns; ++x)
                {
                    // anchor all of the outside points of mass
                    if (x == 0 || y == 0 || x == numColumns - 1 || y == numRows - 1)
                    {
                        Spring spring = new Spring(GetPointOfMass(false, x, y, numColumns), GetPointOfMass(true, x, y, numColumns), 0.1f, 0.1f);
                        m_Springs.Add(spring);
                    }
                    // anchor every 3rd dynamic point
                    else if (x % 3 == 0 && y % 3 == 0)
                    {
                        Spring spring = new Spring(GetPointOfMass(false, x, y, numColumns), GetPointOfMass(true, x, y, numColumns), 0.002f, 0.02f);
                        m_Springs.Add(spring);
                    }


                    // add a spring between each point of mass and the previous
                    if (x > 0)
                    {
                        Spring spring = new Spring(GetPointOfMass(true, x - 1, y, numColumns), GetPointOfMass(true, x, y, numColumns), 0.28f, 0.06f);
                        m_Springs.Add(spring);
                    }

                    if (y > 0)
                    {
                        Spring spring = new Spring(GetPointOfMass(false, x, y - 1, numColumns), GetPointOfMass(true, x, y, numColumns), 0.28f, 0.06f);
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
        private Mass GetPointOfMass(bool dynamic, int x, int y, int numColumns)
        {
            int index = y * numColumns + x;
            return dynamic ? m_DynamicPoints[index] : m_AnchorPoints[index];
        }

        // Update is called once per frame
        public void Update()
        {
            int numSprings = m_Springs.Count;
            for(int i = 0; i < numSprings; ++i)
            {
                Spring spring = m_Springs[i] as Spring;
                spring.Update();
            }

            int numMasses = m_DynamicPoints.Count;
            for (int i = 0; i < numMasses; ++i)
            {
                Mass mass = m_DynamicPoints[i] as Mass;
                mass.Update();
            }
        }
    }
}