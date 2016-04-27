using UnityEngine;
using System.Collections;

namespace ui
{
    /// <summary>
    /// Display the players current high score
    /// </summary>
    public class HighScore : MonoBehaviour
    {
        /// <summary>
        /// The text box used to display the high score
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
        /// Update the text box with the current high score
        /// </summary>
        void Update()
        {
            string text = "High Score: " + game.Score.Instance.HighScore.ToString();
            m_TextBox.text = text;
        }
    }
}