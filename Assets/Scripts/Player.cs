using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 8f;

    bool _wrapShip = false; // Q = toggle wrap
    float _xScreenClampRight = 10.75f;
    float _xScreenClampLeft = -10.75f;
    float _yScreenClampUpper = 0;
    float _yScreenClampLower = -5.2f; // offical game = -3.8f;

    void Start()
    {
        transform.position = new Vector3(0, -5, 0); // offical game (0, -3.5f, 0);

        UI.instance.DisplayShipWrapStatus();
    }

    void Update()
    {
        // Cheat Keys:
        // 
        // Q = Enable ship wrapping left/right
        // Below is for testing purposes only
        //
        if (Input.GetKeyDown(KeyCode.Q)) { _wrapShip = !_wrapShip; UI.instance.SetCheatKey(_wrapShip); UI.instance.DisplayShipWrapStatus(); }

        CalculateMovement();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        // Player Boundaries 
        // Clamp Ship's Y pos
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _yScreenClampLower, _yScreenClampUpper), 0);

        // Clamp xPos
        if (_wrapShip && transform.position.x > _xScreenClampRight) // Wrap ship
        {
            transform.position = new Vector3(_xScreenClampLeft, transform.position.y, 0);
        }
        else if (!_wrapShip && transform.position.x > _xScreenClampRight) // Lock pos
        {
            transform.position = new Vector3(_xScreenClampRight, transform.position.y, 0);
        }

        // or Wrap Ship's X pos
        if (_wrapShip && transform.position.x < _xScreenClampLeft) // Wrap ship
        {
            transform.position = new Vector3(_xScreenClampRight, transform.position.y, 0);
        }
        else if (!_wrapShip && transform.position.x < _xScreenClampLeft) // Lock pos 
        {
            transform.position = new Vector3(_xScreenClampLeft, transform.position.y, 0);
        }
    }
}


