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
        private Slider musicVolumeSlider;
        private Slider sfxVolumeSlider;

        private bool isBackgroundMusicLooping = true;
        private const float FadeDuration = 5f;

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
            UIDocument uiDocument = null;
            if (guiManager != null )
            {
                uiDocument = guiManager.GetComponent<UIDocument>();
            }
            else
            {
                uiDocument = GameObject.Find("VolumeSlider").GetComponent<UIDocument>();
            }

            musicVolumeSlider = uiDocument.rootVisualElement.Q<Slider>("MusicSlider");
            musicVolumeSlider.RegisterValueChangedCallback(OnMusicVolumeChanged);
            musicVolumeSlider.value = 0.7f;

            sfxVolumeSlider = uiDocument.rootVisualElement.Q<Slider>("SFXSlider");
            sfxVolumeSlider.RegisterValueChangedCallback(OnSfxVolumeChanged);
            sfxVolumeSlider.value = 0.7f;
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
            while (elapsedTime < FadeDuration)
            {
                float fadeFactor = elapsedTime / FadeDuration;
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

        private void OnMusicVolumeChanged(ChangeEvent<float> evt)
        {
            musicSource.volume = evt.newValue;
        }

        private void OnSfxVolumeChanged(ChangeEvent<float> evt)
        {
            sfxSource.volume = evt.newValue;
        }
    }
}