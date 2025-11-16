using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    private static GameObject spawnedUIRoot;

    [Header("UI Root Prefab")]
    public GameObject uiRootPrefab;

    [Header("UI References")]
    public GameObject uiRoot;
    public GameObject inventoryPanel;
    public HealthBar healthBar;
    public ArmorBar armorBar;

    [Header("Inventory UI")]
    public List<Inventory_UI> inventoryUIs;
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();

    [Header("Alchemy UI")]
    public GameObject alchemyPanel;

    [Header("Player Stats UI")]
    public PlayerStatsUI playerStatsUI;

    [Header("Setting Game Play")]
    public GameObject settingGame;

    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;

    public bool IsInventoryOpen => inventoryPanel != null && inventoryPanel.activeSelf;

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
            armorBar = uiRoot.transform.Find("PlayerHUD/Giáp").GetComponent<ArmorBar>();

            playerStatsUI = uiRoot.transform.Find("PlayerStatsPanel").GetComponent<PlayerStatsUI>();
            playerStatsUI.gameObject.SetActive(false);

            var togglebtn = uiRoot.transform.Find("PlayerHUD/SettingButton/Button")?.GetComponent<Button>();
            if(togglebtn != null)
            {
                togglebtn.onClick.AddListener(() => TogglePlayerStats());
            }
            else
            {
                Debug.LogWarning("Không tìm thấy nút SettingButton để gán sự kiện TogglePlayerStats()");
            }

            alchemyPanel = uiRoot.transform.Find("AlchemyPanel")?.gameObject;
            if (alchemyPanel == null)
            {
                Debug.LogWarning("Không tìm thấy AlchemyPanel trong UI Root!");
            }
            else
            {
                alchemyPanel.SetActive(false);
            }

            settingGame = uiRoot.transform.Find("SettingGame")?.gameObject;
            if (settingGame == null)
            {
                Debug.LogWarning("Không tìm thấy SettingGamePanel trong UI Root!");
            }
            else
            {
                settingGame.SetActive(false);
            }

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

        if(Input.GetKeyDown(KeyCode.C))
        {
            TogglePlayerStats();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && settingGame != null)
        {
            ToggleSettingGame();
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

    public void OpenUI(GameObject uiToOpen)
    {
        // Ẩn tất cả UI khác
        if (alchemyPanel != null && alchemyPanel != uiToOpen) alchemyPanel.SetActive(false);
        if (inventoryPanel != null && inventoryPanel != uiToOpen) inventoryPanel.SetActive(false);
        if (playerStatsUI != null && playerStatsUI.gameObject != uiToOpen) playerStatsUI.gameObject.SetActive(false);
        if (settingGame != null && settingGame != uiToOpen) settingGame.SetActive(false);

        // Bật UI cần mở
        if (uiToOpen != null)
            uiToOpen.SetActive(true);
    }


    public void ToggleSettingGame()
    {
        if (settingGame != null)
        {
            if (!settingGame.activeSelf)
            {
                OpenUI(settingGame);
            }
            else
            {
                settingGame.SetActive(false);
            }
        }
    }


    public void TogglePlayerStats()
    {
        if (playerStatsUI != null)
        {
            if (!playerStatsUI.gameObject.activeSelf)
            {
                OpenUI(playerStatsUI.gameObject);
            }
            else
            {
                playerStatsUI.gameObject.SetActive(false);
            }
        }
    }


    public void OpenCloseInventoryUI()
    {
        if (inventoryPanel != null)
        {
            if (!inventoryPanel.activeSelf)
            {
                OpenUI(inventoryPanel);
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
