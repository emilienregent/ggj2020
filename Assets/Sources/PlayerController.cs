using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int _speed = 5;
    private PlayerModel model;

    public static int Index = 0;
    public int currentPlayerIndex = -1;

    private void Awake()
    {
        currentPlayerIndex = Index++;
        GameManager.instance.PlayerJoined(this);
    }

    private Vector2 _move = Vector2.zero;
    public Vector2 move { get { return _move; } }

    // Start is called before the first frame update
    void Start()
    {
        model = GetComponent<PlayerModel>();
        model.setColorFromIndex(currentPlayerIndex);
        gameObject.transform.position = new Vector2(-5f, 0f);
    }
    
    private void FixedUpdate() {
        Move();
    }

    private void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.enumGameState.End)
        {
            if(Input.GetButtonDown("ResetGame") == true)
            {
                GameManager.instance.reloadGame();
            }
        }
    }

    private void Move() {

        if (model.canMove() == false)
        {
            return;
        }
        Vector3 moveVector = new Vector3(_move.x, _move.y, 0);
        transform.position = transform.position + (moveVector * _speed * Time.deltaTime);

        // Move character
        if(moveVector != Vector3.zero)
        {
            model.PlayAnimation("Walking", true);

            float angle = Mathf.Atan2(_move.y, _move.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            model.checkJob();
        } else
        {
            model.PlayAnimation("Walking", false);
        }

    }

    private void OnMove(InputValue inputValue) {
        _move = inputValue.Get<Vector2>();
    }

    private void OnDoAction() {
        Debug.Log("OnDoAction : Do something");
    }

    private void OnActionPressed()
    {
        model.actionStart();
    }

    private void OnActionReleased()
    {
        model.actionStop();
    }

}
