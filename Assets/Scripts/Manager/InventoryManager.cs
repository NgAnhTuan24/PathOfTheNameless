using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

    [Header("Backpack")]
    public Inventory backpack;
    public int backpackSlotCount;

    [Header("Toolbar")]
    public Inventory toolbar;
    public int toolbarSlotCount;

    private void Awake()
    {
        backpack = new Inventory(backpackSlotCount);
        toolbar = new Inventory(toolbarSlotCount);

        inventoryByName.Add("Backpack", backpack);
        inventoryByName.Add("Toolbar", toolbar);
    }

    private void Start()
    {
        DefaultTool();
    }

    private void DefaultTool()
    {
        Item kiem = GameManager.instance.itemManager.GetItemByName("Kiếm");
        Item riu = GameManager.instance.itemManager.GetItemByName("Rìu");
        Item cuoc = GameManager.instance.itemManager.GetItemByName("Cuốc");

        if(kiem != null)
        {
            toolbar.Add(kiem);
        }
        if(riu != null)
        {
            toolbar.Add(riu);
        }
        if(cuoc != null)
        {
            toolbar.Add(cuoc);
        }
    }

    public void Add(string inventoryName, Item item)
    {
        if(inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Add(item);
        }
    }

    public Inventory GetInventoryByName(string inventoryName)
    {
        if(inventoryByName.ContainsKey(inventoryName))
        {
            return inventoryByName[inventoryName];
        }

        return null;
    }
}
