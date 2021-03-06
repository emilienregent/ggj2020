﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{

    private TileGrid _grid;

    // SFX
    public List<AudioClip> RepairSFX;
    public AudioClip TurnLeftSFX;
    public AudioClip TurnRightSFX;
    public AudioClip BailOutSFX;
    public AudioClip PressButtonSFX;

    // VOICES
    // TILES
    public List<AudioClip> BrokenTileVoice;
    private List<AudioClip> availableBrokenTileVoice;
    public List<AudioClip> FloodedTileVoice;
    private List<AudioClip> availableFloodedTileVoice;
    //ITEMS
    public List<AudioClip> RockItemVoice;
    private List<AudioClip> availableRockItemVoice;
    public List<AudioClip> PlankItemVoice;
    private List<AudioClip> availablePlankItemVoice;

    // AudioSources
    public AudioSource audioSourceSFX;
    public AudioSource audioSourceVoice;

    private void Awake()
    {
        _grid = GameManager.instance.ship.GetComponentInChildren<TileGrid>();
    }

    private void Start()
    {
        availableBrokenTileVoice = BrokenTileVoice;
        availableFloodedTileVoice = FloodedTileVoice;
        availableRockItemVoice = RockItemVoice;
        availablePlankItemVoice = PlankItemVoice;
    }

    private void Update()
    {
        if(audioSourceVoice.isPlaying == false && GameManager.instance.currentGameState == GameManager.enumGameState.Game)
        {

            AudioClip clipToPlay = null;

            // On a de la dispo pour parler, voyons ce qu'on peut dire
            if(_grid.CountFloodedTiles > 0)
            {
                if (FloodedTileVoice.Count > 0 && Random.Range(1, 3) == 1) // 33% de chance d'avoir un voice fx
                {
                    clipToPlay = availableFloodedTileVoice[Random.Range(0, availableFloodedTileVoice.Count)];
                    availableFloodedTileVoice.Remove(clipToPlay);
                    if (availableFloodedTileVoice.Count == 0)
                    {
                        availableFloodedTileVoice = FloodedTileVoice;
                    }
                }
            }

            // Toujours rien à dire ?
            if(clipToPlay == null && _grid.CountBrokenTiles > 0)
            {
                if (BrokenTileVoice.Count > 0 && Random.Range(1, 3) == 1) // 33% de chance d'avoir un voice fx
                {
                    clipToPlay = availableBrokenTileVoice[Random.Range(0, availableBrokenTileVoice.Count)];
                    availableBrokenTileVoice.Remove(clipToPlay);
                    if (availableBrokenTileVoice.Count == 0)
                    {
                        availableBrokenTileVoice = BrokenTileVoice;
                    }
                }
            }

            if (clipToPlay == null && GameManager.instance.itemMalusManager.inGameItemsCount > 0)
            {
                if (RockItemVoice.Count > 0 && Random.Range(1, 5) == 1) // 20% de chance d'avoir un voice fx
                {
                    clipToPlay = availableRockItemVoice[Random.Range(0, availableRockItemVoice.Count)];
                    availableRockItemVoice.Remove(clipToPlay);
                    if (availableRockItemVoice.Count == 0)
                    {
                        availableRockItemVoice = RockItemVoice;
                    }
                }
            }

            if (clipToPlay == null && GameManager.instance.itemBonusManager.inGameItemsCount > 0)
            {
                if (PlankItemVoice.Count > 0 && Random.Range(1, 10) == 1) // 10% de chance d'avoir un voice fx
                {
                    clipToPlay = availablePlankItemVoice[Random.Range(0, availablePlankItemVoice.Count)];
                    availablePlankItemVoice.Remove(clipToPlay);
                    if (availablePlankItemVoice.Count == 0)
                    {
                        availablePlankItemVoice = PlankItemVoice;
                    }
                }
            }

            if (clipToPlay != null)
            {
                audioSourceVoice.clip = clipToPlay;
                audioSourceVoice.Play();
            }
        }
    }

    public void PlayInputSFX()
    {
        audioSourceSFX.clip = PressButtonSFX;
        audioSourceSFX.Play();
    }

    public void PlayJobSFX(Jobs currentJob)
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
                    audioSourceSFX.clip = RepairSFX[Random.Range(0, RepairSFX.Count)];
                    audioSourceSFX.Play();
                }
                break;

            case Jobs.BailOut:
                audioSourceSFX.clip = BailOutSFX;
                audioSourceSFX.Play();
                break;

            default:
                break;
        }
    }

    public void StopSFX()
    {
        audioSourceSFX.Stop();
    }

    public void StopVoice()
    {
        audioSourceVoice.Stop();
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

        if(newClip != audioSourceSFX.clip)
        {
            audioSourceSFX.Stop();
            audioSourceSFX.clip = newClip;
            audioSourceSFX.Play();
        }

    }
}
