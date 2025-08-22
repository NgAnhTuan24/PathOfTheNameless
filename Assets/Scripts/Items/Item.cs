using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    public ItemData data;

    [SerializeField]
    private string itemID;

    [HideInInspector]
    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (ItemSaveManager.instance != null && ItemSaveManager.instance.IsRemoved(itemID))
        {
            Destroy(gameObject);
        }
    }

    private void OnValidate()
    {
        if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
        {
            itemID = "";
        }
        else if (string.IsNullOrEmpty(itemID) || IsDuplicateID(itemID))
        {
            itemID = GenerateNewID();
        }
    }

    private string GenerateNewID()
    {
        string itemName = data != null ? data.itemName : gameObject.name;
        string number = Guid.NewGuid().ToString("N").Substring(0, 3);
        return $"{itemName}_{number}";
    }

    public void MarkAsRemoved()
    {
        ItemSaveManager.instance.MarkAsRemoved(itemID);
    }

    private bool IsDuplicateID(string id)
    {
        Item[] allItems = FindObjectsOfType<Item>();
        int count = 0;
        foreach (var item in allItems)
        {
            if (item.itemID == id) count++;
            if (count > 1) return true; // đã thấy trùng
        }
        return false;
    }

    public string GetID() => itemID;
}
