using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource _audioSource;

    public AudioClip startScreenMusic;
    public AudioClip endScreenMusic;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayStartScreenMusic(float delay = 0f)
    {
        _audioSource.Stop();
        _audioSource.clip = startScreenMusic;
        Play(delay);
    }

    public void PlayEndScreenMusic(float delay = 0f)
    {
        _audioSource.Stop();
        _audioSource.clip = endScreenMusic;
        Play(delay);
    }

    private void Play(float delay = 0f)
    {
        if (delay > 0f)
        {
            StartCoroutine(AudioEffects.FadeIn(_audioSource, delay));
        }
        else
        {
            _audioSource.Play();
        }
    }

    public void Stop(float delay = 0f)
    {
        if(delay > 0f)
        {
            StartCoroutine(AudioEffects.FadeOut(_audioSource, delay));
        } else
        {
            _audioSource.Stop();
        }
    }

}
