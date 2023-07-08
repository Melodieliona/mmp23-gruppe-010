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
            DontDestroyOnLoad(soundEffect);
            soundEffect.PlaySfx(soundEffect.button);
            SceneManager.LoadScene("Game");
        }

        public void QuitButton()
        {
            Application.Quit();
            soundEffect.PlaySfx(soundEffect.button);
            Debug.Log("Quit Application");
        }
    }
}