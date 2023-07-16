using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Sound
{
    public class SoundEffectsPlayer : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Clip")]
        public AudioClip button;
        public AudioClip background;
        public AudioClip settingTime;
        public AudioClip timer;
        public AudioClip hp;
        public AudioClip cannon;
        public AudioClip horn;
        public AudioClip lost;

        [Header("Sound Sliders")]
        public Slider sfxVolumeSlider;
        public Slider musicVolumeSlider;

        private bool isBackgroundMusicLooping = true;
        private float fadeDuration = 5f;

        private void Start()
        {
            musicSource.clip = background;
            musicSource.Play();
            
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        public void PlaySfx(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        public void PlayBackgroundMusic(AudioClip clip)
        {
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.loop = isBackgroundMusicLooping;
            musicSource.Play();
        }

        public void ChangeBackgroundMusic(AudioClip clip)
        {
            StartCoroutine(FadeOutMusic(clip));
        }

        private IEnumerator FadeOutMusic(AudioClip newClip)
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                float fadeFactor = elapsedTime / fadeDuration;
                musicSource.volume = 1f - fadeFactor;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            musicSource.Stop();
            musicSource.clip = newClip;
            musicSource.Play();
        }

        public void ToggleBackgroundMusicLoop()
        {
            isBackgroundMusicLooping = !isBackgroundMusicLooping;
            musicSource.loop = isBackgroundMusicLooping;
        }

        private void OnSfxVolumeChanged(float volume)
        {
            sfxSource.volume = volume;
        }

        private void OnMusicVolumeChanged(float volume)
        {
            musicSource.volume = volume;
        }
    }
}
