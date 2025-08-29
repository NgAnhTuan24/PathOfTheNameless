using UnityEngine;
using System;

public class GenerateID : MonoBehaviour
{
    [SerializeField] private string uniqueID;

    private void Awake()
    {
        if (ItemSaveManager.instance != null && ItemSaveManager.instance.IsRemoved(uniqueID))
        {
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
        {
            uniqueID = "";
        }
        else if (string.IsNullOrEmpty(uniqueID) || IsDuplicateID(uniqueID))
        {
            CreateID();
        }
    }

    private bool IsDuplicateID(string id)
    {
        GenerateID[] all = FindObjectsOfType<GenerateID>();
        int count = 0;
        foreach (var u in all)
        {
            if (u.uniqueID == id) count++;
            if (count > 1) return true;
        }
        return false;
    }
#endif

    public void CreateID()
    {
        string objName = gameObject.name;
        string number = Guid.NewGuid().ToString("N").Substring(0, 3);
        uniqueID = $"{objName}_{number}";
    }

    public void MarkAsRemoved()
    {
        ItemSaveManager.instance.MarkAsRemoved(uniqueID);
    }

    public string GetID() => uniqueID;
}
