using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    EMPTY,
    BROKEN,
    FLOODED,
}

public class Tile : MonoBehaviour
{

    [SerializeField]
    Sprite[] tileSprites;

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

    }

    public void DoDamage() {
        switch(_type)
        {
            case TileType.BROKEN:
                changeTileType(TileType.FLOODED);
                break;
            case TileType.EMPTY :
                changeTileType(TileType.BROKEN);
                break;
        }
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
