using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemiesManager : MonoBehaviour
{
    [Inject] private PlayerController _player;
    [Inject] private Transform _enemySpawnPoint;
    [Inject] private DiContainer _container;
    [Inject] private List<GameObject> _enemies; 
    public EnemyController _currentEnemy;

    public void SpawnEnemy()
    {
        _currentEnemy = _container.InstantiatePrefab(_enemies[Random.Range(0, _enemies.Count)], _enemySpawnPoint.position, Quaternion.Euler(0, -90, 0), null)
            .GetComponent<EnemyController>();
    }
}
