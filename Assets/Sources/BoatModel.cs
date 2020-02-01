using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatModel : MonoBehaviour

{

    private TileGrid _grid;

    // Start is called before the first frame update
    void Start()
    {
        _grid = GetComponentInChildren<TileGrid>();   
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float getShipLife() {
        return Mathf.Round(_grid.countEmptyTiles / _grid.totalTiles * 100f);
    }
}
