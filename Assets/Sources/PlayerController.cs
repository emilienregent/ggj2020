using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool _isPressed = false;
    [SerializeField]
    private int _speed = 5;

    private SpriteRenderer _sprite;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FixedUpdate() {
        Gamepad gamepad = Gamepad.current;
        if(gamepad == null)
            return; // No gamepad connected.

        if (canMove() == false)
        {
            return;
        }

        Vector2 move = gamepad.rightStick.ReadValue();
        Vector3 moveVector = new Vector3(move.x, move.y, 0);
        transform.position = transform.position + (moveVector * _speed * Time.deltaTime);

        // Move character
        if(moveVector != Vector3.zero)
        {
            _sprite.gameObject.GetComponent<Animator>().SetBool("Walking", true);
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } else
        {
            _sprite.gameObject.GetComponent<Animator>().SetBool("Walking", false);
        }

        if(_isPressed == false && gamepad.buttonSouth.isPressed)
        {
            Debug.Log("BUTTON A PRESSED");
            _isPressed = true;
        }

        if(_isPressed == true && gamepad.buttonSouth.isPressed == false)
        {
            Debug.Log("BUTTON A RELEASED");
            _isPressed = false;
        }
    }

    public bool canMove()
    {
        PlayerModel model = GetComponent<PlayerModel>();
        return model.getCurrentJob() == Jobs.None;
    }

}
