using UnityEngine;
using System.Collections;

namespace ui
{
    /// <summary>
    /// Display the players last score
    /// </summary>
    public class LastScore : MonoBehaviour
    {
        /// <summary>
        /// The text box used to display the last score
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
        /// Update the text box with the last score
        /// </summary>
        void Update()
        {
            string text = "Score: " + game.Score.Instance.LastScore.ToString();
            m_TextBox.text = text;
        }
    }
}