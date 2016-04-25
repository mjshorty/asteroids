using UnityEngine;
using System.Collections;

namespace ui
{
    public class MainMenu : MonoBehaviour
    {
        public void StartNewGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}