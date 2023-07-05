using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class MainMenuScreen : MonoBehaviour
    {
        public void PlayButton()
        {
            SceneManager.LoadScene("Game");
        }

        public void QuitButton()
        {
            Application.Quit();
            Debug.Log("Quit Application");
        }
    }
}