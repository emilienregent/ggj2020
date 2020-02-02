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
    private float _tileSizeX = .5f;
    [SerializeField]
    private float _tileSizeY = .5f;

    private ArrayList _availableTiles;
    private ArrayList _unavailableTiles;

    [SerializeField]
    private int _countEmptyTiles;
    public float countEmptyTiles { get { return _countEmptyTiles; } }
    [SerializeField]
    private int _countBrokenTiles;
    [SerializeField]
    private int _countFloodedTiles;

    public float totalTiles { get { return _width * _height; } }

    public int CountFloodedTiles { get => _countFloodedTiles; set => _countFloodedTiles = value; }
    public int CountBrokenTiles { get => _countBrokenTiles; set => _countBrokenTiles = value; }

    public GameObject tilePrefab;

    [SerializeField]
    private float _playtimeTreshold = 0f;
    [SerializeField]
    private float _minimumDelay = 0f;
    [SerializeField]
    private float _maximumDelay = 0f;
    [SerializeField]
    private AnimationCurve _difficultyByTime = new AnimationCurve();
    [SerializeField]
    private AnimationCurve _spawnDelayByDifficulty = new AnimationCurve();

    private bool _damageShipStarted = false;

    public float playerNumberImpact = 0.65f;

    // Start is called before the first frame update
    void Start()
    {
        _availableTiles = new ArrayList();
        _unavailableTiles = new ArrayList();

        CountBrokenTiles = 0;
        CountFloodedTiles = 0;

        for(int i = 0; i < _width; i++)
        {
            for(int j = 0; j < _height; j++)
            {
                // code gardé de côté
                //Random random = new Random();
                //float maximum = _tileSizeX / 2.0f;
                //float minimum = -1* _tileSizeX / 2.0f;
                //int randomXInt = Random.Range((int)(minimum*1000), (int)(maximum*1000));
                //float randomX = randomXInt / 1000.0f;

                //float maximumY = _tileSizeY / 2.0f;
                //float minimumY = -1 * _tileSizeY / 2.0f;
                //int randomYInt = Random.Range((int)(minimumY * 1000), (int)(maximumY * 1000));
                //float randomY = randomXInt / 1000.0f;

                Vector3 position = new Vector3(
                    transform.position.x + _tileSizeX * i, 
                    transform.position.y + _tileSizeY * j, 
                    0
                );

                GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
                newTile.GetComponent<Tile>().SetGrid(this);
                newTile.transform.SetParent(transform);
                newTile.name = "Tile " + i + "-" + j;
                _availableTiles.Add(newTile);
            }
        }

        _countEmptyTiles = _width * _height;

    }

    public void StartDamageShip()
    {
        if(_damageShipStarted == false && GameManager.instance.currentGameState == GameManager.enumGameState.Game)
        {
            _damageShipStarted = true;
            if(PlayerController.Index > 1)
            {
                _minimumDelay = _minimumDelay / (PlayerController.Index * playerNumberImpact);
            }
            StartCoroutine("DamageShip");
        }
    }

    public void StopDamageShip()
    {
        if (_damageShipStarted == true)
        {
            _damageShipStarted = false;
            StopCoroutine("DamageShip");
        }
    }

    private IEnumerator DamageShip() {
        while(true)
        {
            float difficulty = _difficultyByTime.Evaluate(GameManager.instance.playTime / _playtimeTreshold);

            // From maximum delay (no difficulty) to minimum delay
            float delay = Mathf.Clamp(
                _maximumDelay * _spawnDelayByDifficulty.Evaluate(difficulty),
                _minimumDelay,
                _maximumDelay
            );

            yield return new WaitForSeconds(delay);
            doDamage();

        }
    }

    public void doDamage()
    {
        if (_availableTiles.Count > 0)
        {
            int random = Random.Range(0, _availableTiles.Count - 1);
            GameObject selectedTile = _availableTiles[random] as GameObject;
            TileType newType = selectedTile.GetComponent<Tile>().DoDamage();

            switch (newType)
            {
                case TileType.BROKEN:
                    _countEmptyTiles--;
                    CountBrokenTiles++;
                    break;

                case TileType.FLOODED:
                    CountBrokenTiles--;
                    CountFloodedTiles++;
                    _availableTiles.RemoveAt(random);
                    _unavailableTiles.Add(selectedTile);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartDamageShip();
    }

    // Met à jour les Tiles dispo/pas dispo
    public void updateList(GameObject tile) {
       if(_unavailableTiles.Contains(tile) == true)
        {
            _unavailableTiles.Remove(tile);
            _availableTiles.Add(tile);
            CountFloodedTiles--;
            if(tile.GetComponent<Tile>().type == TileType.EMPTY)
            {
                _countEmptyTiles++;
            } else
            {
                CountBrokenTiles++;
            }
        } else if(_availableTiles.Contains(tile) == true)
        {
            if(tile.GetComponent<Tile>().type == TileType.EMPTY)
            {
                _countEmptyTiles++;
                CountBrokenTiles--;
            }
        }
    }
}
