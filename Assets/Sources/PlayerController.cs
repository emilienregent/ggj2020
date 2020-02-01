using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool _isPressed = false;
    [SerializeField]
    private int _speed = 5;
    private PlayerModel model;

    private SpriteRenderer _sprite;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private Vector2 _move = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        model = GetComponent<PlayerModel>();
    }
    
    void FixedUpdate() {
        Move();
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
            _sprite.gameObject.GetComponent<Animator>().SetBool("Walking", true);
            float angle = Mathf.Atan2(_move.y, _move.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            model.checkJob();
        } else
        {
            _sprite.gameObject.GetComponent<Animator>().SetBool("Walking", false);
        }

    }

    private void OnMove(InputValue inputValue) {
        _move = inputValue.Get<Vector2>();
    }

    private void OnDoAction() {
        Debug.Log("PRESS A");
    }

    private void OnActionPressed()
    {
        Debug.Log("startA");
        model.actionStart();
    }

    private void OnActionReleased()
    {
        Debug.Log("stopA");
        model.actionStop();
    }

}
