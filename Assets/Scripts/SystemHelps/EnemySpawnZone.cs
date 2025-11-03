using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int count;
}

[System.Serializable]
public class Wave
{
    public EnemySpawnInfo[] enemies;
}


public class EnemySpawnZone : MonoBehaviour
{
    [Header("Cấu hình wave spawn enemy")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawnPoints;

    private int enemiesAlive = 0;
    private int currentWave = 0;
    private bool zoneFinished = false;

    private void Start()
    {
        StartWave(1);
    }

    private void StartWave(int waveNumber)
    {
        if (waveNumber > waves.Length || zoneFinished) return;

        currentWave = waveNumber;
        Wave wave = waves[waveNumber - 1];

        // Tính tổng số quái trong wave
        enemiesAlive = 0;
        foreach (var info in wave.enemies)
            enemiesAlive += info.count;

        int spawnIndex = 0;

        // Spawn từng loại quái theo số lượng chỉ định
        foreach (var info in wave.enemies)
        {
            for (int i = 0; i < info.count; i++)
            {
                Transform spawnPoint = spawnPoints[spawnIndex % spawnPoints.Length];
                spawnIndex++;

                GameObject enemy = Instantiate(info.enemyPrefab, spawnPoint.position, Quaternion.identity);

                EnemyHealth health = enemy.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.OnEnemyDeath += () => EnemyKilled();
                }
            }
        }
    }

    private void EnemyKilled()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            if (currentWave < waves.Length)
                StartWave(currentWave + 1);
            else
                zoneFinished = true; // Xong tất cả wave
        }
    }
}
