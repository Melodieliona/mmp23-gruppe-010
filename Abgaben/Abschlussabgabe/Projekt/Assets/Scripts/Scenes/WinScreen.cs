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
            scoreNumber.text = "SCORE: " + PlayerStats.Score;
            
            soundEffect = SoundEffectsPlayer.Instance;
            soundEffect.PlaySfx(soundEffect.won);
            soundEffect.PlayBackgroundMusic(soundEffect.background);
        }

        public void PlayAgainButton()
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