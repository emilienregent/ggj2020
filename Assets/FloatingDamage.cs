using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    private TileGrid _grid;

    // Start is called before the first frame update
    void Start()
    {
        _grid = transform.parent.GetComponentInChildren<TileGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Reef")
        {
            _grid.doDamage();
            //add animation feedback
        }
    }
}
