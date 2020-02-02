using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance = null;
    private static object _lock = new object();

    public PlayerController[] players = new PlayerController[4];

    public StartMenuController startMenuController;
    public EndScreenController endScreenController;

    public AudioManager audioManager;

    private PlayerInputManager _playerInputManager;

    // Game Elements
    public BoatController ship;

    public enum enumGameState { Menu, Game, GameOver, End };
    public enumGameState currentGameState;

    private float _timeSpentBeforeStart = 0f;

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

        currentGameState = GameManager.enumGameState.Menu;
        PlayerController.Index = 0;       
    }

    public void PlayerJoined(PlayerController player)
    {
        _instance.players[player.currentPlayerIndex] = player;
        startMenuController.PlayerJoin();
    }

    public void reloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        _timeSpentBeforeStart = Time.timeSinceLevelLoad;
        currentGameState = GameManager.enumGameState.Game;
        ship.GetComponentInChildren<TileGrid>().StartDamageShip();
    }

    public void GameOver() {
        currentGameState = GameManager.enumGameState.GameOver;
        endScreenController.score.text = Mathf.RoundToInt(Time.timeSinceLevelLoad - _timeSpentBeforeStart).ToString();
        ship.GetComponentInChildren<TileGrid>().StopDamageShip();
    }

    public void EndGame()
    {
        currentGameState = GameManager.enumGameState.End;
        endScreenController.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            reloadGame();
    }

}
