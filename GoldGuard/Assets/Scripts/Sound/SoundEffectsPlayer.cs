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
        public AudioClip backgroundGame;
        public AudioClip settingTime;
        public AudioClip timer;
        public AudioClip hp;
        public AudioClip cannon;
        public AudioClip horn;
        public AudioClip lost;
        public AudioClip coins;

        [Header("Sound Sliders")]
        public Slider sfxVolumeSlider;
        public Slider musicVolumeSlider;

        private bool isBackgroundMusicLooping = true;
        private float fadeDuration = 5f;
        private static SoundEffectsPlayer instance;
        private static float sfxVolume = 1f;
        private static float musicVolume = 1f;


        private void Awake()
        {
            // Ensure only one instance of SoundEffectsPlayer exists
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
           // DontDestroyOnLoad(gameObject);
            FindSliderReferences();
        }

        private void FindSliderReferences()
        {
            Slider[] sliders = FindObjectsOfType<Slider>(true);
            foreach (Slider slider in sliders)
            {
                if (slider.gameObject.name == "SFXVolumeSlider")
                {
                    sfxVolumeSlider = slider;
                    sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
                    sfxVolumeSlider.value = sfxVolume;
                }
                else if (slider.gameObject.name == "MusicVolumeSlider")
                {
                    musicVolumeSlider = slider;
                    musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
                    musicVolumeSlider.value = musicVolume;
                }
            }
        }

        private void Start()
        {
            musicSource.clip = background;
            musicSource.Play();

            sfxVolumeSlider.value = sfxVolume;
            musicVolumeSlider.value = musicVolume;

            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        public static SoundEffectsPlayer Instance
        {
            get { return instance; }
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

        private void OnSfxVolumeChanged(float volume)
        {
            sfxSource.volume = volume;
            sfxVolume = volume;
        }

        private void OnMusicVolumeChanged(float volume)
        {
            musicSource.volume = volume;
            musicVolume = volume;
        }
    }
}
