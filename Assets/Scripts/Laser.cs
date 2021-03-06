using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    ////////////////////////////////
    /// Laser (Player)
    /// 
    /// Laser gameObject travels UP @ _speed
    /// 
    /// Laser triggers/collides with object &
    /// is destroyed or self-desrtucts when
    /// position.y > _yThreshold (8)
    /// 
    ///

    [SerializeField] float _speed = 13.0f;
    float _yThreshold = 8.0f;

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > _yThreshold)
            Destroy(transform.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Asteroid")
        {
            Destroy(gameObject);
        }
    }
}
