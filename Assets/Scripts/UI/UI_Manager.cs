﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    // ------------------- Singleton -------------------
    public static UI_Manager Instance;
    private static GameObject spawnedUIRoot;

    // ------------------- Prefab & References -------------------
    [Header("UI Root Prefab")]
    public GameObject uiRootPrefab;

    [Header("UI References")]
    public GameObject uiRoot;
    public GameObject inventoryPanel;
    public HealthBar healthBar;

    [Header("Inventory UI")]
    public List<Inventory_UI> inventoryUIs;
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();

    // ------------------- Dragging -------------------
    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (spawnedUIRoot == null)
        {
            // Spawn prefab UI_Root 1 lần duy nhất
            spawnedUIRoot = Instantiate(uiRootPrefab);
            DontDestroyOnLoad(spawnedUIRoot);

            uiRoot = spawnedUIRoot;

            // Gán reference tới các object con
            inventoryPanel = uiRoot.transform.Find("Kho đồ/InventoryPanel").gameObject;
            healthBar = uiRoot.transform.Find("PlayerHUD/Thanh Máu").GetComponent<HealthBar>();

            // Lấy hết Inventory_UI trong prefab
            inventoryUIByName.Clear();
            inventoryUIs.Clear();
            foreach (Inventory_UI ui in uiRoot.GetComponentsInChildren<Inventory_UI>(true))
            {
                inventoryUIs.Add(ui);
            }
            Initialize();
        }
        else
        {
            // Nếu UI đã spawn rồi thì dùng lại
            uiRoot = spawnedUIRoot;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            OpenCloseInventoryUI();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            dragSingle = true;
        }
        else
        {
            dragSingle = false;
        }
    }

    public void OpenCloseInventoryUI()
    {
        if (inventoryPanel != null)
        {
            if (!inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(true);
                RefreshInventoryUI("Backpack");
            }
            else
            {
                inventoryPanel.SetActive(false);
            }
        }
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    public void RefreshAll()
    {
        foreach(KeyValuePair<string, Inventory_UI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }

    public Inventory_UI GetInventoryUI(string inventoryName)
    {
        if(inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }

        Debug.LogWarning("There is no inventory ui for " + inventoryName);
        return null;
    }

    void Initialize()
    {
        foreach(Inventory_UI ui in inventoryUIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }
}
