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

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        changeTileType(TileType.EMPTY);
    }

    public void changeTileType(TileType newType) {
        _type = newType;
        spriteRenderer.sprite = tileSprites[(int)_type];
        switch (_type)
        {
            case TileType.BROKEN:
                GetComponent<PositionModel>().setJob(Jobs.Repair);
                break;
            case TileType.FLOODED:
                GetComponent<PositionModel>().setJob(Jobs.BailOut);
                break;
            case TileType.EMPTY:
                GetComponent<PositionModel>().setJob(Jobs.None);
                break;
        }
    }

    public TileType DoDamage() {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
