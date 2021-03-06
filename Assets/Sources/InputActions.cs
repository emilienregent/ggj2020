﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActions : MonoBehaviour
{

    private bool _isPressed = false;
    [SerializeField]
    private int _speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        var gamepad = Gamepad.current;
        if(gamepad == null)
            return; // No gamepad connected.

        if(gamepad.rightTrigger.wasPressedThisFrame)
        {
            Debug.Log("right trigger");
        }

        Vector2 move = gamepad.leftStick.ReadValue();
        Vector3 moveVector = new Vector3(move.x, move.y, 0);
        transform.position = transform.position + (moveVector * _speed * Time.deltaTime);

        // Move character
        if(moveVector != Vector3.zero)
        {
            var angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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

    public void MoveUp(InputAction.CallbackContext context) {
        Debug.Log("Move UP!");
    }
}
