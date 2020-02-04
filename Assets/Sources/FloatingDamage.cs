using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    private TileGrid _grid;
    Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        _grid = transform.parent.GetComponentInChildren<TileGrid>();
        _camera = Camera.main;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Reef")
        {
            _grid.doDamage();

            _camera.GetComponent<CameraShakeBehaviour>().Shake();
        }
    }
}
