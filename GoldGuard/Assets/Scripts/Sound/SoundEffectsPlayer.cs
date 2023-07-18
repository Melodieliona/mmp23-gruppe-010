using Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UIElements.Slider;

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
        private static float sfxVolume = 1f;
        private static float musicVolume = 1f;
        
        private void Awake()
        {
            // Ensure only one instance of SoundEffectsPlayer exists
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            musicSource.clip = background;
            musicSource.Play();

            GUIManager guiManager = FindObjectOfType<GUIManager>();
            UIDocument uIDocument = guiManager.GetComponent<UIDocument>();

            sfxVolumeSlider = uIDocument.rootVisualElement.Q<Slider>("SFXSlider");
            musicVolumeSlider = uIDocument.rootVisualElement.Q<Slider>("VolumeSlider");

            musicVolumeSlider.RegisterValueChangedCallback(OnMusicVolumeChanged);
            sfxVolumeSlider.RegisterValueChangedCallback(OnSfxVolumeChanged);

            sfxVolumeSlider.value = sfxVolume;
            musicVolumeSlider.value = musicVolume;
        }

        public static SoundEffectsPlayer Instance { get; private set; }

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

        private void OnSfxVolumeChanged(ChangeEvent<float> evt)
        {
            sfxSource.volume = evt.newValue;
        }

        private void OnMusicVolumeChanged(ChangeEvent<float> evt)
        {
            musicSource.volume = evt.newValue;
        }
    }
}
