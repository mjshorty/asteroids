using UnityEngine;
using System.Collections;

namespace game
{
    public class Score : utils.Singleton<Score>
    {
        private int m_CurrentScore = 0;
        private int m_HighScore = 0;

        public int CurrentScore { get { return m_CurrentScore; } }
        public int HighScore { get { return m_HighScore; } }
        public int LastScore { get { return PlayerPrefs.GetInt("Last Score"); } }

        void Start()
        {
            m_HighScore = PlayerPrefs.GetInt("High Score");
        }

        public void Award(int points)
        {
            m_CurrentScore += points;
        }

        public void SaveScore()
        {
            if (m_CurrentScore > m_HighScore)
            {
                PlayerPrefs.SetInt("High Score", m_CurrentScore);
            }
                
            PlayerPrefs.SetInt("Last Score", m_CurrentScore);

            m_CurrentScore = 0;
            m_HighScore = PlayerPrefs.GetInt("High Score");
        }
    }
}