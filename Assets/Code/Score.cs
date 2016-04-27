using UnityEngine;
using System.Collections;

namespace game
{
    /// <summary>
    /// The player score class used to record and save 
    /// the current player score and overall high score
    /// </summary>
    public class Score : utils.Singleton<Score>
    {
        /// <summary>
        /// The current score
        /// </summary>
        private int m_CurrentScore = 0;

        /// <summary>
        /// The players high score
        /// </summary>
        private int m_HighScore = 0;

        /// <summary>
        /// Get the current score
        /// </summary>
        public int CurrentScore { get { return m_CurrentScore; } }

        /// <summary>
        /// Get the high score
        /// </summary>
        public int HighScore { get { return m_HighScore; } }

        /// <summary>
        /// Get the players last score
        /// </summary>
        public int LastScore { get { return PlayerPrefs.GetInt("Last Score"); } }

        /// <summary>
        /// Initialize the class
        /// </summary>
        void Start()
        {
            m_HighScore = PlayerPrefs.GetInt("High Score");
            m_CurrentScore = 0;
        }

        /// <summary>
        /// Award the player points
        /// </summary>
        /// <param name="points">the number of points to give to the player</param>
        public void Award(int points)
        {
            m_CurrentScore += points;
        }

        /// <summary>
        /// Save the score
        /// </summary>
        public void SaveScore()
        {
            if (m_CurrentScore > m_HighScore)
            {
                PlayerPrefs.SetInt("High Score", m_CurrentScore);
            }
                
            PlayerPrefs.SetInt("Last Score", m_CurrentScore);
        }
    }
}