using UnityEngine;
using System.Collections;

public static class AudioEffects
{

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        audioSource.volume = 0;

        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += 0.1f * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0.1f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeOutToVolume(AudioSource audioSource, float fadeTime, float targetVolume)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > targetVolume)
        {
            audioSource.volume -= 0.1f * Time.deltaTime / fadeTime;

            yield return null;
        }

    }

}