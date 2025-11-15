using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlchemySlot_UI : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public Image icon;
    private string itemName = "";
    private Inventory.Slot inventorySlot;

    public string ItemName => itemName;

    private void Awake()
    {
        if (icon == null)
        {
            Debug.LogError($"Icon bị null trên {gameObject.name}", gameObject);
        }

        Clear();
    }

    public void SetItem(string name, Sprite sprite, Inventory.Slot slot = null)
    {
        itemName = name;
        icon.sprite = sprite;
        icon.color = Color.white;
        inventorySlot = slot;
    }

    public void Clear()
    {
        itemName = "";
        icon.sprite = null;
        icon.color = new Color(1, 1, 1, 0);
        inventorySlot = null;
    }

    public bool IsEmpty => itemName == "";

    // Xử lý khi thả item vào slot
    public void OnDrop(PointerEventData eventData)
    {
        if (UI_Manager.draggedSlot != null)
        {
            Inventory.Slot draggedInventorySlot = UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID];
            if (!draggedInventorySlot.IsEmpty)
            {
                if (!IsEmpty)
                {
                    Debug.Log("Slot đã có item!");
                    return;
                }

                // Đặt item vào slot luyện kim
                SetItem(draggedInventorySlot.itemName, draggedInventorySlot.icon, draggedInventorySlot);

                // Giảm số lượng item trong inventory
                if (UI_Manager.dragSingle)
                {
                    UI_Manager.draggedSlot.inventory.Remove(UI_Manager.draggedSlot.slotID, 1);
                }
                else
                {
                    UI_Manager.draggedSlot.inventory.Remove(UI_Manager.draggedSlot.slotID, draggedInventorySlot.count);
                }

                // Refresh inventory UI
                GameManager.instance.uiManager.RefreshAll();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsEmpty && inventorySlot == null)
        {
            Item item = GameManager.instance.itemManager.GetItemByName(itemName);
            if (item != null)
            {
                Inventory backpack = GameManager.instance.player.inventory.GetInventoryByName("Backpack");
                bool canAdd = false;
                foreach (var slot in backpack.slots)
                {
                    if (slot.IsEmpty || (slot.itemName == itemName && slot.count < slot.maxAllowed))
                    {
                        canAdd = true;
                        break;
                    }
                }
                if (canAdd)
                {
                    GameManager.instance.player.inventory.Add("Backpack", item);
                    Clear();
                    GameManager.instance.uiManager.RefreshAll();
                    Debug.Log($"Đã lấy {itemName} vào inventory");
                }
                else
                {
                    Debug.Log("Inventory đầy!");
                    // Có thể hiển thị thông báo trên UI
                }
            }
        }
    }
}
