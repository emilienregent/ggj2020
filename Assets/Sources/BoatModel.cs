using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatModel : MonoBehaviour
{

    private TileGrid _grid;

    [SerializeField]
    private float _health;

    // Start is called before the first frame update
    void Start()
    {
        _grid = GetComponentInChildren<TileGrid>();
        _health = 100f; // En %
    }

    // Update is called once per frame
    void Update()
    {

        UpdateShipLife();

        if(_health <= 0f && GameManager.instance.currentGameState == GameManager.enumGameState.Game)
        {
            GameManager.instance.GameOver();
        }
    }

    public void UpdateShipLife() {
        _health = Mathf.Round(_grid.countEmptyTiles / _grid.totalTiles * 100f);
    }
}
