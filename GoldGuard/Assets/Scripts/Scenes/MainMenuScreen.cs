using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class MainMenuScreen : MonoBehaviour
    {
        SoundEffectsPlayer soundEffect;

        private void Awake()
        {
            soundEffect = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundEffectsPlayer>();
        }

        public void PlayButton()
        {
            DontDestroyOnLoad(soundEffect);
            soundEffect.PlaySFX(soundEffect.button);
            SceneManager.LoadScene("Game");
        }

        public void QuitButton()
        {
            Application.Quit();
            soundEffect.PlaySFX(soundEffect.button);
            Debug.Log("Quit Application");
        }
    }
}