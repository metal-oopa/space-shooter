using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _uiManager; // UI Manager

    [SerializeField]
    private GameObject _playerPrefab; // player prefab

    [SerializeField]
    private GameObject _enemyPrefab; // enemy prefab

    [SerializeField]
    private GameObject _enemyContainer; // enemy container

    [SerializeField]
    private GameObject[] _powerups; // powerup prefabs

    [SerializeField]
    private GameObject _powerupContainer; // powerup container
    private Coroutine _spawnEnemyCoroutine;
    private Coroutine _spawnPowerupCoroutine;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3.0f);
        while (true)
        {
            float randomX = Random.Range(-9.0f, 9.0f);
            GameObject newEnemy = Instantiate(
                _enemyPrefab,
                new Vector3(randomX, 7.5f, 0),
                Quaternion.identity
            );
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(3.0f);
        while (true)
        {
            if (_powerups.Length <= 0)
            {
                yield return new WaitForSeconds(5.0f);
                continue;
            }

            int randomPowerup = Random.Range(0, _powerups.Length);
            float randomX = Random.Range(-9.0f, 9.0f);
            GameObject newTripleShotPowerup = Instantiate(
                _powerups[randomPowerup],
                new Vector3(randomX, 7.5f, 0),
                Quaternion.identity
            );
            newTripleShotPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(10.0f, 15.0f));
        }
    }

    public void OnPlayerDeath()
    {
        StopCoroutine(_spawnEnemyCoroutine);
        StopCoroutine(_spawnPowerupCoroutine);
    }

    public void StartSpawning()
    {
        if (_spawnEnemyCoroutine != null || _spawnPowerupCoroutine != null)
        {
            return;
        }
        _spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
        _spawnPowerupCoroutine = StartCoroutine(SpawnPowerup());
    }
}
