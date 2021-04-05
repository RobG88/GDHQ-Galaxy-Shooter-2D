using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject enemyPrefab;
        public int enemyCount;
        public float spawnRate;
        public float delayBetweenEnemySpawns; // seconds between enemy spawning
    }

    public enum SpawnState { SPAWNING, WAITING, COUNTING }
    [SerializeField] SpawnState _spawnState = SpawnState.COUNTING;

    public Wave[] waves;

    int _nextWave = 0; // current wave number

    [SerializeField] float _timeBetweenWaves = 15.0f;
    [SerializeField] float _waveCountdown;

    float _searchForEnemyCountdown = 1f;  // how ofter to search for remaining enemies in the scene

    void Start()
    {
        _waveCountdown = _timeBetweenWaves;
    }

    void Update()
    {
        if (_spawnState == SpawnState.WAITING)
        {
            // check if any enemies remain in scene
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return; // Wait for player to destroy all enemies
            }
        }

        if (_waveCountdown <= 0)
        {
            if (_spawnState != SpawnState.SPAWNING)
            {
                // Begin spawning the wave
                StartCoroutine(SpawnWave(waves[_nextWave]));
            }
        }
        else
        {
            // Countdown 
            _waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        // Begin a new Wave
        // Wave Over
        // Give Name of next wave
        // Start a wave countdown
        // Depending on performance a bonus
        Debug.Log("All Enemies Destroyed --- Wave Completed!");

        _spawnState = SpawnState.COUNTING;
        _waveCountdown = _timeBetweenWaves;

        if (_nextWave + 1 > waves.Length - 1)
        {
            _nextWave = 0;

            // Because all waves are completed
            // Game difficulty could be increased by an enemy stat multiplier
            // Earn perks, bonus, shields, weapons, defense, bombs, nukes, specials

            Debug.Log("All WAVES Complete! ... Loopinng");

            // Game Completed rather then looping
            // Begin a new scene ... new level of the game
        }
        else
        {
            _nextWave++; // increment the NextWave Index
        }

        return;
    }

    bool EnemyIsAlive()
    {
        // Instead of looking for Enemies, use the enemyCount in Wave
        // and update with each enemy destoryed, when 0 return true

        _searchForEnemyCountdown -= Time.deltaTime;

        if (_searchForEnemyCountdown <= 0f)
        {
            _searchForEnemyCountdown = 1f; // reset timer

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        Debug.Log("Wave Enemy Count: " + _wave.enemyCount);

        _spawnState = SpawnState.SPAWNING;

        // Spawn enemies
        for (int i = 0; i < _wave.enemyCount; i++)
        {
            SpawnEnemy(_wave.enemyPrefab);
            // Wait for next enemy in wave to spawn
            // yield return new WaitForSeconds(1f / _wave.spawnRate);
            yield return new WaitForSeconds(_wave.delayBetweenEnemySpawns);
        }

        _spawnState = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(GameObject _enemyPrefab)
    {
        Debug.Log("Spawning Enemy: " + _enemyPrefab.name);
        GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(0, 10, 0), Quaternion.identity);
    }
}
