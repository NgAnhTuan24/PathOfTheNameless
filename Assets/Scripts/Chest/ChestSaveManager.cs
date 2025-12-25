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
}
