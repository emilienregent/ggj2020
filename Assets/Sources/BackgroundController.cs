using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : AbstractScrollableController
{
    // 1024 (size) / 64 (pixel to unit)
    private const float TILE_WORLD_SIZE = 16f;

    private Vector2 _position = Vector2.zero;
    private Vector2 _leftCorner = Vector2.zero;

    private BackgroundManager _manager = null;

    override public float GetWorldSize()
    {
        // Divide by 2 as pivot is center
        return TILE_WORLD_SIZE;
    }

    public void Initialize(BackgroundManager manager)
    {
        _manager = manager;
        _position = transform.position;
    }

    private void Update()
    {
        if (IsOutOfLeftBoundary() == true)
        {
            _position = _manager.SwapToEnd(this);
        }

        _position.x -= GameManager.instance.speed * Time.deltaTime;

        transform.position = _position;
    }
}