using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public const float TILE_WORLD_SIZE = 16f;

    private Vector2 _position = Vector2.zero;
    private Vector2 _leftCorner = Vector2.zero;

    private BackgroundManager _manager = null;

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

        _position.x -= _manager.speed * Time.deltaTime;

        transform.position = _position;
    }

    // Check if background has reached the left side of the screen
    public bool IsOutOfLeftBoundary()
    {
        return transform.position.x + TILE_WORLD_SIZE * 2f < _leftCorner.x;
    }
}