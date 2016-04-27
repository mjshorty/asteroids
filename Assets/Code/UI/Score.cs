using UnityEngine;
using System.Collections;

namespace ui
{
    /// <summary>
    /// Show the players current score
    /// </summary>
    public class Score : MonoBehaviour
    {
        /// <summary>
        /// The text box used to display the current score
        /// </summary>
        private UnityEngine.UI.Text m_TextBox = null;

        /// <summary>
        /// Fixup the class
        /// </summary>
        void Start()
        {
            m_TextBox = GetComponent<UnityEngine.UI.Text>();
        }

        /// <summary>
        /// Update the current score text box
        /// </summary>
        void Update()
        {
            string text = "Score: " + game.Score.Instance.CurrentScore.ToString();
            m_TextBox.text = text;
        }
    }
}