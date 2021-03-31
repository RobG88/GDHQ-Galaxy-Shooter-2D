using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 8f;
    [SerializeField] float _fireRate = 0.15f;  // delay (in Seconds) how quickly the laser will fire
    float _nextFire = -1.0f;  // game time value, tracking when player/ship can fire next laser

    [SerializeField] GameObject _laserPrefab;
    [SerializeField] Transform _gunLeft, _gunRight;
    [SerializeField] Vector3 _laserOffset = new Vector3(0, 1.20f, 0); // distance offset when spawning laser Y-direction
    Animator anim;

    bool _wrapShip = false; // Q = toggle wrap
    float _xScreenClampRight = 10.75f;
    float _xScreenClampLeft = -10.75f;
    float _yScreenClampUpper = 0;
    float _yScreenClampLower = -5.2f; // offical game = -3.8f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        transform.position = new Vector3(0, -5, 0); // offical game (0, -3.5f, 0);

        UI.instance.DisplayShipWrapStatus();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            //FireLaser();
            anim.SetBool("fire", true);
            //anim.SetTrigger("isFiring");
        }

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

    void FireLaser()
    {
        _nextFire = Time.time + _fireRate; // delay (in Seconds) how quickly the laser will fire

        GameObject laser1 = Instantiate(_laserPrefab, _gunLeft.position, Quaternion.identity);
        //laser1.transform.parent = transform;
        GameObject laser2 = Instantiate(_laserPrefab, _gunRight.position, Quaternion.identity);
        //laser2.transform.parent = transform;
    }

    public void FireLaserCanon()
    {
        anim.SetBool("fire", false);
        FireLaser();
    }
}


