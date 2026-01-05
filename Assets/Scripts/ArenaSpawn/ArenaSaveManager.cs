using System.Collections.Generic;
using UnityEngine;

public class ArenaSaveManager : MonoBehaviour
{
    public static ArenaSaveManager Instance;

    private HashSet<string> clearedArenas = new();
    private HashSet<string> activatedArenas = new();

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

    // ===== SAVE =====
    public List<string> GetClearedArenaIDs() => new(clearedArenas);
    public List<string> GetActivatedArenaIDs() => new(activatedArenas);

    // ===== LOAD =====
    public void LoadClearedArenas(List<string> ids)
    {
        clearedArenas = new HashSet<string>(ids);
    }

    public void LoadActivatedArenas(List<string> ids)
    {
        activatedArenas = new HashSet<string>(ids);
    }

    // ===== QUERY =====
    public bool IsArenaCleared(string id) => clearedArenas.Contains(id);
    public bool IsArenaActivated(string id) => activatedArenas.Contains(id);

    // ===== MARK =====
    public void MarkActivated(string id)
    {
        activatedArenas.Add(id);
    }

    public void MarkCleared(string id)
    {
        clearedArenas.Add(id);
    }
}
