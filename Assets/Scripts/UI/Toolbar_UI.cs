using System.Collections.Generic;
using UnityEngine;

public class Toolbar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolbarSlots = new List<Slot_UI>();

    private Slot_UI selectedSlot;

    private void Start()
    {
        SelectSlot(0);
    }

    private void Update()
    {
        CheckAlphaNumericKeys();
    }
    public void SelectSlot(int index)
    {
        if (toolbarSlots.Count == 9)
        {
            if (selectedSlot != null)
            {
                selectedSlot.SetHighlight(false);
            }

            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true);
            //Debug.Log("Selected Slot: " + index);
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
}
