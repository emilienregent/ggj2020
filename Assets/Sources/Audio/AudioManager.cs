using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource audioSource;

    public AudioClip startScreenMusic;
    public AudioClip endScreenMusic;

    public void PlayStartScreenMusic(float delay = 0f)
    {
        audioSource.Stop();
        audioSource.clip = startScreenMusic;
        Play(delay);
    }

    public void PlayEndScreenMusic(float delay = 0f)
    {
        audioSource.Stop();
        audioSource.clip = endScreenMusic;
        audioSource.volume = 1f;
        Play(delay);
    }

    private void Play(float delay = 0f)
    {
        if (delay > 0f)
        {
            StartCoroutine(AudioEffects.FadeIn(audioSource, delay));
        }
        else
        {
            audioSource.Play();
        }
    }

    public void Stop(float delay = 0f)
    {
        if(delay > 0f)
        {
            StartCoroutine(AudioEffects.FadeOut(audioSource, delay));
        } else
        {
            audioSource.Stop();
        }
    }

    public void FadeOutStartScreenMusic()
    {
        StartCoroutine(AudioEffects.FadeOutToVolume(audioSource, 0.60f, 0.15f));
    }

}
