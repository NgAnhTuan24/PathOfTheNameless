using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData
{
    public string currentScene;
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    public int currentHealth;
    public int maxHealth;
    public int currentArmor;
    public int maxArmor;
    public float movementSpeed;
    public int damageAmount;

    public int currentLevel;
    public int currentExp;
    public int totalExp;
    public int expToNextLevel;
    public int skillPoints;

    public List<InventorySlotData> backpackSlots = new List<InventorySlotData>();

    public List<InventorySlotData> toolbarSlots = new List<InventorySlotData>();

    public List<string> openedChestIDs = new List<string>();

    public List<string> completedDialogueIDs = new List<string>();

    public List<string> removedTreeIDs = new List<string>();

    public List<TilledTile> tilledTiles = new();
    public List<CropSaveData> crops = new();

    public List<string> clearedArenaIDs = new();
    public List<string> activatedArenaIDs = new();
}

[Serializable]
public class InventorySlotData
{
    public string itemName = "";
    public int count = 0;
    public int maxAllowed = 99;

    public InventorySlotData() { }

    public InventorySlotData(Inventory.Slot slot)
    {
        if (slot != null && !slot.IsEmpty)
        {
            itemName = slot.itemName;
            count = slot.count;
            maxAllowed = slot.maxAllowed;
        }
    }
}

public static class GameSaver
{
    private const string SAVE_KEY = "PlayerSaveData";

    public static void SaveGame()
    {
        SaveData data = new SaveData
        {
            currentScene = SceneManager.GetActiveScene().name
        };

        // Lưu vị trí player
        if (PlayerController.Instance != null)
        {
            Vector3 pos = PlayerController.Instance.transform.position;
            data.playerPosX = pos.x;
            data.playerPosY = pos.y;
            data.playerPosZ = pos.z;
        }

        if (PlayerController.Instance != null)
        {
            var health = PlayerController.Instance.GetComponent<PlayerHealth>();
            var damage = PlayerController.Instance.GetComponentInChildren<PlayerDamage>();
            var levelSystem = PlayerController.Instance.GetComponent<PlayerLevelSystem>();

            if (health != null)
            {
                data.currentHealth = health.GetCurrentHealth();
                data.maxHealth = health.GetMaxHealth();
                data.currentArmor = health.GetCurrentArmor();
                data.maxArmor = health.GetMaxArmor();
            }

            if (damage != null)
            {
                data.damageAmount = damage.GetDamageAmount();
            }

            data.movementSpeed = PlayerController.Instance.GetMovementSpeed();

            if (levelSystem != null)
            {
                data.currentLevel = levelSystem.GetCurrentLevel();
                data.currentExp = levelSystem.GetCurrentExp();
                data.totalExp = levelSystem.GetTotalExp();
                data.expToNextLevel = levelSystem.GetExpToNextLevel();
                data.skillPoints = levelSystem.GetSkillPoints();
            }
        }

        // Lưu Backpack
        var backpack = GameManager.instance?.player?.inventory?.GetInventoryByName("Backpack");
        if (backpack != null)
        {
            data.backpackSlots.Clear();
            foreach (var slot in backpack.slots)
            {
                data.backpackSlots.Add(new InventorySlotData(slot));
            }
        }

        // Lưu Toolbar
        var toolbar = GameManager.instance?.player?.inventory?.GetInventoryByName("Toolbar");
        if (toolbar != null)
        {
            data.toolbarSlots.Clear();
            foreach (var slot in toolbar.slots)
            {
                data.toolbarSlots.Add(new InventorySlotData(slot));
            }
        }

        if (ChestSaveManager.Instance != null)
        {
            data.openedChestIDs.Clear();
            data.openedChestIDs.AddRange(ChestSaveManager.Instance.GetOpenedChestIDs());
        }

        if (DialogueSaveManager.Instance != null)
        {
            data.completedDialogueIDs.Clear();
            data.completedDialogueIDs.AddRange(
                DialogueSaveManager.Instance.GetCompletedDialogueIDs()
            );
        }

        if (TreeSaveManager.Instance != null)
        {
            data.removedTreeIDs.Clear();
            data.removedTreeIDs.AddRange(TreeSaveManager.Instance.GetRemovedTreeIDs());
        }

        if (TileManager.Instance != null)
        {
            data.tilledTiles = new List<TilledTile>(
                TileManager.Instance.GetTilledTiles()
            );

            data.crops = new List<CropSaveData>(
                TileManager.Instance.savedCrops
            );
        }

        if (ArenaSaveManager.Instance != null)
        {
            data.clearedArenaIDs = ArenaSaveManager.Instance.GetClearedArenaIDs();
            data.activatedArenaIDs = ArenaSaveManager.Instance.GetActivatedArenaIDs();
        }

        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("ĐÃ LƯU GAME THÀNH CÔNG!");
    }

    public static SaveData LoadGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return null;
    }

    public static bool HasSaveData() => PlayerPrefs.HasKey(SAVE_KEY);

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.DeleteKey("IsContinuing");
        PlayerPrefs.Save();
        Debug.Log("ĐÃ XÓA SAVE GAME CŨ");
    }

}