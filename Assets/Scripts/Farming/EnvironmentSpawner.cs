using UnityEngine;
using System.Collections.Generic;

public class EnvironmentSpawner : MonoBehaviour
{
    [Header("Cấu hình sinh cây")]
    public GameObject[] treePrefabs;
    public int numberOfTrees = 50; // số lượng cây cần sinh
    public Vector2 spawnAreaMin;   // ví dụ: (-10, -10)
    public Vector2 spawnAreaMax;   // ví dụ: (10, 10)
    public float minDistanceBetweenTrees = 1.5f;

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        int spawned = 0;
        int maxAttempts = numberOfTrees * 10;

        while (spawned < numberOfTrees && maxAttempts > 0)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                0f
            );

            bool tooClose = false;

            foreach (var pos in spawnedPositions)
            {
                if (Vector3.Distance(pos, randomPos) < minDistanceBetweenTrees)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                int index = Random.Range(0, treePrefabs.Length);
                Instantiate(treePrefabs[index], randomPos, Quaternion.identity);
                spawnedPositions.Add(randomPos);
                spawned++;
            }

            maxAttempts--;
        }

        Debug.Log($"Đã spawn {spawned} cây.");
    }
}
