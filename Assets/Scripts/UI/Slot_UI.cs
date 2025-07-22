using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_UI : MonoBehaviour
{
    public int slotID;
    public Inventory inventory;

    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    [SerializeField] private GameObject highlight;

    public ItemData itemData;

    public void SetItem(Inventory.Slot slot)
    {
        if(slot != null)
        {
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);

            if (IsTool(slot.itemName))
            {
                quantityText.text = "";
            }
            else
            {
                quantityText.text = slot.count.ToString();
            }
        }

        this.itemData = GameManager.instance.itemManager.GetItemByName(slot.itemName)?.data;
    }

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    public void SetHighlight(bool isOn)
    {
        highlight.SetActive(isOn);
    }

    private bool IsTool(string itemName)
    {
        return itemName == "Kiếm" || itemName == "Rìu" || itemName == "Cuốc";
    }
}
