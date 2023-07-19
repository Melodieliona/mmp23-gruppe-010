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
            scoreNumber.text = "SCORE: " + PlayerStats.Score;
            soundEffect = SoundEffectsPlayer.Instance;
            soundEffect.PlaySfx(soundEffect.lost);
            soundEffect.PlayBackgroundMusic(soundEffect.background);
        }

        public void RetryButton()
        {
            soundEffect.PlaySfx(soundEffect.button);
            SceneManager.LoadScene("Game");
        }

        public void QuitButton()
        {
            soundEffect.PlaySfx(soundEffect.button);
            Application.Quit();
        }
    }
}