using UnityEngine;
using UnityEngine.UI;

public class AlchemyUI : MonoBehaviour
{
    public AlchemySlot_UI[] ingredientSlots; // size = 3
    public AlchemySlot_UI resultSlot;

    public Button mixButton;

    private void Awake()
    {
        if (ingredientSlots == null || ingredientSlots.Length != 3)
            Debug.LogError("IngredientSlots không được gán đúng hoặc không đủ 3 slot", gameObject);
        if (resultSlot == null)
            Debug.LogError("ResultSlot bị null", gameObject);
        if (mixButton == null)
            Debug.LogError("MixButton bị null", gameObject);
    }

    private void Start()
    {
        mixButton.onClick.AddListener(OnMix);
    }

    void OnMix()
    {
        // lấy danh sách tên item nguyên liệu
        string[] ingredients = new string[3];

        for (int i = 0; i < 3; i++)
        {
            ingredients[i] = ingredientSlots[i].ItemName;
            if (ingredients[i] == "")
            {
                Debug.Log("Thiếu nguyên liệu");
                return;
            }
        }

        // Kiểm tra công thức
        ItemData resultItem = AlchemyRecipeData.Instance.GetResult(ingredients);

        if (resultItem == null)
        {
            Debug.Log("Sai công thức, boom 💥");
            ClearIngredients();
            resultSlot.Clear();
            return;
        }

        // Nếu đúng công thức → show kết quả
        resultSlot.SetItem(resultItem.itemName, resultItem.icon);
        ClearIngredients();

        Debug.Log("Luyện thành công!");
    }

    void ClearIngredients()
    {
        foreach (var slot in ingredientSlots)
        {
            slot.Clear();
        }
    }
}
