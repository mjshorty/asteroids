using UnityEngine;
using System.Collections;

namespace ui
{
    /// <summary>
    /// Main menu UI manager
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// Start a new game
        /// </summary>
        public void StartNewGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        /// <summary>
        /// Exit the game
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}