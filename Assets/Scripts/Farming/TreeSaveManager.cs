using System.Collections.Generic;
using UnityEngine;

public class TreeSaveManager : MonoBehaviour
{
    public static TreeSaveManager Instance;

    private HashSet<string> removedTrees = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void MarkTreeAsRemoved(string id)
    {
        removedTrees.Add(id);
    }

    public bool IsTreeRemoved(string id)
    {
        return removedTrees.Contains(id);
    }

    public List<string> GetRemovedTreeIDs()
    {
        return new List<string>(removedTrees);
    }

    public void LoadRemovedTrees(List<string> ids)
    {
        removedTrees.Clear();
        removedTrees.UnionWith(ids);
        Debug.Log($"Đã load {removedTrees.Count} cây đã chặt từ save");
    }

}
