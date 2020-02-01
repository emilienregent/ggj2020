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

    private GameObject[,] _grid;

    public GameObject tilePrefab;

    private float currentTime = 0f;
    [SerializeField]
    private float refreshTime = 2f;


    // Start is called before the first frame update
    void Start()
    {
        _grid = new GameObject[_width, _height];

        for(int i = 0; i < _width; i++)
        {
            for(int j = 0; j < _height; j++)
            {
                _grid[i,j] = Instantiate(tilePrefab, new Vector3(transform.position.x + _tileSize * i, transform.position.y + _tileSize * j, 0), Quaternion.identity);
                _grid[i, j].transform.SetParent(transform);
                _grid[i, j].name = "Tile " + i + "-" + j;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > currentTime)
        {
            currentTime += refreshTime;
            int randomX = Random.Range(0, _width - 1);
            int randomY = Random.Range(0, _height -1);

            _grid[randomX, randomY].GetComponent<Tile>().DoDamage();
        }
    }
}
