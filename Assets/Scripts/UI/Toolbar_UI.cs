using System.Collections.Generic;
using UnityEngine;

public class Toolbar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolbarSlots = new List<Slot_UI>();

    private Slot_UI selectedSlot;
    private int currentIndex = 0;

    private void Start()
    {
        SelectSlot(0);
    }

    private void Update()
    {
        CheckAlphaNumericKeys();
        CheckScrollWheel();
    }
    public void SelectSlot(int index)
    {
        if (toolbarSlots.Count == 9)
        {
            if (selectedSlot != null)
            {
                selectedSlot.SetHighlight(false);
            }

            currentIndex = Mathf.Clamp(index, 0, toolbarSlots.Count - 1); // Giữ index trong giới hạn
            selectedSlot = toolbarSlots[currentIndex];
            selectedSlot.SetHighlight(true);
        }
    }

    private void CheckAlphaNumericKeys()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectSlot(6);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectSlot(7);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            SelectSlot(8);
        }
    }

    private void CheckScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0f)
        {
            // Cuộn xuống
            int newIndex = (currentIndex + 1) % toolbarSlots.Count;
            SelectSlot(newIndex);
        }
        else if (scroll > 0f)
        {
            // Cuộn lên
            int newIndex = (currentIndex - 1 + toolbarSlots.Count) % toolbarSlots.Count;
            SelectSlot(newIndex);
        }
    }

    public Inventory.Slot GetSelectedSlot()
    {
        return selectedSlot?.inventory?.slots[selectedSlot.slotID];
    }
}
