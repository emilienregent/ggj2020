﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private float _rotationSpeed = 3f;

    [SerializeField]
    private float _maxRotationAngle = 45f;

    [SerializeField]
    private float _maxTop = 4f;
    [SerializeField]
    private float _maxBottom = -4f;

    [SerializeField]
    private GameObject _captainPlayer;
    public GameObject captainPlayer { get { return _captainPlayer; } }

    public float Speed { get => _speed; set => _speed = value; }

    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Quaternion rotationMin = Quaternion.Euler(new Vector3(0f, 0f, _maxRotationAngle * -1));
        Quaternion rotationMax = Quaternion.Euler(new Vector3(0f, 0f, _maxRotationAngle));
        Quaternion rotation = transform.rotation;
        float transformAngleZ = transform.rotation.eulerAngles.z - 180;

        Vector3 newPosition = transform.position + (Vector3.down * Mathf.Sin(transformAngleZ * Mathf.Deg2Rad) * Speed * Time.deltaTime);
        if(transform.position.x < _startPosition.x && GameManager.instance.currentGameState == GameManager.enumGameState.Game)
        {
            newPosition = newPosition + (Vector3.right * Speed * 0.5f * Time.deltaTime);
        }
      
        // Cas particulier du end game
        if (
            GameManager.instance.currentGameState == GameManager.enumGameState.GameOver ||
            GameManager.instance.currentGameState == GameManager.enumGameState.End
        )
        {
            Vector3 moveVectorBackward = new Vector3(-0.8f, 0, 0);
            transform.position = transform.position + (moveVectorBackward * Speed * Time.deltaTime);

            rotation.z += Quaternion.Euler(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime)).z;
            transform.rotation = rotation;

            if(transform.position.x < -18f && GameManager.instance.currentGameState != GameManager.enumGameState.End)
            {
                GameManager.instance.EndGame();
            }

            return;
        }

        newPosition.y = Mathf.Clamp(newPosition.y, _maxBottom, _maxTop);

        transform.position = newPosition;

        if(newPosition.y <= _maxBottom && rotation.z != 0)
        {
            rotation.z += Quaternion.Euler(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime)).z;
        }

        if(newPosition.y >= _maxTop && rotation.z != 0)
        {
            rotation.z -= Quaternion.Euler(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime)).z;
        }

        if(rotation.z > rotationMax.z)
        {
            rotation.z -= Quaternion.Euler(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime)).z;
        }

        if(rotation.z < rotationMin.z)
        {
            rotation.z += Quaternion.Euler(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime)).z;
        }

        if(transform.rotation != rotation)
        {
            transform.rotation = rotation;
        }

        // personne a la barre, monsieur!
        if (_captainPlayer == null)
            return;

        Vector2 move = _captainPlayer.GetComponent<PlayerController>().move;
        Vector3 moveVector = new Vector3(move.x, move.y, 0);
        
        if(moveVector != Vector3.zero)
        {
            if(move.y > 0 && rotation.z < rotationMax.z)
            {
                rotation.z += Quaternion.Euler(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime)).z;
            }

            if(move.y < 0 && rotation.z > rotationMin.z)
            {
                rotation.z -= Quaternion.Euler(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime)).z;
            }
            transform.rotation = rotation;

            _captainPlayer.GetComponent<PlayerModel>().SetAnimationValue("Direction", move.y);
        }
        
    }

    public void setCaptain(GameObject captainToSet)
    {
        if(captainToSet == null || _captainPlayer == null)
        {
            _captainPlayer = captainToSet;
        }
    }

    public void OnBecameInvisible() {
        if(GameManager.instance.currentGameState == GameManager.enumGameState.Game)
        {
            GameManager.instance.EndGame();
        }
    }
}
