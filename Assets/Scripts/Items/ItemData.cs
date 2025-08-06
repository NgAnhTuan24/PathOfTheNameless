﻿using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    public string itemName = "Item Name";
    public Sprite icon;

    public ItemType itemType = ItemType.None;
    public ToolType toolType = ToolType.None;

    public GameObject cropPrefab; // Prefab cây để sinh ra khi trồng
}

public enum ItemType 
{
    None, 
    congCu, 
    kiem, 
    hatGiong, 
}

public enum ToolType
{
    None,
    Hoe,
    Axe,
}