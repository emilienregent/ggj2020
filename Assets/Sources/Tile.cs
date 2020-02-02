using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    EMPTY,
    BROKEN,
    FLOODED,
}

public class Tile : MonoBehaviour {

    [SerializeField]
    Sprite[] tileSprites;

    [SerializeField]
    private TileGrid _grid;

    [SerializeField]
    private TileType _type;
    public TileType type { get { return _type; } }

    private SpriteRenderer spriteRenderer;

    private bool _underRepair;

    public List<AudioClip> BrokenTileSFX;
    public List<AudioClip> FloodedTileSFX;
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
                    audioSource.clip = BrokenTileSFX[Random.Range(0, BrokenTileSFX.Count)];
                    audioSource.Play();
                }
                GetComponent<PositionModel>().setJob(Jobs.Repair);
                break;
            case TileType.FLOODED:
                if (FloodedTileSFX.Count > 0)
                {
                    audioSource.clip = FloodedTileSFX[Random.Range(0, FloodedTileSFX.Count)];
                    audioSource.Play();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
