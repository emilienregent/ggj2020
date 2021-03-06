﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    EMPTY,
    BROKEN,
    FLOODED,
}

public class Tile : MonoBehaviour {

    public Sprite[] tileSprites;

    [SerializeField]
    private TileGrid _grid;

    [SerializeField]
    private TileType _type;
    public TileType type { get { return _type; } }

    private SpriteRenderer spriteRenderer;

    private bool _underRepair;

    // SFX
    public List<AudioClip> BrokenTileSFX;
    public List<AudioClip> FloodedTileSFX;

    // AudioSources
    public AudioSource audioSourceSFX;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        changeTileType(TileType.EMPTY);
        _underRepair = false;
    }

    public void changeTileType(TileType newType) {
        _type = newType;
        spriteRenderer.sprite = tileSprites[(int)_type];
        switch (_type)
        {
            case TileType.BROKEN:
                if(BrokenTileSFX.Count > 0)
                {
                    audioSourceSFX.clip = BrokenTileSFX[Random.Range(0, BrokenTileSFX.Count)];
                    audioSourceSFX.Play();
                }
                GetComponent<PositionModel>().setJob(Jobs.Repair);
                break;
            case TileType.FLOODED:
                if (FloodedTileSFX.Count > 0)
                {
                    audioSourceSFX.clip = FloodedTileSFX[Random.Range(0, FloodedTileSFX.Count)];
                    audioSourceSFX.Play();
                }
                GetComponent<PositionModel>().setJob(Jobs.BailOut);
                break;
            case TileType.EMPTY:
                GetComponent<PositionModel>().setJob(Jobs.None);
                break;
        }
    }

    public TileType DoDamage() {
        if (_underRepair) {
            return _type;
        }
        switch(_type)
        {
            case TileType.BROKEN:
                changeTileType(TileType.FLOODED);
                break;
            case TileType.EMPTY :
                changeTileType(TileType.BROKEN);
                break;
        }

        return _type;
    }

    public void doRepair() {
        switch(_type)
        {
            case TileType.BROKEN:
                changeTileType(TileType.EMPTY);
                break;
            case TileType.FLOODED:
                changeTileType(TileType.BROKEN);
                break;
        }
        SetUnderRepair(false);
        // Préviens la grille qu'il faut mettre à jour ses listes de Tiles
        _grid.updateList(this.gameObject);
    }

    public void SetGrid(TileGrid grid) {
        _grid = grid;
    }

    public void SetUnderRepair(bool underRepair)
    {
        _underRepair = underRepair;
    }

    private void OnDrawGizmos() {
        // Draw a yellow sphere at the transform's position
        if(_underRepair == false)
        {
            switch(_type)
            {
                case TileType.EMPTY:
                    Gizmos.color = Color.grey;
                    break;
                case TileType.BROKEN:
                    Gizmos.color = Color.red;
                    break;
                case TileType.FLOODED:
                    Gizmos.color = Color.cyan;
                    break;
            }
        }
        else
        {
            Gizmos.color = Color.magenta;
        }
        
        
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
