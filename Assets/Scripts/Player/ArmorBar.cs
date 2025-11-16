using UnityEngine;
using UnityEngine.UI;

public class ArmorBar : MonoBehaviour
{
    public Slider slider;
    public Text armorText;

    public void SetMaxArmor(int armor)
    {
        slider.maxValue = armor;
        //slider.value = armor;
        SetArmorText(armor);
    }

    public void SetArmor(int armor)
    {
        slider.value = armor;
        SetArmorText(armor);
    }

    private void SetArmorText(int currentArmor)
    {
        if (armorText != null)
        {
            armorText.text = currentArmor.ToString();
        }
    }
}
