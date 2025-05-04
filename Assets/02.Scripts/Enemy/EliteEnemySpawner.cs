using UnityEngine;
using System.Collections.Generic;

public class EliteEnemySpawner : MonoBehaviour
{
    public GameObject[] eliteEnemyPrefabs; 
    public float spawnInterval = 30f;
    public float spawnRadius = 20f;
    public int maxEliteEnemies = 2;

    private float nextSpawnTime;
    private List<GameObject> activeEliteEnemies = new List<GameObject>();

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        activeEliteEnemies.RemoveAll(item => item == null);

        if (Time.time >= nextSpawnTime && activeEliteEnemies.Count < maxEliteEnemies)
        {
            SpawnEliteEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEliteEnemy()
    {
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = 1f;

        int randomIndex = Random.Range(0, eliteEnemyPrefabs.Length);
        GameObject eliteEnemy = Instantiate(eliteEnemyPrefabs[randomIndex], randomPosition, Quaternion.identity);
        
        activeEliteEnemies.Add(eliteEnemy);
    }
} 