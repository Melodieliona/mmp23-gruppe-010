using Player;
using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes
{
    public class WinScreen : MonoBehaviour
    {
        public Text scoreNumber;
        private SoundEffectsPlayer soundEffect;

        private void Awake()
        {
            scoreNumber.text = "SCORE: " + ScoreController.GetScore();
            soundEffect = SoundEffectsPlayer.Instance;
            soundEffect.PlaySfx(soundEffect.lost); // Change to different sound
            soundEffect.PlayBackgroundMusic(soundEffect.background);

            Debug.Log(scoreNumber.text);
        }

        public void PlayAgainButton()
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