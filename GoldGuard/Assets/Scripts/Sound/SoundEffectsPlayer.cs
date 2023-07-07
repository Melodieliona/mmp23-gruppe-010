using UnityEngine;

namespace Sound
{
    public class SoundEffectsPlayer : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Clip")]
        public AudioClip button;
        public AudioClip mainBackground;

        private void Start()
        {
            musicSource.clip = mainBackground;
            musicSource.Play();
        }

        public void PlaySfx(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}