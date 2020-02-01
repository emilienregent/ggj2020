using UnityEngine;
using System.Collections;

public class ItemController : AbstractScrollableController
{
    // 1024 (size) / 128 (pixel to unit)
    private const float TILE_WORLD_SIZE = 8f;

    private Vector2 _position = Vector2.zero;

    private ItemManager _manager = null;
    private int _index = -1;

    [SerializeField]
    private ItemEnum _itemType = ItemEnum.NONE;

    [SerializeField]
    private float _speedOffset = 0f;

    public int index { get { return _index; } }
    public ItemEnum itemType { get { return _itemType; } }

    override public float GetWorldSize()
    {
        // Divide by 2 as pivot is center
        return TILE_WORLD_SIZE / 2f;
    }

    public void Initialize(ItemManager manager)
    {
        _manager = manager;
        _position = transform.position;
    }

    // Prepare item for its life
    public void SetReady(int inGameIndex)
    {
        _index = inGameIndex;

        _position.x = GameManager.instance.rightCorner.x;
        _position.y = Random.Range(
            GameManager.instance.leftCorner.y,
            GameManager.instance.rightCorner.y);

        transform.position = _position;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (IsOutOfLeftBoundary() == true)
        {
            _manager.ReleaseItem(this);
        }

        _position.x -= (GameManager.instance.speed - _speedOffset) * Time.deltaTime;

        transform.position = _position;
    }
}
