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
    public Transform arenaCenter;

    private int currentWave;
    private int aliveEnemies;
    private bool arenaCleared;

    private int spawnPointIndex = 0;

    private ArenaID arenaID;
    private bool bossSpawned;
    private bool arenaStarted;

    private void Awake()
    {
        arenaID = GetComponent<ArenaID>();
    }

    private void Start()
    {
        if (arenaID == null || ArenaSaveManager.Instance == null) return;

        var progress = ArenaSaveManager.Instance.GetProgress(arenaID.GetID());
        if (progress != null)
        {
            currentWave = progress.currentWave;
            bossSpawned = progress.bossSpawned;
            arenaCleared = progress.cleared;
        }
    }

    public void StartArena()
    {
        if (arenaCleared || arenaStarted) return;

        arenaStarted = true;
        SaveProgress();
        StartCoroutine(SpawnWave());
    }

    public void ResumeArena()
    {
        if (arenaCleared || arenaStarted) return;

        arenaStarted = true;

        if (currentWave >= waves.Count)
        {
            if (!bossSpawned)
            {
                SpawnBoss();
            }
            return;
        }

        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        if (currentWave < 0 || currentWave >= waves.Count)
            yield break;

        spawnPointIndex = 0;

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
            spawnPointIndex = 0;

        GameObject enemy = Instantiate(prefab, point.position, Quaternion.identity);

        aliveEnemies++;

        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyHealth.OnEnemyDeath += OnEnemyKilled;
    }


    void OnEnemyKilled()
    {
        aliveEnemies--;

        if (aliveEnemies > 0) return;

        currentWave++;
        SaveProgress();

        if (currentWave < waves.Count)
        {
            StartCoroutine(SpawnWave());
        }
        else if (!bossSpawned)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        var boss = Instantiate(bossPrefab, arenaCenter.position, Quaternion.identity);
        boss.GetComponent<EnemyHealth>().OnEnemyDeath += OnBossKilled;
    }


    void OnBossKilled()
    {
        arenaCleared = true;
        SaveProgress();

        ArenaSaveManager.Instance.MarkCleared(arenaID.GetID());

        Instantiate(chestPrefab, arenaCenter.position, Quaternion.identity);
    }
    void SaveProgress()
    {
        ArenaSaveManager.Instance.SetProgress(
            arenaID.GetID(),
            currentWave,
            bossSpawned,
            arenaCleared
        );
    }
}
