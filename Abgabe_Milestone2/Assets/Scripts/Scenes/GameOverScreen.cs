using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes
{
    public class GameOverScreen : MonoBehaviour
    {
        public Text scoreNumber;

        private void Awake()
        {
            scoreNumber.text = "SCORE: " + ScoreController.GetScore().ToString();
            Debug.Log(scoreNumber.text);
        }

        public void RetryButton()
        {
            SceneManager.LoadScene("Game");
            Debug.Log("Retry");
        }

        public void QuitButton()
        {
            Application.Quit();
            Debug.Log("Main");
        }
    }
}