using UnityEngine;
using System.Collections.Generic;

public class EliteEnemySpawner : MonoBehaviour
{
    public GameObject[] EliteEnemyPrefabs; 
    public float SpawnInterval = 30f;
    public float SpawnRadius = 20f;
    public int MaxEliteEnemies = 2;

    private float _nextSpawnTime;
    private List<GameObject> _activeEliteEnemieList = new List<GameObject>();

    private void Start()
    {
        _nextSpawnTime = Time.time + SpawnInterval;
    }

    private void Update()
    {
        _activeEliteEnemieList.RemoveAll(x => x == null);

        if (Time.time >= _nextSpawnTime && _activeEliteEnemieList.Count < MaxEliteEnemies)
        {
            SpawnEliteEnemy();
            _nextSpawnTime = Time.time + SpawnInterval;
        }
    }

    private void SpawnEliteEnemy()
    {
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * SpawnRadius;
        randomPosition.y = 1f;

        int randomIndex = Random.Range(0, EliteEnemyPrefabs.Length);
        GameObject eliteEnemy = Instantiate(EliteEnemyPrefabs[randomIndex], randomPosition, Quaternion.identity);
        
        _activeEliteEnemieList.Add(eliteEnemy);
    }
} 