using UnityEngine;
using System.Collections;

namespace ui
{
    public class Score : MonoBehaviour
    {
        private UnityEngine.UI.Text m_TextBox = null;

        // Use this for initialization
        void Start()
        {
            m_TextBox = GetComponent<UnityEngine.UI.Text>();
        }

        // Update is called once per frame
        void Update()
        {
            string text = "Score: " + entity.Player.Score.ToString();
            m_TextBox.text = text;
        }
    }
}