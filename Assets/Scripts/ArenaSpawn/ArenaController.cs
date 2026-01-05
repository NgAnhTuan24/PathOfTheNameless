using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public GameObject enemyPrefab;
    public int count;
    public float spawnDelay;
}

public class ArenaController : MonoBehaviour
{
    [Header("Waves")]
    public List<WaveData> waves;
    public Transform[] enemySpawnPoints;

    [Header("Boss & Reward")]
    public GameObject bossPrefab;
    public GameObject chestPrefab;
    public Transform arenaCenter; // đúng vị trí khoanh đỏ

    private int currentWave;
    private int aliveEnemies;
    private bool arenaCleared;

    private int spawnPointIndex = 0;

    private ArenaID arenaID;

    private void Awake()
    {
        arenaID = GetComponent<ArenaID>();
    }

    private void Start()
    {
        if (arenaID != null &&
            ArenaSaveManager.Instance != null &&
            ArenaSaveManager.Instance.IsArenaCleared(arenaID.GetID()))
        {
            arenaCleared = true;
        }
    }

    public void StartArena()
    {
        if (arenaCleared) return;

        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        spawnPointIndex = 0; // reset mỗi wave

        WaveData wave = waves[currentWave];

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(wave.spawnDelay);
        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        if (enemySpawnPoints.Length == 0) return;

        Transform point = enemySpawnPoints[spawnPointIndex];

        spawnPointIndex++;

        if (spawnPointIndex >= enemySpawnPoints.Length)
            spawnPointIndex = 0; // quay vòng

        GameObject enemy = Instantiate(prefab, point.position, Quaternion.identity);

        aliveEnemies++;

        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyHealth.OnEnemyDeath += OnEnemyKilled;
    }


    void OnEnemyKilled()
    {
        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            currentWave++;

            if (currentWave < waves.Count)
            {
                StartCoroutine(SpawnWave());
            }
            else
            {
                SpawnBoss();
            }
        }
    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(
            bossPrefab,
            arenaCenter.position,
            Quaternion.identity
        );

        EnemyHealth bossHealth = boss.GetComponent<EnemyHealth>();
        bossHealth.OnEnemyDeath += OnBossKilled;
    }


    void OnBossKilled()
    {
        arenaCleared = true;

        if (arenaID != null)
        {
            ArenaSaveManager.Instance.MarkCleared(arenaID.GetID());
        }

        Instantiate(
            chestPrefab,
            arenaCenter.position,
            Quaternion.identity
        );
    }

}
