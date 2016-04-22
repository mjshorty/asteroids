using UnityEngine;
using System.Collections;

namespace grid
{
    public class Generator : MonoBehaviour
    {
        private ArrayList m_Springs;
        private ArrayList m_Masses;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            int numSprings = m_Springs.Count;
            for(int i = 0; i < numSprings; ++i)
            {
                Spring spring = m_Springs[i] as Spring;
                spring.Update();
            }

            int numMasses = m_Masses.Count;
            for (int i = 0; i < numMasses; ++i)
            {
                Mass mass = m_Masses[i] as Mass;
                mass.Update();
            }
        }
    }
}