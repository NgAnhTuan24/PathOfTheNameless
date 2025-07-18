using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;

    public GameObject balo;
 
    public Player player;

    public List<Slot_UI> slots = new List<Slot_UI>();

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            OpenCloseInventory();
        }
    }

    public void OpenCloseInventory()
    {
        if (!inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(true);
            balo.SetActive(false);
            Setup();
        }
        else
        {
            inventoryPanel.SetActive(false);
            balo.SetActive(true);
        }
    }

    void Setup()
    {
        if (slots.Count == player.inventory.slots.Count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (player.inventory.slots[i].type != CollectableType.NONE)
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }
}
