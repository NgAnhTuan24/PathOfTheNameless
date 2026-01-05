using UnityEngine;
using System.Collections;

public class GameContinueLoader : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("IsContinuing", 0) == 1)
        {
            PlayerPrefs.DeleteKey("IsContinuing");
            PlayerPrefs.Save();

            SaveData data = GameSaver.LoadGame();
            if (data == null) return;

            if (ChestSaveManager.Instance != null)
            {
                ChestSaveManager.Instance.LoadOpenedChests(data.openedChestIDs);
            }

            if (DialogueSaveManager.Instance != null)
            {
                DialogueSaveManager.Instance.LoadCompletedDialogues(
                    data.completedDialogueIDs
                );
            }

            if (TreeSaveManager.Instance != null)
            {
                TreeSaveManager.Instance.LoadRemovedTrees(data.removedTreeIDs);
            }

            if (TileManager.Instance != null)
            {
                TileManager.Instance.LoadFromSave(data.tilledTiles, data.crops);
            }

            if (ArenaSaveManager.Instance != null)
            {
                ArenaSaveManager.Instance.LoadClearedArenas(data.clearedArenaIDs);
                ArenaSaveManager.Instance.LoadActivatedArenas(data.activatedArenaIDs);
            }


            // 1. Đặt lại vị trí player trước (an toàn)
            if (PlayerController.Instance != null)
            {
                Vector3 pos = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
                PlayerController.Instance.transform.position = pos;
            }

            // 2. Đợi TẤT CẢ mọi thứ sẵn sàng rồi mới load inventory + refresh UI
            StartCoroutine(LoadWhenEverythingIsReady(data));
        }
    }

    IEnumerator LoadWhenEverythingIsReady(SaveData data)
    {
        // Đợi GameManager, Player, InventoryManager, UI_Manager phải tồn tại
        while (GameManager.instance == null ||
               GameManager.instance.player == null ||
               GameManager.instance.player.inventory == null ||
               UI_Manager.Instance == null)
        {
            yield return null;
        }

        // Đợi thêm 1 frame nữa để tất cả Inventory_UI chạy Start() xong
        yield return new WaitForEndOfFrame();

        LoadInventory(data);

        // Bây giờ mới dám refresh UI – chắc chắn không null nữa
        UI_Manager.Instance.RefreshAll();

        ApplyPlayerStats(data);

        Debug.Log("LOAD GAME HOÀN TẤT");
    }

    void LoadInventory(SaveData data)
    {
        var inv = GameManager.instance.player.inventory;

        // Xóa sạch
        ClearInventory(inv.backpack);
        ClearInventory(inv.toolbar);

        // Load Backpack
        for (int i = 0; i < data.backpackSlots.Count && i < inv.backpack.slots.Count; i++)
        {
            ApplySlotData(inv.backpack.slots[i], data.backpackSlots[i]);
        }

        // Load Toolbar
        for (int i = 0; i < data.toolbarSlots.Count && i < inv.toolbar.slots.Count; i++)
        {
            ApplySlotData(inv.toolbar.slots[i], data.toolbarSlots[i]);
        }
    }

    void ApplySlotData(Inventory.Slot slot, InventorySlotData data)
    {
        if (!string.IsNullOrEmpty(data.itemName))
        {
            var item = GameManager.instance.itemManager.GetItemByName(data.itemName);
            if (item != null)
            {
                slot.itemName = data.itemName;
                slot.icon = item.data.icon;
                slot.count = data.count;
                slot.maxAllowed = data.maxAllowed;
            }
        }
        else
        {
            slot.itemName = "";
            slot.icon = null;
            slot.count = 0;
        }
    }

    void ClearInventory(Inventory inventory)
    {
        foreach (var slot in inventory.slots)
        {
            slot.itemName = "";
            slot.count = 0;
            slot.icon = null;
        }
    }

    private void ApplyPlayerStats(SaveData data)
    {
        if (PlayerController.Instance == null) return;

        var health = PlayerController.Instance.GetComponent<PlayerHealth>();
        var damage = PlayerController.Instance.GetComponentInChildren<PlayerDamage>();
        var levelSystem = PlayerController.Instance.GetComponent<PlayerLevelSystem>();

        if (health != null)
        {
            health.IncreaseMaxHealth(data.maxHealth - health.GetMaxHealth());
                                                                              
            health.GetType().GetField("maxHeath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(health, data.maxHealth);
            health.GetType().GetField("currentHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(health, data.currentHealth);

            health.GetType().GetField("maxArmor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(health, data.maxArmor);
            health.GetType().GetField("currentArmor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(health, data.currentArmor);

            // Cập nhật UI bar
            if (health.healthBar) health.healthBar.SetMaxHealth(data.maxHealth);
            if (health.healthBar) health.healthBar.SetHealth(data.currentHealth);
            if (health.armorBar) health.armorBar.SetMaxArmor(data.maxArmor);
            if (health.armorBar) health.armorBar.SetArmor(data.currentArmor);
        }

        if (damage != null)
        {
            damage.SetDamageAmount(data.damageAmount);
        }

        PlayerController.Instance.SetMovementSpeed(data.movementSpeed);

        if (levelSystem != null)
        {
            var levelField = levelSystem.GetType().GetField("currentLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var expField = levelSystem.GetType().GetField("currentExp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var totalExpField = levelSystem.GetType().GetField("totalExp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var expToNextField = levelSystem.GetType().GetField("expToNextLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var skillPointsField = levelSystem.GetType().GetField("skillPoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            levelField?.SetValue(levelSystem, data.currentLevel);
            expField?.SetValue(levelSystem, data.currentExp);
            totalExpField?.SetValue(levelSystem, data.totalExp);
            expToNextField?.SetValue(levelSystem, data.expToNextLevel);
            skillPointsField?.SetValue(levelSystem, data.skillPoints);

            if (levelSystem.GetType().GetField("expBar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                .GetValue(levelSystem) is ExpBar expBar && expBar != null)
            {
                expBar.SetExp(data.currentExp, data.expToNextLevel);
                expBar.SetLevel(data.currentLevel);
            }
        }

        GameEvents.ChangedStats();
    }
}