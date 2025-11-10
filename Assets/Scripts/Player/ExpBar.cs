using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBar : MonoBehaviour
{
    [Header("UI References")]
    public Slider slider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expAmountText;

    public void SetExp(int currentExp, int expToNextLevel)
    {
        if (slider != null)
        {
            slider.maxValue = expToNextLevel;
            slider.value = currentExp;
        }

        if (expAmountText != null)
        {
            expAmountText.text = $"{currentExp} / {expToNextLevel}";
        }
    }

    public void SetLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"Level: {level}";
        }
    }
}
