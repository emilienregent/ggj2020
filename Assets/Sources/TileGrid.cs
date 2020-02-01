using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileGrid : MonoBehaviour
{

    [SerializeField]
    private int _width = 5;
    [SerializeField]
    private int _height = 2;

    [SerializeField]
    private float _tileSize = 1f;

    private ArrayList _availableTiles;
    private ArrayList _unavailableTiles;

    [SerializeField]
    private int countEmptyTiles;
    [SerializeField]
    private int countBrokenTiles;
    [SerializeField]
    private int countFloodedTiles;

    public GameObject tilePrefab;

    private float currentTime = 0f;
    [SerializeField]
    private float refreshTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _availableTiles = new ArrayList();
        _unavailableTiles = new ArrayList();

        countBrokenTiles = 0;
        countFloodedTiles = 0;

        for(int i = 0; i < _width; i++)
        {
            for(int j = 0; j < _height; j++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(transform.position.x + _tileSize * i, transform.position.y + _tileSize * j, 0), Quaternion.identity);
                newTile.transform.SetParent(transform);
                newTile.name = "Tile " + i + "-" + j;
                _availableTiles.Add(newTile);
            }
        }

        countEmptyTiles = _width * _height;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > currentTime)
        {
            currentTime += refreshTime;
            if(_availableTiles.Count >0)
            {
                int random = Random.Range(0, _availableTiles.Count - 1);
                GameObject selectedTile = _availableTiles[random] as GameObject;
                TileType newType = selectedTile.GetComponent<Tile>().DoDamage();

                switch(newType)
                {
                    case TileType.BROKEN:
                        countEmptyTiles--;
                        countBrokenTiles++;
                        break;
                    case TileType.FLOODED:
                        countBrokenTiles--;
                        countFloodedTiles++;
                        _availableTiles.RemoveAt(random);
                        _unavailableTiles.Add(selectedTile);
                        break;
                }
            }
        }
    }
}
