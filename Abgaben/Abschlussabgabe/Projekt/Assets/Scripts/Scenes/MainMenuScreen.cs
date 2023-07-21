using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class MainMenuScreen : MonoBehaviour
    {
        private SoundEffectsPlayer soundEffect;

        private void Awake()
        {
            soundEffect = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundEffectsPlayer>();
        }

        public void PlayButton()
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