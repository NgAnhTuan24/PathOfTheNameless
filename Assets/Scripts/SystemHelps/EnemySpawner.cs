using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int spawnCount = 5;

    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;

    private int curentEnemyCount;
    private bool bossSpawned = false;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);

            curentEnemyCount++;

            EnemyHealth e = enemy.GetComponent<EnemyHealth>();
            if(e != null)
            {
                e.OnEnemyDeath += () =>
                {
                    curentEnemyCount--;
                    if(curentEnemyCount <= 0 && !bossSpawned)
                    {
                        SpawnBoss();
                    }
                };
            }
        }
    }

    void SpawnBoss()
    {
        Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        bossSpawned = true;
        Debug.Log("Boss Spawned");
    }
}
