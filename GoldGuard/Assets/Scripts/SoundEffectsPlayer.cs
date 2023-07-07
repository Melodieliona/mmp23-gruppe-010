using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    [Header("--------------- Audio Source -----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------------- Audio Clip -----------")]
    public AudioClip button;
    public AudioClip background;
    public AudioClip settingTime;
    public AudioClip timer;
    public AudioClip hp;
    public AudioClip cannon;

    private bool isBackgroundMusicLooping = true;
    private float fadeDuration = 5f;


    private void Start()
    {
        PlayBackgroundMusic(settingTime);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
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

        StartCoroutine(FadeInMusic(newClip));
    }

    private IEnumerator FadeInMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.volume = 0f;
        musicSource.Play();

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float fadeFactor = elapsedTime / fadeDuration;
            musicSource.volume = fadeFactor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = 1f;
    }

    public void ToggleBackgroundMusicLoop()
    {
        isBackgroundMusicLooping = !isBackgroundMusicLooping;
        musicSource.loop = isBackgroundMusicLooping;
    }
}