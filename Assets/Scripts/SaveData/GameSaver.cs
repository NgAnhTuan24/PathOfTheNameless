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

    // Inventory Backpack
    public List<InventorySlotData> backpackSlots = new List<InventorySlotData>();

    // Inventory Toolbar
    public List<InventorySlotData> toolbarSlots = new List<InventorySlotData>();

    public List<string> openedChestIDs = new List<string>();

    public List<string> completedDialogueIDs = new List<string>();
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

        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("ĐÃ LƯU GAME THÀNH CÔNG! (Backpack + Toolbar + Vị trí Player + Scene hiện tại + trạng thái mở rương + trạng thái hội thoại chỉ chạy được 1 lần)");
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