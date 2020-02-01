using System.Collections;
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
    private GameObject captainPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // personne a la barre, monsieur!
        if (captainPlayer == null)
            return;

        var gamepad = Gamepad.current;
        if(gamepad == null)
            return; // No gamepad connected.

        Vector2 move = gamepad.leftStick.ReadValue();
        Vector3 moveVector = new Vector3(move.x, move.y, 0);

        if(moveVector != Vector3.zero)
        {
            Quaternion rotationMin = Quaternion.Euler(new Vector3(0f, 0f, _maxRotationAngle * -1));
            Quaternion rotationMax = Quaternion.Euler(new Vector3(0f, 0f, _maxRotationAngle));
            Quaternion rotation = transform.rotation;

            if(move.y > 0 && rotation.z < rotationMax.z)
            {
                rotation.z += Quaternion.Euler(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime)).z;
            }

            if(move.y < 0 && rotation.z > rotationMin.z)
            {
                rotation.z -= Quaternion.Euler(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime)).z;
            }
            transform.rotation = rotation;

        }

        float transformAngleZ = transform.rotation.eulerAngles.z - 180;

        Vector3 newPosition = transform.position + (Vector3.down * Mathf.Sin(transformAngleZ * Mathf.Deg2Rad) * _speed * Time.deltaTime);
        newPosition.y = Mathf.Clamp(newPosition.y, _maxBottom, _maxTop);

        transform.position = newPosition;
    }

    public void setCaptain(GameObject captainToSet)
    {
        captainPlayer = captainToSet;
    }
}
