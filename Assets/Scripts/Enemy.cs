using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[SerializeField] int _scoreValue = 0;
    [SerializeField] float _enemySpeed = 4.0f;
    //[SerializeField] float _enemyLaser = 6.0f;  // speed of enemy's laser
    //[SerializeField] GameObject _enemyLaserPrefab;

    [SerializeField] GameObject _enemyInvaderExplosion;
    [SerializeField] GameObject _enemyChild;

    BoxCollider enemyCollider;

    //float _fireRate = 3.0f;
    //float _canFire = -1.0f;
    bool _isDestroyed = false; // if enemy is hit by player/ship Laser then isDestroyed = true, enemy is put back into pool

    float _enemyReSpawnThreshold = -6.0f; // Game Screen threshold, once enemy is beyond this point and has been destroyed it will respawn 
    Vector3 _enemyPos = new Vector3(0, 0, 0); // Random position of enemy once re-spawned X(-8,8) Y(12,9)

    float _respawnXmin = -9.0f;
    float _respawnXmax = 9.0f;

    void Awake()
    {
        enemyCollider = GetComponent<BoxCollider>();
    }

    void Start()
    {
        Spawn();
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if (transform.position.y < _enemyReSpawnThreshold && !_isDestroyed)
        {
            transform.position = SpawnEnmeyAtRandomLocation();
        }
    }

    Vector3 SpawnEnmeyAtRandomLocation()
    {
        return (_enemyPos = new Vector3(Random.Range(_respawnXmin, _respawnXmax), Random.Range(7.0f, 12.0f), 0));
    }

    void Spawn()
    {
        float respawnX = Random.Range(_respawnXmin, _respawnXmax);
        _enemyPos.x = respawnX;
        //_enemyPos.x = 0; // testing, line enemy for two lasers intersecting collider
        _enemyPos.y = Random.Range(7, 12);
        transform.position = _enemyPos;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser" || other.tag == "Player")
        {
            enemyCollider.enabled = false; // disable collider so two lasers can not collider at the same time
            Instantiate(_enemyInvaderExplosion, transform.position, Quaternion.identity);
            _enemyChild.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        WaveSpawner.instance.EnemyDeath();
    }
}
