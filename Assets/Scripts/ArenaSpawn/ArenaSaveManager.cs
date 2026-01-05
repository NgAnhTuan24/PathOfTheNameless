using System.Collections.Generic;
using UnityEngine;

public class ArenaSaveManager : MonoBehaviour
{
    public static ArenaSaveManager Instance;

    private HashSet<string> clearedArenas = new();
    private HashSet<string> activatedArenas = new();

    private Dictionary<string, ArenaProgressData> arenaProgress = new Dictionary<string, ArenaProgressData>();


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<string> GetClearedArenaIDs() => new(clearedArenas);
    public List<string> GetActivatedArenaIDs() => new(activatedArenas);

    public void LoadClearedArenas(List<string> ids)
    {
        clearedArenas = new HashSet<string>(ids);
    }

    public void LoadActivatedArenas(List<string> ids)
    {
        activatedArenas = new HashSet<string>(ids);
    }

    public bool IsArenaCleared(string id) => clearedArenas.Contains(id);
    public bool IsArenaActivated(string id) => activatedArenas.Contains(id);

    public void MarkActivated(string id)
    {
        activatedArenas.Add(id);
    }

    public void MarkCleared(string id)
    {
        clearedArenas.Add(id);
    }

    public void SetProgress(string id, int wave, bool bossSpawned, bool cleared)
    {
        arenaProgress[id] = new ArenaProgressData
        {
            arenaID = id,
            currentWave = wave,
            bossSpawned = bossSpawned,
            cleared = cleared
        };
    }

    public ArenaProgressData GetProgress(string id)
    {
        arenaProgress.TryGetValue(id, out var data);
        return data;
    }

    public List<ArenaProgressData> GetAllProgress()
    {
        return new List<ArenaProgressData>(arenaProgress.Values);
    }

    public void LoadProgress(List<ArenaProgressData> list)
    {
        arenaProgress.Clear();
        foreach (var p in list)
            arenaProgress[p.arenaID] = p;
    }

}
