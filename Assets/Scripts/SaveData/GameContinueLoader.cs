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

        Debug.Log("LOAD GAME HOÀN TẤT – Inventory + Player Position + Current Scene");
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
}