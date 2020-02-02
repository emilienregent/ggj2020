using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{

    public List<AudioClip> RepairSFX;
    public AudioClip TurnLeftSFX;
    public AudioClip TurnRightSFX;
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(Jobs currentJob)
    {
        switch(currentJob)
        {
            case Jobs.Fish1:
                break;

            case Jobs.Fish2:
                break;

            case Jobs.Repair:
                if (RepairSFX.Count > 0)
                {
                    audioSource.loop = false;
                    audioSource.clip = RepairSFX[Random.Range(0, RepairSFX.Count)];
                    audioSource.Play();
                }
                break;

            case Jobs.BailOut:
                break;

            default:
                break;
        }
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void PlayTurnDirection(float value)
    {
        if(value == 0f)
        {
            return;
        }

        AudioClip newClip;

        if (value > 0f)
        {
            newClip = TurnLeftSFX;
        } else
        {
            newClip = TurnRightSFX;
        }

        if(newClip != audioSource.clip)
        {
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();
        }

    }
}
