using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    private TileGrid _grid;
    Camera _camera;

    PolygonCollider2D objCollider;
    Plane[] planes;


    // Start is called before the first frame update
    void Start()
    {
        _grid = transform.parent.GetComponentInChildren<TileGrid>();
        _camera = Camera.main;

        planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        objCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GeometryUtility.TestPlanesAABB(planes, objCollider.bounds) == false && GameManager.instance.currentGameState == GameManager.enumGameState.Game)
        {
            GameManager.instance.GameOver();
        }
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
