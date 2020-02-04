using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatModel : MonoBehaviour
{

    private TileGrid _grid;

    [SerializeField]
    private float _health;
    [SerializeField]
    private SpriteRenderer _shadowRenderer;

    public Sprite[] boatSprites;
    public Sprite[] shadowSprites;

    public float shipHealthDestructedLimit = 20f; // 80% de dégâts
    public float shipHealthOKLimit = 60f; // 40% de dégâts

    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _grid = GetComponentInChildren<TileGrid>();
        _health = 100f; // En %
    }

    // Update is called once per frame
    void Update()
    {

        UpdateShipLife();

        if(_health <= 0f && GameManager.instance.currentGameState == GameManager.enumGameState.Game)
        {
            GetComponent<BoatController>().Speed *= 1.5f;
            GameManager.instance.GameOver();
        }
    }

    public void UpdateShipLife() {
        _health = Mathf.Floor(_grid.countEmptyTiles / _grid.totalTiles * 100f);
        
        int spriteIndex = 0;
        if (_health > shipHealthDestructedLimit && _health < shipHealthOKLimit) {
           spriteIndex = 1;
        }
        if (_health <= shipHealthDestructedLimit ) {
          spriteIndex = 2;
        }
        _spriteRenderer.sprite = boatSprites[spriteIndex];
        _shadowRenderer.sprite = shadowSprites[spriteIndex];
    }
}
