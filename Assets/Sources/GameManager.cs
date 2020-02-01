using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance = null;
    private static object _lock = new object();

    public PlayerController[] players = new PlayerController[4];

    public MenuController menuController;

    public static GameManager instance
    {
        get
        {
            lock(_lock)
            {
                if (_instance == null)
                {
                    // Search for existing instance.
                    _instance = (GameManager)FindObjectOfType(typeof(GameManager));
                    
                    // Create new instance if one doesn't already exist.
                    if (_instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<GameManager>();
                        singletonObject.name = "GameManager (Singleton auto-created)";
                    }
                }

                return _instance;
            }
        }
    }

    // Properties
    [SerializeField]
    private float _speed = 0f;

    private Vector2 _leftCorner = Vector2.zero;
    private Vector2 _rightCorner = Vector2.zero;

    public float speed { get { return _speed; } }
    public Vector2 leftCorner { get { return _leftCorner; } }
    public Vector2 rightCorner { get { return _rightCorner; } }


    private void Start()
    {
        _leftCorner = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        _rightCorner = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }

    public void PlayerJoined(PlayerController player)
    {

        _instance.players[player.currentPlayerIndex] = player;
        menuController.PlayerJoin(player);
    }

}
