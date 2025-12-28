using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSaveManager : MonoBehaviour
{
    public static ChestSaveManager Instance;

    private HashSet<string> openedChestIDs = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsChestOpened(string id)
    {
        return openedChestIDs.Contains(id);
    }

    public void MarkChestAsOpened(string id)
    {
        openedChestIDs.Add(id);
    }

    public IEnumerable<string> GetOpenedChestIDs()
    {
        return openedChestIDs;
    }

    public void LoadOpenedChests(List<string> ids)
    {
        openedChestIDs.Clear();
        if (ids != null)
        {
            foreach (var id in ids)
            {
                openedChestIDs.Add(id);
            }
        }
        Debug.Log($"Đã load {openedChestIDs.Count} rương đã mở từ save");
    }
}
