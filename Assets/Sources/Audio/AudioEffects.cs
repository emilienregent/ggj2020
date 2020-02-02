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
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

}