using Player;
using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes
{
    public class GameOverScreen : MonoBehaviour
    {
        public Text scoreNumber;
        private SoundEffectsPlayer soundEffect;

        private void Awake()
        {
            scoreNumber.text = "SCORE: " + ScoreController.GetScore().ToString();
            soundEffect = SoundEffectsPlayer.Instance;
            soundEffect.PlaySfx(soundEffect.lost);
            soundEffect.PlayBackgroundMusic(soundEffect.background);

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