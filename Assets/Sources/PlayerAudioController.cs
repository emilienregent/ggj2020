using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{

    public List<AudioClip> RepairSFX;
    //public AudioClip TurnLeftSFX;
    //public AudioClip TurnRightSFX;
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

            case Jobs.Direction:
                break;

            case Jobs.Repair:
                if (RepairSFX.Count > 0)
                {
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
}
