using UnityEngine;
using System.Collections;

namespace ui
{
    public class Lives : MonoBehaviour
    {
        [SerializeField]
        private entity.Entity m_Player = null;

        [SerializeField]
        private UnityEngine.UI.Image[] m_Lives = null;

        // Update is called once per frame
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
