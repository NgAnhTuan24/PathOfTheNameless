using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlchemySlot_UI : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public Image icon;

    [SerializeField] private bool isResultSlot = false;

    private string itemName = "";

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
    }

    public void Clear()
    {
        itemName = "";
        icon.sprite = null;
        icon.color = new Color(1, 1, 1, 0);
    }

    public bool IsEmpty => itemName == "";

    // Xử lý khi thả item vào slot
    public void OnDrop(PointerEventData eventData)
    {
        if (isResultSlot)
        {
            Debug.Log("Không thể kéo thả vật phẩm vào result slot!");
            return;
        }

        if (UI_Manager.draggedSlot != null)
        {
            Inventory.Slot draggedInventorySlot = UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID];
            if (!draggedInventorySlot.IsEmpty)
            {
                ItemData itemData = GameManager.instance.itemManager.GetItemByName(draggedInventorySlot.itemName)?.data;
                if (itemData == null)
                    return;

                if (itemData.itemType != ItemType.None)
                {
                    Debug.Log($"{itemData.itemName} - {itemData.itemType} không thể bỏ vào lò luyện kim!");
                    return;
                }

                if (!IsEmpty)
                {
                    Debug.Log("Slot đã có item!");
                    return;
                }

                // Đặt item vào slot luyện kim
                SetItem(draggedInventorySlot.itemName, draggedInventorySlot.icon, draggedInventorySlot);

                // Giảm số lượng item trong inventory
                UI_Manager.draggedSlot.inventory.Remove(UI_Manager.draggedSlot.slotID, 1);

                // Refresh inventory UI
                GameManager.instance.uiManager.RefreshAll();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsEmpty)
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
                    GameManager.instance.player.inventory.Add("Toolbar", item);
                    Clear();
                    GameManager.instance.uiManager.RefreshAll();
                    Debug.Log($"Đã lấy vật phẩm vào toolbar");
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
