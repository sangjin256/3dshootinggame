using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    // Enemy 타입별로 개별적인 풀 생성
    public List<GameObjectPool<Enemy>> EnemyPoolList;
    public List<GameObject> EnemyPrefabList;
    public List<Vector3> SpawnPointList;
    public GameObject SpawnPointParent;
    public int PoolSize;

    public float SpawnCooltime;
    private float _spawnElapsedTime = 0f;
    public float ClusterCooltime;
    private float _clusterElapsedTime = 0f;

    public int[] SpawnCountForRound = {5, 10, 15, 20, 25};
    private int _indexSpawnCountArr = 0;
    private int _currentSpawned = 0;

    private List<Vector3> _placedEnemyPositionList = new List<Vector3>();

    private void Start()
    {
        EnemyPoolList = new List<GameObjectPool<Enemy>>();

        for (int i = 0; i < EnemyPrefabList.Count; i++)
        {
            EnemyPoolList.Add(new GameObjectPool<Enemy>(EnemyPrefabList[i], PoolSize));
        }

        SpawnPointList = new List<Vector3>();
        for(int i = 0; i < SpawnPointParent.transform.childCount; i++)
        {
            SpawnPointList.Add(SpawnPointParent.transform.GetChild(i).position);
        }
    }

    private void Update()
    {
        if (_indexSpawnCountArr >= SpawnCountForRound.Length) return;
        if(_currentSpawned < SpawnCountForRound[_indexSpawnCountArr])
        {
            _clusterElapsedTime += Time.deltaTime;
            if (_clusterElapsedTime >= ClusterCooltime)
            {
                Vector3 position = SpawnPointList[Random.Range(0, SpawnPointList.Count)];
                _currentSpawned += SpawnEnemyCluster(position, (SpawnCountForRound[_indexSpawnCountArr] - _currentSpawned), 5, 5);
                _clusterElapsedTime = 0;
            }
        }
        else
        {
            _spawnElapsedTime += Time.deltaTime;
            if(_spawnElapsedTime >= SpawnCooltime)
            {
                _currentSpawned = 0;
                _spawnElapsedTime = 0;
                _indexSpawnCountArr++;
            }
        }
    }

    public int SpawnEnemyCluster(Vector3 center, int spawnCount, float radiusX, float radiusY)
    {
        int tries = 0;
        int spawned = 0;
        _placedEnemyPositionList = new List<Vector3>();
        while (spawned < spawnCount && tries < spawnCount * 10)
        {
            tries++;
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = Random.Range(0f, 1f);

            float x = Mathf.Cos(angle) * radius * radiusX;
            float y = Mathf.Sin(angle) * radius * radiusY;

            Vector3 offset = new Vector3(x, 1.1f, y);
            Vector3 spawnPoint = center + offset;

            bool tooClose = _placedEnemyPositionList.Any(pos => Vector3.Distance(pos, spawnPoint) < 0.5f);
            if (tooClose) continue;

            _placedEnemyPositionList.Add(spawnPoint);
            int index = Random.Range(0, 2);
            Enemy enemy = EnemyPoolList[index].Get();
            enemy.transform.position = spawnPoint;
            spawned++;
        }

        return spawned;
    }
}
