using UnityEngine;
using System.Collections;

namespace ui
{
    /// <summary>
    /// Display the current number of player lives
    /// </summary>
    public class Lives : MonoBehaviour
    {
        /// <summary>
        /// The plaer
        /// </summary>
        [SerializeField]
        private entity.Entity m_Player = null;

        /// <summary>
        /// The life images
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image[] m_Lives = null;

        /// <summary>
        /// Update the life images displaying the number of player lives
        /// </summary>
        void Update()
        {
            int lives = m_Player.Lives;
            for(int i = 0; i < m_Lives.Length; ++i)
            {
                bool visible = lives > i;
                m_Lives[i].enabled = visible;
            }
        }
    }
}
